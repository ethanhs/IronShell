using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace MerulaShellUi.dock
{
    /// <summary>
    /// Interaction logic for SlideMenu.xaml
    /// </summary>
    public partial class SlideMenu : Window
    {
        private SharedSettings settings;

        public SlideMenu(Dock docking, double width = 0)
        {
            InitializeComponent();
            Width = width;
            Show();
            Focus();
            SetDock(docking);

            Closed += SlideMenuClosed;
            
        }

        void SlideMenuClosed(object sender, EventArgs e)
        {
            IsClosed = true;
        }

        void SlideMenuLoaded(object sender, RoutedEventArgs e)
        {
            settings = SharedSettings.GetInstance();
            settings.ColorsUpdated += SettingsColorsUpdated;
            ColorComponents();
        }

        void SettingsColorsUpdated(object sender, EventArgs e)
        {
            ColorComponents();
        }

        public bool FreezeSlideUp { get; set; }

        private void SetDock(Dock docking)
        {
            switch (docking)
            {
                case Dock.Top:
                    DockTop();
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private void DockTop()
        {
            var screenWidth = SystemParameters.PrimaryScreenWidth;
            if (Width < 10)
                Width = screenWidth-20;
            Top = -ActualHeight;
            Left = (screenWidth-Width)/2;

            Deactivated += new EventHandler(SlideMenuTopLostFocus);

            var slideAni = new DoubleAnimation(Top, -5, new Duration(TimeSpan.FromMilliseconds(200)));
            BeginAnimation(TopProperty,slideAni);

            
        }

        private void SlideMenuTopLostFocus(object sender, EventArgs e)
        {
            if(!FreezeSlideUp)
                SlideUp();
        }

        public void SlideUp()
        {
            var slideAni = new DoubleAnimation(-5, -ActualHeight, new Duration(TimeSpan.FromMilliseconds(200)));
            slideAni.Completed += CloseAniCompleted;
            BeginAnimation(TopProperty, slideAni);
        }

        void CloseAniCompleted(object sender, EventArgs e)
        {
            Close();
        }

        private void ColorComponents()
        {
            
            bdrHolder.BorderBrush = settings.UiForeBrushGlass;
            bdrHolder.Background  = settings.UiGradientBrushV;
        }

        public UIElement InnerControl
        {
            get { return bdrHolder.Child;  }
            set
            {
                bdrHolder.Child = value;
            }
        }



        public bool IsClosed { get; set; }
    }
}
