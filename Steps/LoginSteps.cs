using FMPilot2Automation.Extension;
using Microsoft.Playwright;
using Newtonsoft.Json.Linq;
using ReportPortal.Client.Abstractions.Responses;
using Reqnroll;
using System.Diagnostics.Metrics;

namespace FMPilot2Automation.Steps
{
    [Binding]
    public class LoginSteps
    {
        readonly IPage _page;

        public LoginSteps(ScenarioContext scenarioContext)
        {
            _page = scenarioContext.Get<IPage>();
        }

        [Given(@"I navigate to ""(.*)""")]
        public async Task GivenINavigateTo(string url)
        {
            await _page.GotoAsync(url);
            await _page.GotoAsync(url, new() { WaitUntil = WaitUntilState.NetworkIdle });
            // Wait for 3 seconds
            await _page.WaitForTimeoutAsync(3000);
        }

        [When(@"I enter username ""(.*)"" and password ""(.*)""")]
        public async Task WhenIEnterCredentials(string user, string pass)
        {

            // await _page.FillAsync("#txtLoginUserName", user);
            // await _page.FillAsync("#txtLoginPassword", pass);
                       

            // Wait until the input appears in the DOM
            await _page.WaitForSelectorAsync("input[name='txtLoginUserName']", new() { State = WaitForSelectorState.Visible });
            await _page.FillAsync("input[name='txtLoginUserName']", user);

            await _page.WaitForSelectorAsync("input[name='txtLoginPassword']");
            await _page.FillAsync("input[name='txtLoginPassword']", pass);


            //Locator.FillAsync() automatically waits for the element to be:Attached to the DOM and Visible and Enabled

            var usernameField = _page.Locator("input[name='txtLoginUserName']");
            await usernameField.FillAsync(user);
            var userpassField = _page.Locator("input[name='txtLoginPassword']");
            await userpassField.FillAsync(pass);


            // Set file to upload
            await _page.SetInputFilesAsync("#upload", @"C:\path\to\your\file.pdf");
            await _page.SetInputFilesAsync("#upload", new[] {@"C:\files\file1.jpg", @"C:\files\file2.pdf"});
            //you can check the value of the input(after upload):
            var uploaded = await _page.EvalOnSelectorAsync<string>("#upload", "el => el.files[0].name");


            // Optionally check count:
            var rows = await _page.QuerySelectorAllAsync("table tbody tr");

            ReportLogger.Info("Entered credentials");
        }

        [When(@"I click the login button")]
        public async Task WhenIClickLogin()
        {
            await _page.ClickAsync("#btnLogin");
            ReportLogger.Info("Clicked the login button on FM Pilot 2 login page.");
            TestContext.Progress.WriteLine("Clicked login button");
        }

        [Then(@"I should be redirected to the dashboard")]
        public async Task ThenISeeDashboard()
        {
            await _page.WaitForURLAsync("**/dashboard");
            var title = await _page.InnerTextAsync("h1");
            ReportLogger.Info($"Dashboard heading: {title}");
            Assert.That(await _page.Locator("h1").TextContentAsync(), Does.Contain("Dashboard"));
        }
    }
}
