using System;
using System.Windows.Controls;
using MerulaShellController;

namespace MerulaShellUi.dock
{
    /// <summary>
    /// Interaction logic for ShutdownMenu.xaml
    /// </summary>
    public partial class ShutdownMenu : UserControl
    {
        private SharedSettings settings;

        public ShutdownMenu()
        {
            InitializeComponent();
            settings = SharedSettings.GetInstance();
            settings.ColorsUpdated += new System.EventHandler(SettingsColorsUpdated);
            ColorComponents();
        }

        private void ColorComponents()
        {
            btnShutdown.Foreground = settings.UiForeBrush;
            btnShutdown.Background = settings.UiGradientBrushV;
            btnRestart.Foreground = settings.UiForeBrush;
            btnRestart.Background = settings.UiGradientBrushV;
            btnHibernate.Foreground = settings.UiForeBrush;
            btnHibernate.Background = settings.UiGradientBrushV;
        }

        void SettingsColorsUpdated(object sender, System.EventArgs e)
        {
            ColorComponents();
        }

        private void BtnShutdownClick(object sender, System.Windows.RoutedEventArgs e)
        {
            PowerOptions.Shutdown();
        }

        private void BtnRestartClick(object sender, System.Windows.RoutedEventArgs e)
        {
            PowerOptions.Restart();
        }

        private void BtnHibernateClick(object sender, System.Windows.RoutedEventArgs e)
        {
            PowerOptions.Hibernate();
        }
    }
}
