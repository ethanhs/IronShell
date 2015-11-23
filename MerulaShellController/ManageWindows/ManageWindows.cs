using System;
using System.Collections.Generic;
using MerulaShell.windows;

namespace MerulaShellController.ManageWindows
{
    public class ManageWindows
    {
        private MerulaShell.MerulaShell shell;

        public ManageWindows()
        {
            shell = MerulaShell.MerulaShell.GetInstance();
            shell.WindowListChanged += ShellWindowListChanged;
        }
        /// <summary>
        /// This event will be triggerd when the number of open windows has changed
        /// </summary>
        public event EventHandler WindowListChanged;

        public void InvokeWindowListChanged(EventArgs e)
        {
            EventHandler handler = WindowListChanged;
            if (handler != null) handler(this, e);
        }

        private void ShellWindowListChanged(object sender, EventArgs e)
        {
            InvokeWindowListChanged(new EventArgs());
        }
        public void MinimizeWindow()
        {
        }

      

        /// <summary>
        /// Gets a list of active windows 
        /// </summary>
        /// <returns>A list of window objects</returns>
        public List<Window> GetWindows()
        {
            var controller = new GetWindows();
            return controller.GetActiveWindows();
        }

        public void ShowWindow()
        {
        }

        public void CloseWindow()
        {
            
        }

        public void AddException(IntPtr handle)
        {
            shell.AddException(handle);
        }
    }
}
