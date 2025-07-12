
namespace FMPilot2Automation.Extension
{
    public static class ReportLogger
    {
        public static void Info(string message) => TestContext.Progress.WriteLine($"[INFO] {message}");
        public static void Error(string message) => TestContext.Error.WriteLine($"[ERROR] {message}");
        public static void Attach(string filePath, string title = "")
        {
            TestContext.AddTestAttachment(filePath, title);
            Info($"Attached file: {filePath}");
        }
    }
}
