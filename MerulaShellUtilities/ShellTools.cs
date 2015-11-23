using System;
using System.Runtime.InteropServices;

namespace MerulaShell.windows
{
    /// <summary>
    /// Tools for using the shell 
    /// </summary>
    public class ShellTools
    {
        [DllImport("user32.dll")]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X,
           int Y, int cx, int cy, uint uFlags);

        const UInt32 SWP_NOSIZE = 0x0001;
        const UInt32 SWP_NOMOVE = 0x0002;
        const UInt32 SWP_NOACTIVATE = 0x0010;

        static readonly IntPtr HWND_BOTTOM = new IntPtr(1);

        /// <summary>
        /// Sends a window to the botttem of the Z order
        /// </summary>
        /// <param name="hWnd">Windows handler</param>
        public static void SetBottomWindow(IntPtr hWnd)
        {
            SetWindowPos(hWnd, HWND_BOTTOM, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_NOACTIVATE);
        }
    }
}
