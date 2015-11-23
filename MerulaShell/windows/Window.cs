using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace MerulaShell.windows
{
    /// <summary>
    /// Window object
    /// </summary>
    public class Window
    {
        private bool stopped;
        public IntPtr Handler { get; private set; }
        /// <summary>
        /// Create a new window
        /// </summary>
        /// <param name="handler">window handler(unieke key)</param>
        public Window(IntPtr handler)
        {
            

            this.Handler = handler;
            Title=GetTitle();
            SetIcon();
            Thumbnail = new Thumbnail(handler);

            var tilteThread = new Thread(CheckTitle);
            tilteThread.Start();
        }
        /// <summary>
        /// A title of a window often changes 
        /// this event will trigger when that happens
        /// </summary>
        public event EventHandler TitleChanged;

        public void InvokeTitleChanged(EventArgs e)
        {
            EventHandler handler = TitleChanged;
            if (handler != null) handler(this, e);
        }

        private bool isMinimized;
        /// <summary>
        /// Checks if the title is changed of a window
        /// </summary>
        private void CheckTitle()
        {
            isMinimized = IsIconic(Handler);
            while (!stopped)
            {
                if(!Title.Equals(GetTitle()))
                {
                    Title = GetTitle();
                    InvokeTitleChanged(new EventArgs());
                }
                if(IsIconic(Handler)!=isMinimized)
                {
                    if(IsIconic(Handler))
                    {
                        HideWindow();
                    }
                    isMinimized = IsIconic(Handler);
                }

                Thread.Sleep(40);
            }
        }

        private void SetIcon()
        {
            var processId = 0;
            GetWindowThreadProcessId(Handler, out processId);

            var p = Process.GetProcessById(processId);
            
            var ic = Icon.ExtractAssociatedIcon(p.MainModule.FileName);

            ProgramIcon  =  Imaging.CreateBitmapSourceFromHBitmap((ic.ToBitmap()).GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            ProgramIcon.Freeze();
            ic.Dispose();
        }
        /// <summary>
        /// Gets the title of the window
        /// and sets the title property
        /// </summary>
        private string GetTitle()
        {
            var sb = new StringBuilder(200);
            GetWindowText(Handler, sb, sb.Capacity);
            return sb.ToString();
        }

        /// <summary>
        /// Unloads all the resources used by this object
        /// </summary>
        public  void Destroy()
        {

            stopped = true;
            ProgramIcon = null;
            Thumbnail.Unregister();
            Thumbnail = null;
            Title = null;
        }
        /// <summary>
        /// Get the live tumbnail of a window
        /// (doesn't work on windows xp)
        /// </summary>
        public Thumbnail Thumbnail { get; private set; }
        /// <summary>
        /// Title of the window
        /// </summary>
        public string Title { get; private set; }
        /// <summary>
        /// Icon of the program
        /// </summary>
        public BitmapSource ProgramIcon { get; private set; }

        /// <summary>
        /// This funtion does the default taskbar action of a window
        /// Like minimize / maximize
        /// </summary>
        public void MaximizeMinimize()
        {
            if (!IsIconic(Handler))
            { 
                CloseWindow(Handler);
            }
            else
            {
                ShowWindow(Handler, 4);
                SetForegroundWindow(Handler);
            }
        }
        /// <summary>
        /// Hide the window
        /// </summary>
        public void HideWindow()
        {
           MoveWindow(Handler, 0, -50, 0, 0, false);
        }
        /// <summary>
        /// Sets the window to the foreground
        /// </summary>
        public void SetToForeground()
        {
            SetForegroundWindow(Handler);
            if(IsIconic(Handler))
            {
                MaximizeMinimize();
            }
        }
        /// <summary>
        /// Sets the window at no top most position
        /// -Topmost
        /// -Normal windows
        /// </summary>
        public void SetTopMost()
        {
            SetWindowPos(Handler, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOSIZE);
        }

        /// <summary>
        /// Closes the window
        /// </summary>
        public void Close()
        {
            ShowWindow(Handler, 0);
            //EndTask(Handler, false,false);
        }


        [DllImport("user32.dll")]
        private static extern void GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        private static extern bool IsIconic(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        private static extern bool CloseWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("user32.dll")]
        private static extern int GetWindowThreadProcessId(IntPtr hWnd, out int processId);

        [DllImport("user32.dll")]
        private static extern bool EndTask(IntPtr hWnd, bool fShutDown, bool fForce);

        private IntPtr HWND_TOPMOST = (IntPtr)(-1);
        private int SWP_NOSIZE = 0x1;

        [DllImport("user32.dll")]
        public static extern int SetWindowPos(IntPtr hwnd, IntPtr
        hWndInsertAfter, int x, int y, int cx, int cy, int wFlags);


    }
}
