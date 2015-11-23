using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using MerulaShellProgramManager.shell;

namespace MerulaShellProgramManager.programs
{
    public abstract class IoItem
    {
        public string Name { get; protected set; }
        public string Path { get; protected set; }

        public abstract BitmapSource GetLargeIcon();

        public abstract BitmapSource GetSmallIcon();

        public abstract void Start();

        public bool IsFolder()
        {
            return this is Folder;
        }
    }
}
