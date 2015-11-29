using System;
using System.Net.Mime;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using MerulaShellProgramManager;
using MerulaShellProgramManager.programs;

namespace MerulaShellUi.dock
{
    /// <summary>
    /// Interaction logic for LocationMenu.xaml
    /// </summary>
    public partial class LocationMenu : UserControl
    {
        public LocationMenu()
        {
            InitializeComponent();
            LoadLocations();
            var settings = SharedSettings.GetInstance();
            icon.Foreground = settings.UiForeBrushGlassL;
            this.Loaded += LocationMenu_Loaded;
        }

        void LocationMenu_Loaded(object sender, RoutedEventArgs e)
        {
            ((SlideMenu) ((Decorator) Parent).Parent).Height = pnlButtons.Children.Count*34+24;
        }

        private void LoadLocations()
        {
            var manager = new ProgramManager();
            var locations = manager.GetSpecialLocations();
            foreach (var location in locations)
            {
                CreateButton(location);
            }
        }

        private void CreateButton(IoItem location)
        {
            var settings = SharedSettings.GetInstance();

            var content = new WrapPanel();
            content.Children.Add(new Image { Source = location.GetSmallIcon(), Height = 16, Width = 16, Margin = new Thickness(2) });
            content.Children.Add(new TextBlock { Text = location.Name, Margin = new Thickness(2) });
            var btnLocation = new Button
                                  {
                                      Content = content,
                                      Tag = location,
                                      HorizontalContentAlignment = HorizontalAlignment.Left,
                                      Foreground = settings.UiForeBrush,
                                      Background = settings.UiGradientBrushV,
                                      Margin = new Thickness(5)
                                  };
            btnLocation.Click += btnLocation_Click;

            pnlButtons.Children.Add(btnLocation);
        }

        private void btnLocation_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var win = new Explore10.MainWindow( ((IoItem)((Button) sender).Tag).Path);
            win.Show();

        }
    }
}
