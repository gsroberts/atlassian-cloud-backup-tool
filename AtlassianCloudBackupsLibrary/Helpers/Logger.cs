namespace AtlassianCloudBackupsLibrary.Helpers
{
    using System;
    using System.IO;

    /// <summary>
    /// Logging helper
    /// 
    /// TODO: Refactor in a less write intensive way so that memory consumption stays low and the
    /// number of writes to the log file is reduced.
    /// </summary>
    public class Logger
    {
        private string _logLocation;
        private string _logFile;

        private static Logger _logger;
        public static Logger Current
        {
            get
            {
                if (_logger == null)
                {
                    _logger = new Logger();
                }
                return _logger;
            }
        }

        public void Init(string logPath, string logFileName)
        {
            _logLocation = logPath;
            _logFile = logFileName;
        }

        public void Log(string topic, string entry)
        {
            var logEntry = string.Format("{0} [{1}] {2}", DateTime.Now.ToString(), topic, entry);

            File.AppendAllLines(Path.Combine(_logLocation, _logFile), new[] { logEntry });
            Console.WriteLine(logEntry);
        }
    }
}
