using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;

namespace MerulaShell.windows
{
    class WindowCenter
    {
        internal List<Window> Windows { get; set; }

        private int windowCount;
        private readonly List<IntPtr> exceptionList;

        private bool stopped;

        public event EventHandler WindowListChanged;


        private void InvokeWindowListChanged(EventArgs e)
        {
            EventHandler handler = WindowListChanged;
            if (handler != null) handler(this, e);
        }

        public WindowCenter()
        {
            exceptionList = new List<IntPtr>();

            LoadWindows();


            WindowListChanged += WindowCenterWindowListChanged;

            //This thread checks if there are changes in the window list 
            var checkWindowThread = new Thread(CheckWindows) {Priority = ThreadPriority.Lowest};
            checkWindowThread.Start();
        }

        void WindowCenterWindowListChanged(object sender, EventArgs e)
        {
            LoadWindows();
        }

        private void LoadWindows()
        {
            DestroyWindows();
            Windows = new List<Window>();
            EnumWindows(Callback, 0);
        }

        private void DestroyWindows()
        {
            if (Windows == null) return;
            foreach (var window in Windows)
            {
                window.Destroy();
            }
        }

        /// <summary>
        /// When the list of windows are changed this method will
        /// fire a event that the windows are changed 
        /// </summary>
        private void CheckWindows()
        {
            while (!stopped)
            {
                windowCount = 0;
                EnumWindows(CountCallback, 0);

                if(windowCount != Windows.Count)
                    InvokeWindowListChanged(new EventArgs());
                Thread.Sleep(40);
            }
        }

        public void AddException(IntPtr handle)
        {
            exceptionList.Add(handle);
        }

        ~WindowCenter()
        {
            stopped = true;
        }

        #region APIHANDLERS
        private static readonly int GWL_STYLE = -16;
        private static readonly ulong WS_VISIBLE = 0x10000000L;
        private static readonly ulong WS_BORDER = 0x00800000L;
        private static readonly ulong TARGETWINDOW = WS_BORDER | WS_VISIBLE;
//
        private static readonly int WS_EX_TOOLWINDOW = 0x80;
        private static readonly int WS_EX_APPWINDOW = 0x40000;
        private static readonly int GW_OWNER = 4;
        private static readonly int GWL_EXSTYLE = -20;

        private bool Callback(IntPtr hwnd, int lParam)
        {
            if (IsTaskBarWindow(hwnd))
            {
                var w = new Window(hwnd);
                Windows.Add(w);
            }
            return true; //continue enumeration
        }

        private bool IsTaskBarWindow(IntPtr hwnd)
        {
            if (!exceptionList.Contains(hwnd) &&
                (GetWindowLongA(hwnd, GWL_STYLE) & TARGETWINDOW) == TARGETWINDOW &&
                IsWindowVisible(hwnd) &&
                GetParent(hwnd) == IntPtr.Zero)
            {
                bool bNoOwner = (GetWindow(hwnd, GW_OWNER) == IntPtr.Zero);
                int lExStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
                if (!bNoOwner) return false;
                if ((((lExStyle & WS_EX_TOOLWINDOW) == 0)))
                {
                    return true;
                }
            }
            return false;
        }

        private bool CountCallback(IntPtr hwnd, int lParam)
        {
            if (IsTaskBarWindow(hwnd))
            {
                windowCount++;
            }
            return true; //continue enumeration
        }

        [DllImport("user32.dll")]
        static extern int EnumWindows(EnumWindowsCallback lpEnumFunc, int lParam);

        [DllImport("user32.dll")]
        static extern ulong GetWindowLongA(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern IntPtr GetParent(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindow(IntPtr hWnd, int wFlag);

        [DllImport("user32.dll")]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        
        

        delegate bool EnumWindowsCallback(IntPtr hwnd, int lParam);
        #endregion

        
    }
}
