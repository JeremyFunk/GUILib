using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuiLib.Logger
{
    public enum LogLevel
    {
        Info, Warning, Error
    }

    abstract class ALogger
    {
        public static readonly ALogger defaultLogger = new ConsoleLogger(LogLevel.Info);

        public LogLevel loggerLevel;

        public ALogger(LogLevel loggerLevel)
        {
            this.loggerLevel = loggerLevel;
        }

        public void Log(string message, LogLevel logLevel)
        {
            if((loggerLevel == LogLevel.Error && logLevel == LogLevel.Error) ||
                (loggerLevel == LogLevel.Warning && (logLevel == LogLevel.Error || logLevel == LogLevel.Warning)) ||
                loggerLevel == LogLevel.Info)
            {
                LogMessage(message, logLevel);
            }
        }

        protected abstract void LogMessage(string message, LogLevel logLevel);
    }
}
