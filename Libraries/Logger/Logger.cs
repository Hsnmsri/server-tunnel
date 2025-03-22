namespace TunnelServer.Libraries.Logger
{
    /// <summary>
    /// Represents different log status levels.
    /// </summary>
    enum LogStatus
    {
        Log,
        Debug,
        Success,
        Error,
        Warning,
    }

    public class Logger
    {
        private Dictionary<LogStatus, string> LogStatusTitles;
        private Dictionary<LogStatus, ConsoleColor> LogStatusColors;

        /// <summary>
        /// Initializes the logger with predefined log status titles and colors.
        /// </summary>
        public Logger()
        {
            // Initialize Log Status Titles
            this.LogStatusTitles = new Dictionary<LogStatus, string>
            {
                { LogStatus.Debug, "DBUG" },
                { LogStatus.Log, "LOG" },
                { LogStatus.Error, "ERR" },
                { LogStatus.Success,"SCSS" },
                { LogStatus.Warning, "WARN" },
            };

            // Initialize Log Status Colors
            this.LogStatusColors = new Dictionary<LogStatus, ConsoleColor>
            {
                { LogStatus.Log, ConsoleColor.Gray },
                { LogStatus.Debug, ConsoleColor.DarkGray },
                { LogStatus.Warning, ConsoleColor.DarkYellow },
                { LogStatus.Error, ConsoleColor.DarkRed },
                { LogStatus.Success, ConsoleColor.DarkGreen },
            };
        }

        /// <summary>
        /// Prints a formatted log message to the console with the appropriate color.
        /// </summary>
        /// <param name="text">The message to log.</param>
        /// <param name="logStatus">The log status level.</param>
        private void PrintLog(string text, LogStatus logStatus)
        {
            Console.ForegroundColor = this.LogStatusColors.GetValueOrDefault(logStatus, ConsoleColor.Gray);
            Console.WriteLine($"[TunnelServer] {DateTime.Now:yyyy/MM/dd-HH:mm} [{this.LogStatusTitles.GetValueOrDefault(logStatus, "UNKN")}]{(this.LogStatusTitles.GetValueOrDefault(logStatus, "UNKN").Length == 3 ? " " : "")}: {text}");
            Console.ResetColor();
        }

        /// <summary>
        /// Logs a debug message.
        /// </summary>
        /// <param name="message">The debug message to log.</param>
        public void Debug(string message)
        {
            this.PrintLog(message, LogStatus.Debug);
        }

        /// <summary>
        /// Logs a general log message.
        /// </summary>
        /// <param name="message">The log message to log.</param>
        public void Log(string message)
        {
            this.PrintLog(message, LogStatus.Log);
        }

        /// <summary>
        /// Logs an error message.
        /// </summary>
        /// <param name="message">The error message to log.</param>
        public void Error(string message)
        {
            this.PrintLog(message, LogStatus.Error);
        }

        /// <summary>
        /// Logs a warning message.
        /// </summary>
        /// <param name="message">The warning message to log.</param>
        public void Warning(string message)
        {
            this.PrintLog(message, LogStatus.Warning);
        }

        /// <summary>
        /// Logs a success message.
        /// </summary>
        /// <param name="message">The warning message to log.</param>
        public void Success(string message)
        {
            this.PrintLog(message, LogStatus.Success);
        }
    }
}
