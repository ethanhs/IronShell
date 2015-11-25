using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using Explore10;
using Microsoft.Win32;

namespace MerulaShellUi
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {

            MessageBox.Show("Welcome to ShellSharp.\n\n" +
                                "This is a demo and the windows shell is temporary replaced with the MerulaShell. If you want the old Windows back, please go to start -> Explorer.\n\n" +
                                "Have fun and good luck with ShellSharp.");
            //KillExplorer();

            var thisProc = Process.GetCurrentProcess();
            if (Process.GetProcessesByName(thisProc.ProcessName).Length > 1)
            {
                var win = new Explore10.MainWindow();
                win.Show();
            }
        }

        private void KillExplorer()
        {
            var explore = Process.GetProcessesByName("explorer");
            if (explore.Length > 0)
            {
                var key = Registry.LocalMachine;
                key=key.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon", true);
                key.SetValue("AutoRestartShell", 0);
                foreach (var proc in explore)
                {
                    proc.Kill();
                    proc.WaitForExit();
                }
                key.SetValue("AutoRestartShell", 0);
            }
        }
    }
}
