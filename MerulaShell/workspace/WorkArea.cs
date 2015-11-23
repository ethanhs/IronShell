using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MerulaShell.workspace
{
    /// <summary>
    /// This class can modify the windows workspace, so programs wont overlap your taskbar
    /// </summary>
    public class WorkArea
    {

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SystemParametersInfo(uint uiAction, uint uiParam,
            ref RECT pvParam, uint fWinIni);


        public enum SPI : int
        {
            SPI_SETWORKAREA = 0x002F,
            SPI_GETWORKAREA = 0x0030
        }

        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

//        private static RECT m_rcOldDesktopRect;
//        private static IntPtr m_hTaskBar;
        /// <summary>
        /// Sets the windows workspace
        /// </summary>
        /// <param name="offsetLeft"></param>
        /// <param name="offsetTop"></param>
        /// <param name="offsetRight"></param>
        /// <param name="offsetBottom"></param>
        public static void MakeNewDesktopArea(int offsetLeft, int offsetTop, int offsetRight, int offsetBottom)
        {
            // Make a new Workspace
            RECT rc;
            rc.left = SystemInformation.VirtualScreen.Left + offsetLeft;
            // We reserve the 24 pixels on top for our taskbar
            rc.top = SystemInformation.VirtualScreen.Top + offsetTop;
            rc.right = SystemInformation.VirtualScreen.Right - offsetRight;
            rc.bottom = SystemInformation.VirtualScreen.Bottom - offsetBottom;
            SystemParametersInfo((int)SPI.SPI_SETWORKAREA, 0, ref rc, 0);
        }

    }


    }


