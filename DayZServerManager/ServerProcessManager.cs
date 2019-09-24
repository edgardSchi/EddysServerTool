using System;
using DayZServerManager;

namespace DayZServerManager
{
    public class ServerProcessManager
    {

        public ServerProcessManager()
        {
        }

        bool FindServerProcess()
        {
            Process[] pname = Process.GetProcessesByName("DayZ");
            if (pname.Length == 0)
                return false;
            else
                return true;
        }
    }
}

