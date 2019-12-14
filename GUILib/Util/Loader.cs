using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUILib.Logger;

namespace GUILib.Util
{
    class Loader
    {
        private static readonly string absoluteDir = System.IO.Directory.GetParent(System.Environment.CurrentDirectory).Parent.FullName + @"\Resource\";

        public static string ReadFile(string relativePath)
        {
            ALogger.defaultLogger.Log("Loading file: " + relativePath, LogLevel.Info);

            return System.IO.File.ReadAllText(absoluteDir + relativePath);
        }

        public static string[] ReadFileLines(string relativePath)
        {
            ALogger.defaultLogger.Log("Loading file: " + relativePath, LogLevel.Info);

            return System.IO.File.ReadAllLines(absoluteDir + relativePath);
        }

        public static byte[] ReadAllBytes(string relativePath)
        {
            ALogger.defaultLogger.Log("Loading file: " + relativePath, LogLevel.Info);

            return System.IO.File.ReadAllBytes(absoluteDir + relativePath);
        }
    }
}
