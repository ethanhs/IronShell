using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using MerulaShellController.ManagePrograms;
using MerulaShellUi.desktop;
using MerulaShellUi.dock;

namespace MerulaShellUi
{
    /// <summary>
    /// Interaction logic for Desktop.xaml
    /// </summary>
    public partial class Desktop : Window
    {
        private SharedSettings settings;

        public Desktop()
        {
            InitializeComponent();
            this.DockScreen();
            this.SendToBottom();
            Activated += DesktopActivated;
            Deactivated += DesktopDeactivated;

            LoadButtons();

            settings = SharedSettings.GetInstance();
            settings.WallpaperChanged += SettingsWallpaperChanged;
            SetWallpaper();
        }

        void DesktopDeactivated(object sender, EventArgs e)
        {
            var ani = new DoubleAnimation(root.Opacity, 0, TimeSpan.FromMilliseconds(1000));
            root.BeginAnimation(OpacityProperty, ani);
            ani.Completed += new EventHandler(AniCompleted);
        }

        void AniCompleted(object sender, EventArgs e)
        {
            pnlShortcuts.Visibility = System.Windows.Visibility.Collapsed;
        }

        void SettingsWallpaperChanged(object sender, EventArgs e)
        {
            SetWallpaper();
        }

        private void LoadButtons()
        {
            var programs = new GetPrograms();
            var desktopItems = programs.GetDesktopItems();

            foreach (var desktopItem in desktopItems)
            {
                var btn = new DesktopButton(desktopItem);
                pnlShortcuts.Children.Add(btn);
            }
        }

        public void DrawPanel()
        {
            var w = root.ActualWidth  - (root.ActualWidth/4 );
            var h = root.ActualHeight - (root.ActualHeight/4);
//            if (w < h)
//                w = h;
//            else
//                h = w;
            pnlShortcuts.Width = w;
            pnlShortcuts.Height = h;

        }

        void DesktopActivated(object sender, System.EventArgs e)
        {
            var ani = new DoubleAnimation(root.Opacity, 1, TimeSpan.FromMilliseconds(500));
            root.BeginAnimation(OpacityProperty, ani);
            this.SendToBottom();
        }

        public void SetDesktopMargin(Thickness margin)
        {
            root.Margin = margin;
            DrawPanel();
        }

        private void WindowMouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            
        }

        private void WindowMouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            
        }

        private void SetWallpaper()
        {
            if (settings.Wallpaper.ToLower().EndsWith(".jpg") || settings.Wallpaper.ToLower().EndsWith(".jpeg") || settings.Wallpaper.ToLower().EndsWith(".png") || settings.Wallpaper.ToLower().EndsWith(".gif"))
            {
                Background = new ImageBrush(new BitmapImage(new Uri(settings.Wallpaper)))
                                 {Stretch = Stretch.UniformToFill};
                return;
            }
            var element = new MediaElement
                              {
                                  Source = new Uri(settings.Wallpaper, UriKind.Relative),
                                  IsMuted = true,
                                  Stretch = Stretch.UniformToFill
                              };
            element.MediaEnded += element_MediaEnded;

            var videoBrush = new VisualBrush {Visual = element, Stretch = Stretch.UniformToFill};
            Background = videoBrush;
        }

        void element_MediaEnded(object sender, RoutedEventArgs e)
        {
            var element = (MediaElement) sender;
            element.Position = TimeSpan.FromSeconds(0);
        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
