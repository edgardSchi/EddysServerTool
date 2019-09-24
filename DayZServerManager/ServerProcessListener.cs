using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Windows;
using System.Timers;

namespace DayZServerManager
{
    class ServerProcessListener
    {

        private MainWindow window;
        private static System.Timers.Timer timer = new System.Timers.Timer();
        private static bool timerRunning = false;


        public ServerProcessListener(MainWindow window)
        {
            this.window = window;        
        }

        public void InstanceMethod()
        {
            WriteToConsole("Serverlistener thread is starting...");

            ListenToProcess();            
        }

        private void WriteToConsole(string text)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() => { window.WriteToConsole(text); }));
        }

        private void ListenToProcess()
        {
            WriteToConsole("Serverlistener thread is running...");
            while (true)
            {
                if (IsServerRunning())
                {
                    //WriteToConsole("Server is running...");
                }
                else
                {
                    if(ConfigHolder.autorestart)
                    {
                        WriteToConsole("Process not found, restarting server...");
                        Logger.WriteToLogFile("Process not running, trying to restart server...");
                        ServerLauncher.StartServer();
                    }                  
                }

                Application.Current.Dispatcher.BeginInvoke(new Action(() => {  window.SetServerIsRunning(IsServerRunning()); }));
                Thread.Sleep(7000);
            }
        }

        private bool IsServerRunning()
        {

            Process[] pname = Process.GetProcessesByName(ConfigHolder.GetProcessName());
            
            if(pname.Length != 0 && !pname[0].Responding && timer == null && ConfigHolder.killNonRespProcess)
            {
                timer = new System.Timers.Timer();
                timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
                timer.Interval = 30000;
                timer.AutoReset = false;
                timer.Start();
            }

            if (pname.Length == 0)
            {
               return false;

            } 
            else
            {
                return true;
            }
        }
        
        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            KillProcess();
            timer = null;
        }

        public static bool KillProcess()
        {
            Process[] pname = Process.GetProcessesByName(ConfigHolder.GetProcessName());

            foreach(Process p in pname)
            {
                p.Kill();
            }

            return pname.Length != 0;
        }

        public static bool ExitProcess()
        {
            Process[] pname = Process.GetProcessesByName(ConfigHolder.GetProcessName());

            foreach (Process p in pname)
            {
                p.CloseMainWindow();
            }

            return pname.Length != 0;
        }


    }
}
