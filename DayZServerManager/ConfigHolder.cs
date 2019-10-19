using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;

namespace DayZServerManager
{
    class ConfigHolder
    {

        private static ArrayList parameters = new ArrayList();
        private static ArrayList mods = new ArrayList();
        private static ArrayList restarts = new ArrayList();
        public static bool autorestart = false;
        public static bool killNonRespProcess = false;
        public static bool startServerOnStartup = false;

        private static string processPath = @"";

        public static void SetProcessPath(string path)
        {
            processPath = path;
        }

        public static string GetProcessPath()
        {
            return processPath;
        }

        public static string GetProcessName()
        {
            string name = Path.GetFileName(ConfigHolder.GetProcessPath());
            string newName = "";

            for(int i = 0; i < name.Length; i++)
            {
                

                if(name[i] == char.Parse("."))
                {
                    break;
                } else
                {
                    newName += name[i];
                }
            }

            return newName;
        }

        public static void AddParameter(string parameter)
        {
            parameters.Add(parameter);
        }

        public static void RemoveParameter(string parameter)
        {
            parameters.Remove(parameter);
        }

        public static ArrayList GetParameters()
        {
            return parameters;
        }

        public static void SetParameters(ArrayList parametersList)
        {
            parameters = parametersList;
        }

        public static void AddMod(string path)
        {
            mods.Add(path);
        }

        public static void RemoveMod(string path)
        {
            mods.Remove(path);
        }

        public static ArrayList GetMods()
        {
            return mods;
        }

        public static void SetMods(ArrayList modsList)
        {
            mods = modsList;
        }

        public static void AddRestart(string restart)
        {
            restarts.Add(restart);
        }

        public static void RemoveRestart(string restart)
        {
            restarts.Remove(restart);
        }

        public static ArrayList GetRestarts()
        {
            return restarts;
        }

        public static void SetRestarts(ArrayList restartsList)
        {
            restarts = restartsList;
        }

        public static string ShortenModPath(string path)
        {

            string shortPath = "";
            bool atFound = false;
            foreach(char c in path)
            {
                if(c == char.Parse("@") || atFound)
                {
                    shortPath += c;
                    atFound = true;
                }
            }

            if(atFound)
            {
                return shortPath;
            } else
            {
                return path;
            }
        }

        public static bool IsModFolder(string shortendPath)
        {
            return (shortendPath[0] == char.Parse("@"));
        }
    }
}
