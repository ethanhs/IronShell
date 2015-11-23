using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using MerulaShellProgramManager.shell;

namespace MerulaShellProgramManager.programs
{
    class Folder:IoItem
    {
        private readonly DirectoryInfo info;

        public Folder(DirectoryInfo info)
        {
            this.info = info;
            if(info.Name == info.Root.Name)
            {
                var drive = new DriveInfo(info.Root.Name);
      
                if (drive.VolumeLabel != string.Empty)
                    Name = string.Format("{0} ({1})", drive.VolumeLabel, drive.Name);
                else
                    Name = drive.Name;
            }else
                Name = info.Name;
            Path = info.FullName;
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
            var ic = IconReader.GetFolderIcon(Path, size, IconReader.FolderType.Open);
            var icon = Imaging.CreateBitmapSourceFromHBitmap((ic.ToBitmap()).GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            icon.Freeze();
            ic.Dispose();
            return icon;
        }

        public override void Start()
        {
            Process.Start(info.FullName);
        }
    }
}
