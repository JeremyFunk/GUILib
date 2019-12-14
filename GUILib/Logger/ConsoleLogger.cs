using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUILib.Logger
{
    class ConsoleLogger : ALogger
    {
        public ConsoleLogger(LogLevel level) : base(level)
        {

        }

        protected override void LogMessage(string message, LogLevel logLevel)
        {
            string print = GetTimestamp() + " ";

            if (logLevel == LogLevel.Error)
            {
                print += "Error:   ";
            }else if (logLevel == LogLevel.Warning)
            {
                print += "Warning: ";
            }
            else if (logLevel == LogLevel.Info)
            {
                print += "Info:    ";
            }

            print += message;

            Console.WriteLine(print);
        }

        protected override void Terminate()
        {
            Console.ReadKey();
            Environment.Exit(-1);
        }


        private string GetTimestamp()
        {
            return DateTime.Now.ToString("[HH:mm:ss]");
        }
    }
}
