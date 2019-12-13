using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuiLib.Logger
{
    public enum LogLevel
    {
        Info, Warning, Error, FatalError
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
            if((loggerLevel == LogLevel.FatalError && (logLevel == LogLevel.FatalError)) ||
                (loggerLevel == LogLevel.Error && (logLevel == LogLevel.Error || logLevel == LogLevel.FatalError)) ||
                (loggerLevel == LogLevel.Warning && (logLevel == LogLevel.Error || logLevel == LogLevel.Warning || logLevel == LogLevel.FatalError)) ||
                loggerLevel == LogLevel.Info)
            {
                LogMessage(message, logLevel);

                if(logLevel == LogLevel.FatalError)
                {
                    LogMessage("The program has ran into a fatal error and will exit now!", LogLevel.FatalError);
                    Terminate();
                }
            }
        }

        protected abstract void LogMessage(string message, LogLevel logLevel);
        protected abstract void Terminate();
    }
}
