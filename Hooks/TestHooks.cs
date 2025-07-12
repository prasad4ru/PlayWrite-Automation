using FMPilot2Automation.Extension;
using Microsoft.Playwright;
using Reqnroll;


namespace FMPilot2Automation.Hooks
{
    [Binding]
    public class TestHooks
    {
        static IBrowser _browser;
        private IPage _page;

        [BeforeTestRun]
        public static async Task BeforeTestRun()
        {
            var pw = await Playwright.CreateAsync();
            _browser = await pw.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true });
        }

        [BeforeScenario]
        public async Task BeforeScenario(ScenarioContext ctx)
        {
            var context = await _browser.NewContextAsync();
            _page = await context.NewPageAsync();
            ctx.Set(_page);
        }

        [AfterScenario]
        public async Task AfterScenario(ScenarioContext ctx)
        {
            var page = ctx.Get<IPage>();

            // Save screenshot
            var screenshotPath = Path.Combine(Directory.GetCurrentDirectory(), $"screenshot_{Guid.NewGuid()}.png");
            await page.ScreenshotAsync(new PageScreenshotOptions { Path = screenshotPath });

            // Attach screenshot to NUnit test output (which ReportPortal picks up)
            TestContext.AddTestAttachment(screenshotPath, "Scenario screenshot");
            TestContext.Progress.WriteLine("Screenshot attached: " + screenshotPath);
            ReportLogger.Attach(screenshotPath, "Scenario Screenshot");
            await page.Context.CloseAsync();
        }

        [AfterTestRun]
        public static async Task AfterTestRun() =>
            await _browser.CloseAsync();
    }
}
