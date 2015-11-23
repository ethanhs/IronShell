//using System;
//using System.ComponentModel;
//using System.Runtime.InteropServices;
//using System.Windows.Forms;
//
//namespace MerulaShell.windows
//{
//    public class ShellHook
//    {
//        private readonly IntPtr windowHandler;
//
//        internal enum ShellEvents
//        {
//            HSHELL_WINDOWCREATED = 1,
//
//            HSHELL_WINDOWDESTROYED = 2,
//
//            HSHELL_ACTIVATESHELLWINDOW = 3,
//
//            HSHELL_WINDOWACTIVATED = 4,
//
//            HSHELL_GETMINRECT = 5,
//
//            HSHELL_REDRAW = 6,
//
//            HSHELL_TASKMAN = 7,
//
//            HSHELL_LANGUAGE = 8,
//
//            HSHELL_ACCESSIBILITYSTATE = 11
//        }
//
//        private readonly UInt32 uMsgNotify;
//        private IntPtr hookHandler;
//        private const int WH_SHELL = 10;
//
//            public ShellHook(IntPtr windowHandler)
//            {
//                this.windowHandler = windowHandler;
//
//                var hook = new HookProc(ShellHookProc);
//
//                IntPtr hMod = Marshal.GetHINSTANCE(typeof(Form).Module);
//
//                hookHandler = SetWindowsHookEx(WH_SHELL, hook, hMod, 0);
//            }
//
//            private delegate int HookProc(int code, IntPtr wParam, IntPtr lParam);
//
//            [DllImport("user32.dll")]
//            private static extern UInt32 RegisterWindowMessageA(string lpString);
//
//            [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
//            private static extern Boolean DeregisterShellHookWindow(IntPtr hwnd);
//
//            [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
//            private static extern Boolean RegisterShellHookWindow(IntPtr hwnd);
//
//            [DllImport("user32.dll", EntryPoint = "SetWindowsHookEx", SetLastError = true)]
//            static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hMod, uint dwThreadId);
//
//
//            /// <summary> 
//            /// shell listener
//            /// </summary> 
//            /// <param name="m"></param> 
//            protected override void WndProc(ref Message m)
//            {
//                if (m.Msg == Convert.ToInt32(uMsgNotify))
//                {
//                    switch ((ShellEvents)m.WParam)
//                    {
//                        case ShellEvents.HSHELL_WINDOWCREATED:
//
//                            Console.WriteLine(m.LParam.ToString()); //this is the handle to the window created 
//
//                            break;
//                    }
//                }
//
//
//                base.WndProc(ref m);
//            }
//
//            public int ShellHookProc(int nCode, IntPtr wParam, IntPtr lParam)
//            {
               // Do some painting here.
//                return CallNextHookEx(hookHandler, nCode, wParam, lParam); 
//            }
//
//            /// <summary>
//            /// Derigisters the shell hook
//            /// </summary>
//            ~ShellHook()
//            {
//                var test = "tre";
//            }
//
//        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
//        static extern IntPtr FindWindowByCaption(int ZeroOnly, string lpWindowName);
//
//        [DllImport("user32.dll")]
//        static extern int CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);
//
//        [DllImport("user32.dll")]
//        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
//
//        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
//        public static extern IntPtr GetModuleHandle(string lpModuleName);
//    }
// }