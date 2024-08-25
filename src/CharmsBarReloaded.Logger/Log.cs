using NLog;

namespace CharmsBarReloaded
{
    public static class Log
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public static void Info(string message)
        {
            logger.Info(message);
        }
        public static void Warn(string message)
        {
            logger.Warn(message);
        }
        public static void Error(string message)
        {
            logger.Error(message);
        }
        public static void Fatal(Exception e, string message)
        {
            logger.Fatal($"{message}\n{e.Message}\n{e.Data}\n{e.Source}");
        }
        public static void Debug(string message)
        {
            logger.Debug(message);
        }
        public static void ClearPreviousLog()
        {
            string logFilePath =$"{Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CharmsBarReloaded"), "latest.log")}";

            // Clear the log file
            if (File.Exists(logFilePath))
            {
                File.WriteAllText(logFilePath, string.Empty);
            }
        }
    }
}
