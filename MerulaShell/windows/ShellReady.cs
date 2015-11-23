using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace MerulaShell.windows
{
    class ShellReady
    {
        private const uint EVENT_MODIFY_STATE = 0x2;

        public enum EventAccessRights
        {
            EVENT_ALL_ACCESS = 0x1f0003,
            EVENT_MODIFY_STATE = 0x2
        }

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenEvent(UInt32 dwDesiredAcess, bool bInheritHandle, string lpName);

        [DllImport("kernel32.dll")]
        public static extern bool SetEvent(IntPtr hEevent);

        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(IntPtr hObject);

        public static bool SetShellReadyEvent()
        {
            IntPtr hEvent = default(IntPtr);

            // open an event
            hEvent = OpenEvent(EVENT_MODIFY_STATE, false, "ShellDesktopSwitchEvent");

            // return if event is null
            if ((hEvent == IntPtr.Zero))
            {
                return false;
            }

            // set the event using the handle
            SetEvent(hEvent);

            // close the handle
            CloseHandle(hEvent);

            return true;
        }
    }
}
