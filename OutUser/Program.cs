using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace OutUser
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string processName = Process.GetCurrentProcess().ProcessName;
            if (Process.GetProcesses().Count<Process>((Process p) => p.ProcessName == processName) <= 1)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new UsersOut());
            }
            else
            {
                MessageBox.Show("Allready Opened, Plz See Below Taskbar");
            }
        }
    }
}
