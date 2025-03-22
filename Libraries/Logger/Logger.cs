using TunnelServer.Libraries.Setting;

namespace TunnelServer.Libraries.Logger
{
    public class Logger
    {
        private readonly Dictionary<LogStatus, string> _logStatusTitles;
        private readonly Dictionary<LogStatus, ConsoleColor> _logStatusColors;
        private SettingLoader? _setting;

        /// <summary>
        /// Initializes the logger with predefined log status titles and colors.
        /// </summary>
        public Logger()
        {
            // Initialize Log Status Titles
            this._logStatusTitles = new Dictionary<LogStatus, string>
            {
                { LogStatus.Debug, "DBUG" },
                { LogStatus.Log, "LOG" },
                { LogStatus.Error, "ERR" },
                { LogStatus.Success,"SCSS" },
                { LogStatus.Warning, "WARN" },
            };

            // Initialize Log Status Colors
            this._logStatusColors = new Dictionary<LogStatus, ConsoleColor>
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
            Console.ForegroundColor = this._logStatusColors.GetValueOrDefault(logStatus, ConsoleColor.Gray);
            Console.WriteLine($"[TunnelServer] {DateTime.Now:yyyy/MM/dd-HH:mm} [{this._logStatusTitles.GetValueOrDefault(logStatus, "UNKN")}]{(this._logStatusTitles.GetValueOrDefault(logStatus, "UNKN").Length == 3 ? " " : "")}: {text}");
            Console.ResetColor();
        }

        /// <summary>
        /// Set setting for logger
        /// </summary>
        /// <param name="setting"></param>
        public void Setting(SettingLoader setting)
        {
            this._setting = setting;
        }

        /// <summary>
        /// Logs a debug message.
        /// </summary>
        /// <param name="message">The debug message to log.</param>
        public void Debug(string message)
        {
            if ((_setting != null) &&
            (_setting.Logging != null) &&
            (_setting.Logging.Level != LogLevel.Debug)
            )
            {
                return;
            }
            this.PrintLog(message, LogStatus.Debug);
        }

        /// <summary>
        /// Logs a general log message.
        /// </summary>
        /// <param name="message">The log message to log.</param>
        public void Log(string message)
        {
            if ((_setting != null) &&
            (_setting.Logging != null) &&
            (_setting.Logging.Level != LogLevel.Debug)
            )
            {
                return;
            }
            this.PrintLog(message, LogStatus.Log);
        }

        /// <summary>
        /// Logs an error message.
        /// </summary>
        /// <param name="message">The error message to log.</param>
        public void Error(string message)
        {
            if ((_setting != null) &&
            (_setting.Logging != null) &&
            (_setting.Logging.Level != LogLevel.Error) &&
            (_setting.Logging.Level != LogLevel.Debug)
            )
            {
                return;
            }
            this.PrintLog(message, LogStatus.Error);
        }

        /// <summary>
        /// Logs a warning message.
        /// </summary>
        /// <param name="message">The warning message to log.</param>
        public void Warning(string message)
        {
            if ((_setting != null) &&
            (_setting.Logging != null) &&
            (_setting.Logging.Level != LogLevel.Debug)
            )
            {
                return;
            }
            this.PrintLog(message, LogStatus.Warning);
        }

        /// <summary>
        /// Logs a success message.
        /// </summary>
        /// <param name="message">The warning message to log.</param>
        public void Success(string message)
        {
            if ((_setting != null) &&
            (_setting.Logging != null) &&
            (_setting.Logging.Level != LogLevel.Debug)
            )
            {
                return;
            }
            this.PrintLog(message, LogStatus.Success);
        }
    }
}
