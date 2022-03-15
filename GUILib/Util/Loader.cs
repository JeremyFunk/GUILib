using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using GUILib.Logger;
using System.IO;

namespace GUILib.Util
{
    public class Loader
    {
        private static Assembly assembly = Assembly.GetExecutingAssembly();

        public static string ReadFile(string relativePath)
        {
            ALogger.defaultLogger.Log("Loading file: " + relativePath, LogLevel.Info);

            string path = "GUILib.Resource." + relativePath.Replace(@"\", ".").Replace("/", ".");
            string result = "";
            //return System.IO.File.ReadAllText(absoluteDir + relativePath);
            using (Stream stream = assembly.GetManifestResourceStream(path))
            using (StreamReader reader = new StreamReader(stream))
            {
                result = reader.ReadToEnd();
            }

            return result;
        }

        public static string[] ReadFileLines(string relativePath)
        {
            ALogger.defaultLogger.Log("Loading file: " + relativePath, LogLevel.Info);

            string path = "GUILib.Resource." + relativePath.Replace(@"\", ".").Replace("/", ".");
            List<string> result = new List<string>();
            //return System.IO.File.ReadAllText(absoluteDir + relativePath);
            using (Stream stream = assembly.GetManifestResourceStream(path))
            using (StreamReader reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    result.Add(reader.ReadLine());
                }
            }

            return result.ToArray();
        }
    }
}
