using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayZServerManager
{
    class GUIHandler
    {

        private MainWindow window;

        public GUIHandler(MainWindow window)
        {
            this.window = window;
        }

        public void WriteToConsole(string text)
        {
            window.WriteToConsole(text);
        }

        public void LoadGUI()
        {
            window.ProcessTextBox.Text = ConfigHolder.GetProcessPath();
            window.KillNonRespProcessBox.IsChecked = ConfigHolder.killNonRespProcess;
            window.StartServerWithToolBox.IsChecked = ConfigHolder.startServerOnStartup;

            foreach (string s in ConfigHolder.GetParameters())
            {
                window.AddItemToList(window.ParameterList, s);
            }
            foreach (string s in ConfigHolder.GetMods())
            {
                window.AddItemToList(window.ModList, ConfigHolder.ShortenModPath(s));
            }
            WriteToConsole("Config files loaded");
        }

        public void LoadConfigs()
        {
            if(ConfigIO.CheckForSettingsConfig())
            {
                //WriteToConsole("Config for settings found");
                ConfigHolder.SetProcessPath((string) Properties.Settings.Default["processPath"]);
                ConfigHolder.killNonRespProcess = (bool) Properties.Settings.Default["killNonRespProcess"];
                ConfigHolder.startServerOnStartup = (bool)Properties.Settings.Default["startServerOnStartup"];
            }

            if(ConfigIO.CheckForParametersConfig())
            {
                //WriteToConsole("Config for parameters found");
                ConfigHolder.SetParameters(ConfigIO.LoadParameters());

            } else
            {
                //WriteToConsole("Config for parameters not found. Creating a new config...");
            }

            if(ConfigIO.CheckForModsConfig())
            {
                //WriteToConsole("Config for mods found");
                ConfigHolder.SetMods(ConfigIO.LoadMods());

            } else
            {
                //WriteToConsole("Config for mods not found. Creating a new config...");
            }
        }

        public void RemoveParameter(string parameter)
        {

            for (int i = 0; i < ConfigHolder.GetParameters().Count; i++)
            {
                if (ConfigHolder.GetParameters()[i].ToString() == parameter)
                {
                    ConfigHolder.GetParameters().RemoveAt(i);
                }
            }

            ConfigIO.WriteParameters(ConfigHolder.GetParameters());
        }

        public void AddParameter(string parameter)
        {
            ConfigHolder.AddParameter(parameter);
            ConfigIO.WriteParameters(ConfigHolder.GetParameters());
        }

        public void ChangeProcessPath(string path)
        {
            Properties.Settings.Default["processPath"] = path;
            Properties.Settings.Default.Save();
            ConfigHolder.SetProcessPath(path);
            //ConfigIO.WriteSettings(path);
        }

        public void ChangeKillNonrespProcess(bool kill)
        {
            Properties.Settings.Default["killNonRespProcess"] = kill;
            Properties.Settings.Default.Save();
            ConfigHolder.killNonRespProcess = kill;
        }

        public void ChangeServerStartOnStartup(bool start)
        {
            Properties.Settings.Default["startServerOnStartup"] = start;
            Properties.Settings.Default.Save();
            ConfigHolder.startServerOnStartup = start;
        }

        public bool AddMod(string mod)
        {

            foreach(string m in ConfigHolder.GetMods())
            {
                if(m.Equals(mod))
                {
                    WriteToConsole("The mod " + mod + " is already loaded!");
                    return false;
                }
            }

            string keysPath = "";

            if(Directory.Exists(mod + @"\keys") || Directory.Exists(mod + @"\key"))
            {
                if(Directory.Exists(mod + @"\keys"))
                {
                    keysPath = mod + @"\keys";
                } else if(Directory.Exists(mod + @"\key"))
                {
                    keysPath = mod + @"\key";
                }
                if (Directory.Exists(keysPath))
                {
                    foreach (string s in Directory.GetFiles(keysPath))
                    {
                        if (s.EndsWith(".bikey"))
                        {
                            if (!File.Exists(GetProcessDirectory() + @"\keys\" + Path.GetFileName(s)))
                            {
                                File.Copy(s, GetProcessDirectory() + @"\keys\" + Path.GetFileName(s));
                            }

                        }
                    }
                }
            }

            ConfigHolder.AddMod(mod);
            ConfigIO.WriteMods(ConfigHolder.GetMods());
            return true;
        }

        public void RemoveMod(string mod)
        {
            string modPath = "";
            if(mod[0] == char.Parse("@"))
            {
                modPath = GetProcessDirectory() + mod;
                WriteToConsole("Deleting mod: " + modPath);
            } else
            {
                modPath = mod;
            } 

            for (int i = 0; i < ConfigHolder.GetMods().Count; i++)
            {
                if (ConfigHolder.GetMods()[i].ToString() == modPath)
                {
                    ConfigHolder.GetMods().RemoveAt(i);
                }
            }

            ConfigIO.WriteMods(ConfigHolder.GetMods());
        }

        public void RemoveAllMods()
        {
            ConfigHolder.SetMods(new System.Collections.ArrayList());
            ConfigIO.WriteMods(ConfigHolder.GetMods());
        }

        private string GetProcessDirectory()
        {
            return Path.GetDirectoryName(ConfigHolder.GetProcessPath()) + @"\";
        }

        public void PrintProcessDirectory()
        {
            WriteToConsole(Path.GetFullPath(ConfigHolder.GetProcessPath()));
        }

        public void LoadAllModsInDirectory()
        {
            var directories = Directory.GetDirectories(Path.GetDirectoryName(ConfigHolder.GetProcessPath()));

            foreach(string s in directories)
            {
                if(ConfigHolder.IsModFolder(ConfigHolder.ShortenModPath(s)))
                {
                    if(AddMod(s))
                    {
                        window.AddItemToList(window.ModList, ConfigHolder.ShortenModPath(s));
                    }                  
                }
            }
        }

        public void StartServer()
        {
            window.WriteToConsole("Starting server...");
            window.UpdateStatusLabel();
            ServerLauncher.StartServer();
        }

        //Starts the server if the option "Start Server On Startup" and and sets autostart to true
        public void CheckOnStartup()
        {
            if(ConfigHolder.startServerOnStartup)
            {
                ConfigHolder.autorestart = true;
                window.AutorestartSwitch.IsChecked = true;
                StartServer();
            }
        }

    }
}
