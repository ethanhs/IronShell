using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Media;

namespace MerulaShellUi
{
    public class SharedSettings
    {
        private static SharedSettings settings;

        public event EventHandler ColorsUpdated;

        public void InvokeColorsUpdated(EventArgs e)
        {
            EventHandler handler = ColorsUpdated;
            if (handler != null) handler(this, e);
        }

        private SharedSettings()
        {
            //Glass = true;
            //UiColor = Colors.White;
            //UiColorFore = Colors.DarkSlateGray;
            //TaskbarAlwaysVisible = true;
            CreateColors();
        }

        public const int GlassStrengt = 100;

        private void CreateColors()
        {
            var glassBack = UiColor;
            var glassFore = UiColorFore;
            var glassForeLocked = UiColorFore;

                glassFore.A = (byte) Glass;
                glassBack.A = (byte) Glass;
                glassForeLocked.A = GlassStrengt;

            UiBrush = new SolidColorBrush(UiColor);
            UiBrushGlass = new SolidColorBrush(glassBack);
            UiForeBrush = new SolidColorBrush(UiColorFore);
            UiForeBrushGlass = new SolidColorBrush(glassFore);
            UiForeBrushGlassL = new SolidColorBrush(glassForeLocked);
            UiGradientBrushV = new LinearGradientBrush(glassBack, glassFore, new Point(0.5, 0), new Point(0.5, 4));
            InvokeColorsUpdated(new EventArgs());
        }

        public SolidColorBrush UiForeBrushGlassL { get; set; }

        public static SharedSettings GetInstance()
        {
            if (settings == null)
                settings = new SharedSettings();
            return settings;
        }
        
        /// <summary>
        /// The taskwindow
        /// </summary>
        public Window TaskWindow { get; set; }
        /// <summary>
        /// The id of the task window this is the window where the tumbnails are drawn on
        /// </summary>
        public IntPtr TaskWindowHandle { get; set; }

        public Color UiColor
        {
            get
            {
                var color = Properties.Settings.Default.UIColor;
                return color;
            }
            set
            {
                Properties.Settings.Default.UIColor = value;
                Properties.Settings.Default.Save();
                CreateColors();
            }
        }

        public Color UiColorFore    
        {
            get
            {
                return Properties.Settings.Default.UIColorFore;
            }
            set
            {
                Properties.Settings.Default.UIColorFore = value;
                Properties.Settings.Default.Save();
                CreateColors();
            }
        }

        public string Wallpaper 
        {
            get { return Properties.Settings.Default.DesktopImage; }
            set
            {
                Properties.Settings.Default.DesktopImage = value;
                Properties.Settings.Default.Save();
                InvokeWallpaperChanged(new EventArgs());
            }
        }

        public ListDictionary Tiles
        {
            get { return Properties.Settings.Default.Tiles; }
            set
            {
                if (Equals(Properties.Settings.Default.Tiles, null))
                {
                    Properties.Settings.Default.Tiles = new ListDictionary();
                }
                else
                {
                    if (!Equals(null, value))
                    Properties.Settings.Default.Tiles=value;
                }
            }
        } 
        public event EventHandler WallpaperChanged;

        public void InvokeWallpaperChanged(EventArgs e)
        {
            EventHandler handler = WallpaperChanged;
            if (handler != null) handler(this, e);
        }


        public SolidColorBrush UiBrush { get; set; }

        public SolidColorBrush UiForeBrush { get; set; }

        public SolidColorBrush UiBrushGlass { get; set; }

        public SolidColorBrush UiForeBrushGlass { get; set; }

        public GradientBrush UiGradientBrushV { get; set; }


        /// <summary>
        /// when true: taskbar will always be visible
        /// </summary>
        public bool TaskbarAlwaysVisible 
        {
            get
            {
                return Properties.Settings.Default.TaskbarAlwaysVisible;
            }
            set
            {
                Properties.Settings.Default.TaskbarAlwaysVisible = value;
                Properties.Settings.Default.Save();
            }

        }
        /// <summary>
        /// When true the layout has a trasparrent effect.
        /// </summary>
        public int Glass
        {
            get
            {
                return Properties.Settings.Default.Glass;
            }
            set
            {
                Properties.Settings.Default.Glass = value;
                Properties.Settings.Default.Save();
                CreateColors();
            }

        }

        public int TaskbarHeight
        {
            get
            {
                return Properties.Settings.Default.TaskbarHeight;
            }
            set
            {
                Properties.Settings.Default.TaskbarHeight = value;
                Properties.Settings.Default.Save();
            }

        }
        //public  Type { get; set; }
    }
}
