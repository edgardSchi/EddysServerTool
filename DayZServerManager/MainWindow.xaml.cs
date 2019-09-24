using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using Microsoft.Win32;
using MahApps.Metro.Controls;

namespace DayZServerManager
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {


        private bool serverIsRunning = false;
        public bool autorestart = false;

        private GUIHandler guiHandler;

        public MainWindow()
        {
            guiHandler = new GUIHandler(this);
            guiHandler.LoadConfigs();
            Logger.CreateLogFolder();

            InitializeComponent();
            //TextOptions.SetTextFormattingMode(this.StatusLabel, TextFormattingMode.Display);
            //ServerProcessManager spm = new ServerProcessManager();

            guiHandler.LoadGUI();

            

            InitiateThread();
        }

        private void InitiateThread()
        {
            ServerProcessListener spl = new ServerProcessListener(this);
            Thread InstanceCaller = new Thread(new ThreadStart(spl.InstanceMethod));
            InstanceCaller.IsBackground = true;
            InstanceCaller.Start();
        }

        void OnStartBtnClick(object sender, RoutedEventArgs e)
        {
            //serverIsRunning = true;

            WriteToConsole("Starting server...");
            UpdateStatusLabel();
            ServerLauncher.StartServer();
        }

        private string CurrentTime()
        {
            return DateTime.Now.ToString("HH:mm:ss", System.Globalization.DateTimeFormatInfo.InvariantInfo);
        }

        public void WriteToConsole(string text)
        {
            this.ConsoleTextBlock.Text += Environment.NewLine;
            this.ConsoleTextBlock.Text += CurrentTime() + " ";
            this.ConsoleTextBlock.Text += text;
            this.ConsoleScrollViewer.ScrollToEnd();
        }

        public void AddItemToList(ListBox list, string name)
        {
            ListBoxItem item = new ListBoxItem();
            item.Foreground = new SolidColorBrush(Colors.White);
            item.FontFamily = new FontFamily("Global User Interface");
            item.FontSize = 16;
            item.Content = name;

            list.Items.Add(item);
        }

        private void UpdateStatusLabel()
        {
            if(serverIsRunning)
            {
                this.StatusLabel.Content = "ONLINE";
                this.StatusLabel.Foreground = new SolidColorBrush(Colors.Green);
            } else
            {
                this.StatusLabel.Content = "OFFLINE";
                this.StatusLabel.Foreground = new SolidColorBrush(Colors.Red);
            }
        }

        public void SetServerIsRunning(bool running)
        {
            serverIsRunning = running;
            UpdateStatusLabel();
        }

        void OnRestartBtnClick(object sender, RoutedEventArgs e)
        {
            ServerProcessListener.KillProcess();
            Logger.WriteToLogFile("Restarted server with server tool");
            ServerLauncher.StartServer();
        }

        void OnStopBtnClick(object sender, RoutedEventArgs e)
        {
            WriteToConsole("Killing server...");
            if (ServerProcessListener.KillProcess())
            {
                Logger.WriteToLogFile("Server killed with server tool");
            }         
            
        }

        void OnCloseProcessBtnClick(object sender, RoutedEventArgs e)
        {
            WriteToConsole("Shutting down server...");
            if (ServerProcessListener.ExitProcess())
            {
                Logger.WriteToLogFile("Server was shut down with server tool");
            }
        }

        void OnAutorestartSwitch(object sender, EventArgs e)
        {
            ConfigHolder.autorestart = (bool) AutorestartSwitch.IsChecked;
        }

        void OnAddParameter(object sender, RoutedEventArgs e)
        {
            ParameterWindow dialog = new ParameterWindow("Add a parameter", "Parameter:", "");
            if (dialog.ShowDialog() == true)
            {
                if(dialog.textbox.Text != "")
                {
                    if(dialog.textbox.Text[0] == char.Parse("-"))
                    {
                        AddItemToList(this.ParameterList, dialog.textbox.Text);
                        guiHandler.AddParameter(dialog.textbox.Text);
                    } else if(dialog.textbox.Text[0] == char.Parse("\"")) 
                    {
                        AddItemToList(this.ParameterList, dialog.textbox.Text);
                        guiHandler.AddParameter(dialog.textbox.Text);
                    } else
                    {
                        AddItemToList(this.ParameterList, "-" + dialog.textbox.Text);
                        guiHandler.AddParameter("-" + dialog.textbox.Text);
                    }

                }             
            }
        }

        void OnRemoveParameter(object sender, RoutedEventArgs e)
        {
            int index = this.ParameterList.SelectedIndex;
            string parameter = ((ListBoxItem)ParameterList.SelectedValue).Content.ToString();
            guiHandler.RemoveParameter(parameter);
            this.ParameterList.Items.RemoveAt(index);         
        }

        void OnEditParameter(object sender, RoutedEventArgs e)
        {
            int index = this.ParameterList.SelectedIndex;
            string parameter = ((ListBoxItem)ParameterList.SelectedValue).Content.ToString();

            ParameterWindow dialog = new ParameterWindow("Edit parameter", "Parameter:", parameter);
            if (dialog.ShowDialog() == true)
            {
                if (dialog.textbox.Text != "")
                {
                    guiHandler.RemoveParameter(parameter);
                    this.ParameterList.Items.RemoveAt(index);
                    AddItemToList(this.ParameterList, dialog.textbox.Text);
                    guiHandler.AddParameter(dialog.textbox.Text);
                }
            }
        }

        public void OnAddMod(object sender, RoutedEventArgs e)
        {
            string path = OpenFolderDialog();
            guiHandler.AddMod(path);
            AddItemToList(this.ModList, ConfigHolder.ShortenModPath(path));
        }

        public void OnRemoveMod(object sender, RoutedEventArgs e)
        {
            int index = this.ModList.SelectedIndex;
            string mod = ((ListBoxItem)ModList.SelectedValue).Content.ToString();
            guiHandler.RemoveMod(mod);
            this.ModList.Items.RemoveAt(index);
        }

        void OnRemoveAllMods(object sender, RoutedEventArgs e)
        {
            ModList.Items.Clear();

            guiHandler.RemoveAllMods();
        }

        public void OnLoadModsFromDirectoy(object sender, RoutedEventArgs e)
        {
            guiHandler.LoadAllModsInDirectory();
        }

        void OnSetProcessBtn(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Executables (*.exe)|*.exe";
            if(dialog.ShowDialog() == true)
            {
                this.ProcessTextBox.Text = dialog.FileName;
                guiHandler.ChangeProcessPath(dialog.FileName);
            }
        }

        void OnProcessPathChanged(object sender, RoutedEventArgs e)
        {
            guiHandler.ChangeProcessPath(ProcessTextBox.Text);
        }

        private string OpenFolderDialog()
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();

            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            if(result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.SelectedPath))
            {
                return dialog.SelectedPath;
            } else
            {
                return "";
            }
        }

        void OnGitHubClick(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/edgardSchi/EddysServerTool");
        }

        void OnKillUnrespProcessBox(object sender, RoutedEventArgs e)
        {
            guiHandler.ChangeKillNonrespProcess( (bool) KillNonRespProcessBox.IsChecked);
        }
    }
}
