using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace DayZServerManager
{
    class ServerLauncher
    {

        public static string error = "";
        public static string output = "";

        public static string StartServer()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = ConfigHolder.GetProcessPath();
            startInfo.Arguments = ReadParameters() + " " + ReadMods();

            Process process = new Process();
            process.StartInfo = startInfo;         
            process.Start();

            if(!process.Responding)
            {
                process.Kill();
            }

            Logger.WriteToLogFile("Starting server with parameters: " + ReadParameters() + " " + ReadMods());

            return ReadParameters() + " " + ReadMods();
        }

        private static string ReadParameters()
        {
            string parameters = "";

            foreach(string s in ConfigHolder.GetParameters())
            {
                parameters += s + " ";
            }

            return parameters;
        }

        private static string ReadMods()
        {
            string parameter = "\"-mod=";

            for(int i = 0; i < ConfigHolder.GetMods().Count; i++) 
            {
                string mod = ConfigHolder.GetMods()[i].ToString();
                if(i != ConfigHolder.GetMods().Count - 1)
                {
                    parameter += ConfigHolder.ShortenModPath(mod) + ";";
                } else
                {
                    parameter += ConfigHolder.ShortenModPath(mod);
                }
            }
            return parameter += "\"";
        }

    }
}
