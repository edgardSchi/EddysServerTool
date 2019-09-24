using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayZServerManager
{
    class Logger
    {

        private static string logPath = ConfigIO.CurrentDirectory() + @"\logs\";

        public static void CreateLogFolder()
        {
            if (!Directory.Exists(logPath))
            {
                Directory.CreateDirectory(logPath);
            }
        }

        public static void WriteToLogFile(string text)
        {
            if(File.Exists(GetLogFile()))
            {
                using (StreamWriter sw = File.AppendText(GetLogFile()))
                {
                    sw.WriteLine(GetCurrentTime() + " " + text);
                }
            } else
            {
                using (StreamWriter sw = new StreamWriter(GetLogFile()))
                {
                    sw.WriteLine(GetCurrentTime() + " " + text);
                }
            }

        }

        private static string GetLogFile()
        {
            return logPath + GetDate() + ".log";
        }

        private static string GetDate()
        {
            return DateTime.Now.ToString("dd_M_yyyy");
        }

        private static string GetCurrentTime()
        {
            return DateTime.Now.ToString("HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo);
        }

    }
}
