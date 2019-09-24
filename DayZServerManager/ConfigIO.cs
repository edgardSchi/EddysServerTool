using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;
using System.Reflection;

namespace DayZServerManager
{
    class ConfigIO
    {

        public const string PARAMETER_PATH = "/config/parameters.cfg";
        public const string MODS_PATH = "/config/mods.cfg";
        public const string SETTINGS_PATH = "/config/settings.cfg";

        public static ArrayList LoadMods()
        {
            if (File.Exists(CurrentDirectory() + MODS_PATH))
            {
                ArrayList mods = new ArrayList();
                var lines = File.ReadLines(CurrentDirectory() + MODS_PATH);
                foreach (var line in lines)
                {
                    if(line != "")
                    {
                        mods.Add(line);
                    }
                    
                }
                return mods;
            }

            return null;

        }

        public static bool WriteMods(ArrayList mods)
        {
            if (File.Exists(CurrentDirectory() + MODS_PATH))
            {
                File.WriteAllText(CurrentDirectory() + MODS_PATH, ConvertListToString(mods));
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool WriteParameters(ArrayList parameters)
        {
            if(File.Exists(CurrentDirectory() + PARAMETER_PATH))
            {
                File.WriteAllText(CurrentDirectory() + PARAMETER_PATH, ConvertListToString(parameters));
                return true;
            } else
            {
                return false;
            }
        }

        public static ArrayList LoadParameters()
        {

            if(File.Exists(CurrentDirectory() + PARAMETER_PATH))
            {
                ArrayList parameters = new ArrayList();
                var lines = File.ReadLines(CurrentDirectory() + PARAMETER_PATH);
                foreach (var line in lines)
                {
                    parameters.Add(line);
                }
                return parameters;
            }

            return null;

        }

        public static bool WriteSettings(ArrayList settings)
        {
            if (File.Exists(CurrentDirectory() + SETTINGS_PATH))
            {
                File.WriteAllText(CurrentDirectory() + SETTINGS_PATH, ConvertListToString(settings));
                return true;
            }
            else
            {
                return false;
            }
        }

        public static ArrayList LoadSettings()
        {
            if (File.Exists(CurrentDirectory() + SETTINGS_PATH))
            {
                ArrayList settings = new ArrayList();
                var lines = File.ReadLines(CurrentDirectory() + SETTINGS_PATH);
                foreach (var line in lines)
                {
                    settings.Add(line);
                }
                return settings;
            }

            return null;
        }

        public static string ConvertListToString(ArrayList list)
        {
            string text = "";

            for(int i = 0; i < list.Count; i++) {
                text += list[i];
                if(i < list.Count)
                {
                    text += Environment.NewLine;
                }
            }

            return text;
        }

        public static string CurrentDirectory()
        {
            return System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        }

        private static bool CheckForConfig(string path)
        {
            if(File.Exists(CurrentDirectory() + path))
            {
                return true;
            } else
            {
                System.IO.Directory.CreateDirectory(CurrentDirectory() + "/config/");
                File.Create(CurrentDirectory() + path);
                return false;
            }
        }

        public static bool CheckForParametersConfig()
        {
            return CheckForConfig(PARAMETER_PATH);
        }

        public static bool CheckForModsConfig()
        {
            return CheckForConfig(MODS_PATH);
        }

        public static bool CheckForSettingsConfig()
        {
            return CheckForConfig(SETTINGS_PATH);
        }

    }
}
