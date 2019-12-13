using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuiLib.Logger
{
    class ConsoleLogger : ALogger
    {
        public ConsoleLogger(LogLevel level) : base(level)
        {

        }

        protected override void LogMessage(string message, LogLevel logLevel)
        {
            if(logLevel == LogLevel.Error)
            {
                Console.WriteLine("ERROR:   " + message);
            }else if (logLevel == LogLevel.Warning)
            {
                Console.WriteLine("Warning: " + message);
            }else if (logLevel == LogLevel.Info)
            {
                Console.WriteLine("Info:    " + message);
            }
        }

        protected override void Terminate()
        {
            Console.ReadKey();
            Environment.Exit(-1);
        }
    }
}
