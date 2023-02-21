namespace SpecFlowPdfReader.Helpers
{
    public static class Logger
    {
        public static void WrtieLog(string testName, string message)
        {
            string logDirectory = JsonHelper.GetValues().logPath;
            string logFileName = $"{DateTime.Now:yyyy-MM-dd_HH-mm-ss}_{testName}.txt";
            string logFilePath = Path.Combine(logDirectory, logFileName);

            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine($"{DateTime.Now}:{message}\n_____");
            }
        }
    }
}