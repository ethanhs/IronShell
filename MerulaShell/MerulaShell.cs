using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MerulaShell.windows;

namespace MerulaShell
{
    public class MerulaShell
    {
        private static MerulaShell shell;
        private readonly WindowCenter windowCenter;

        public event EventHandler WindowListChanged;

        public void InvokeWindowListChanged(EventArgs e)
        {
            EventHandler handler = WindowListChanged;
            if (handler != null) handler(this, e);
        }

        private MerulaShell()
        {
            windowCenter = new WindowCenter();
            windowCenter.WindowListChanged += WindowCenterWindowListChanged;
            ShellReady.SetShellReadyEvent();
        }

        private void WindowCenterWindowListChanged(object sender, EventArgs e)
        {
            InvokeWindowListChanged(new EventArgs());
        }

        public static MerulaShell GetInstance()
        {
            return shell ?? (shell = new MerulaShell());
        }

        /// <summary>
        /// Gets a list of active windows 
        /// </summary>
        /// <returns>A list of window objects</returns>
        public List<Window> GetActiveWindows()
        {
            return windowCenter.Windows;
        }

        public void AddException(IntPtr handle)
        {
            windowCenter.AddException(handle);
        }

        /// <summary>
        /// Hide a window 
        /// </summary>
        /// <param name="name">name of the window</param>
        public static void HideWindow(string name)
        {
            var window = ShellCommands.FindWindow(name, null);
            if ((int)window != 0)
            {
                ShellCommands.ShowWindow(window, (int)ShellCommands.ShowStyle.Hide);
            }
        }
    }
}
