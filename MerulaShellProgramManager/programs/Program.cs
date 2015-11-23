using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using IWshRuntimeLibrary;
using MerulaShellProgramManager.shell;

namespace MerulaShellProgramManager.programs
{
    class Program:IoItem
    {
        private readonly FileInfo fileInfo;

        public Program(FileInfo fileInfo)
        {
            this.fileInfo = fileInfo;
            try
            {
                Name = fileInfo.Name.Remove(fileInfo.Name.Length - fileInfo.Extension.Length);
            }
            catch (ArgumentOutOfRangeException)
            {
                Debug.WriteLine("Skipping file without extension:  " + fileInfo.Name);
                Name = "";
            }

            Path = fileInfo.FullName;
        }

        public override BitmapSource GetLargeIcon()
        {
            return GetIcon(IconReader.IconSize.Large);
        }

        public override BitmapSource GetSmallIcon()
        {
            return GetIcon(IconReader.IconSize.Small);
        }

        private BitmapSource GetIcon(IconReader.IconSize size)
        {
            var path = Path;
            if (Path.ToLower().EndsWith(".lnk"))
            {
                var shell = new WshShell();
                var link = (IWshShortcut)shell.CreateShortcut(Path);
                if (link.TargetPath != string.Empty)
                {
                    var info = new FileInfo(link.TargetPath);
                    if (info.Exists)
                        path = link.TargetPath;
                }
            }

            var ic = IconReader.GetFileIcon(path, size, false);
            var icon = Imaging.CreateBitmapSourceFromHBitmap((ic.ToBitmap()).GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            icon.Freeze();
            ic.Dispose();
            return icon;
        }

        public override void Start()
        {
            Process.Start(fileInfo.FullName);
        }
    }
}
