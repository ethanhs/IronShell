using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace MerulaShell.windows
{
    public class Thumbnail
    {
        private readonly IntPtr windowHandler;

        private IntPtr thumb = IntPtr.Zero;
        public IntPtr Thumb
        {
            get { return thumb; }
        }

        public Thumbnail(IntPtr windowHandler)
        {
            this.windowHandler = windowHandler;
        }

        /// <summary>
        /// Creates a live thumb
        /// </summary>
        /// <param name="userHandle">the handle of your window</param>
        /// <param name="area">a rect of a area in the window</param>
        /// <param name="widthArea">Width of the area</param>
        /// <param name="heightArea">Height of the area</param>
        public Point GetThumb(IntPtr userHandle, Rect area, int widthArea, int heightArea)
        {
            if (thumb == IntPtr.Zero)
                DwmRegisterThumbnail(userHandle, windowHandler, out thumb);
            return UpdateThumb(area, widthArea,heightArea);

        }

        /// <summary>
        /// Deletes the thumb
        /// </summary>
        public void Unregister()
        {
            if (thumb == IntPtr.Zero) return;
            DwmUnregisterThumbnail(thumb);
            thumb = IntPtr.Zero;
        }

        static readonly int DWM_TNP_VISIBLE = 0x8;
        static readonly int DWM_TNP_OPACITY = 0x4;
        static readonly int DWM_TNP_RECTDESTINATION = 0x1;
        static readonly int DWM_TNP_SOURCECLIENTAREAONLY = 0x00000010;

        private Point UpdateThumb(Rect area, int width, int height)
        {
            if (thumb != IntPtr.Zero)
            {
                PSIZE size;
                DwmQueryThumbnailSourceSize(thumb, out size);

                DWM_THUMBNAIL_PROPERTIES props = new DWM_THUMBNAIL_PROPERTIES();
                props.dwFlags = DWM_TNP_VISIBLE | DWM_TNP_RECTDESTINATION | DWM_TNP_OPACITY | DWM_TNP_SOURCECLIENTAREAONLY;

                props.fVisible = true;
                props.fSourceClientAreaOnly = true;
                props.opacity = 255;

                props.rcDestination.Bottom = height;

                props.rcDestination = area;

                var scaleFactor = height / (double)size.y;
                int scaledX = (int)(size.x * scaleFactor);
                if (scaledX < width)
                {
                    props.rcDestination.Left += (width/2) - (scaledX/2);
                    props.rcDestination.Right += (width/2) - (scaledX/2);
                }

                scaleFactor = width / (double)size.x;
                int scaledY = (int)(size.y * scaleFactor);
                if (scaledY < height)
                {
                    props.rcDestination.Top += (height / 2) - (scaledY / 2);
                    props.rcDestination.Bottom += (height / 2) - (scaledY / 2);
                }

                DwmUpdateThumbnailProperties(thumb, ref props);
                return new Point(scaledX,scaledY);
            }
            return new Point(0,0);
        }

        ~Thumbnail()
        {
            if (thumb != IntPtr.Zero)
                DwmUnregisterThumbnail(thumb);
        }

        #region api
        [DllImport("dwmapi.dll")]
        private static extern int DwmRegisterThumbnail(IntPtr dest, IntPtr src, out IntPtr thumb);

        [DllImport("dwmapi.dll")]
        private static extern int DwmUnregisterThumbnail(IntPtr thumb);

        [DllImport("dwmapi.dll")]
        private static extern int DwmQueryThumbnailSourceSize(IntPtr thumb, out PSIZE size);

        [StructLayout(LayoutKind.Sequential)]
        internal struct PSIZE
        {
            public int x;
            public int y;
        }

        [DllImport("dwmapi.dll")]
        static extern int DwmUpdateThumbnailProperties(IntPtr hThumb, ref DWM_THUMBNAIL_PROPERTIES props);

        [StructLayout(LayoutKind.Sequential)]
        internal struct DWM_THUMBNAIL_PROPERTIES
        {
            public int dwFlags;
            public Rect rcDestination;
            public Rect rcSource;
            public byte opacity;
            public bool fVisible;
            public bool fSourceClientAreaOnly;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Rect
        {
            internal Rect(int left, int top, int right, int bottom)
            {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }

            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
        #endregion

        
    }
}
