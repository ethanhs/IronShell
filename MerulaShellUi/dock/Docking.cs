using System.Windows;
using System.Windows.Interop;
using MerulaShell.windows;
using Window = System.Windows.Window;

namespace MerulaShellUi.dock
{
    static class Docking
    {
        public static void DockBottom(this Window window)
        {
            var screenWidth = SystemParameters.PrimaryScreenWidth;
            var screenHeight = SystemParameters.PrimaryScreenHeight;

            window.Left = 0;
            window.Width = screenWidth;
            window.Top = screenHeight - window.ActualHeight;
        }

        public static void DockTop(this Window window)
        {
            var screenWidth = SystemParameters.PrimaryScreenWidth;
            window.Top = 0;
            window.Left = 0;
            window.Width = screenWidth;
        }

        public static void DockScreen(this Window window)
        {
            var screenWidth = SystemParameters.PrimaryScreenWidth;
            var screenHeight = SystemParameters.PrimaryScreenHeight;
            window.Top = 0;
            window.Left = 0;
            window.Width = screenWidth;
            window.Height = screenHeight;
        }

        public static void SendToBottom(this Window window)
        {
            ShellTools.SetBottomWindow(new WindowInteropHelper(window).Handle);
        }
    }
}
