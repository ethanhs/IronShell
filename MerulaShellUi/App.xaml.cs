using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace MerulaShellUi
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            if(e.Args.Any(a=>a.Replace("/",string.Empty).Replace("-",string.Empty).ToLower() == "demo"))
            {
                MessageBox.Show("Welkom to MerulaShell.\n\n" +
                                "This is a demo and the windows shell is temporary replaced with the MerulaShell. If you want the old Windows back all what you will have to do is restart your computer.\n\n" +
                                "Have fun and goodluck with MerulaShell.");
                KillExplorer();
            }
        }

        private static void KillExplorer()
        {
            MerulaShell.MerulaShell.HideWindow("Shell_TrayWnd");
            MerulaShell.MerulaShell.HideWindow("Button");
        }
    }
}
