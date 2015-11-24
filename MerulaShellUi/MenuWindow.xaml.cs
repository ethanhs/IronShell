using System;
using SystemInfo;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using MerulaShellController.ManageWindows;
using MerulaShellUi.dock;

namespace MerulaShellUi
{
    /// <summary>
    /// Interaction logic for MenuWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        private readonly SharedSettings settings;
        private SolidColorBrush uiBrush;
        private LinearGradientBrush uiGradientBrush;

        public MenuWindow()
        {
            InitializeComponent();
            settings = SharedSettings.GetInstance();
            ColorCompenents();
            settings.ColorsUpdated += SettingsColorsUpdated;
            Loaded += MainWindowLoaded;
            LocationChanged += MenuWindowLocationChanged;
            var Battery =  new SystemInfo.SystemInfo();
            if (Battery.BatteryConnected == true)
            {
                ucBattery.Text = "Battery Remaining: " + Battery.BatteryPercent +"%";
            } else {
                ucBattery.Text = "";
            }
            
        }
        void MenuWindowLocationChanged(object sender, EventArgs e)
        {
            this.DockTop();
        }

        void SettingsColorsUpdated(object sender, EventArgs e)
        {
            ColorCompenents();
        }

        private void MainWindowLoaded(object sender, RoutedEventArgs e)
        {
            var manager = new ManageWindows();
            manager.AddException(new WindowInteropHelper(this).Handle);
            uiBrush = new SolidColorBrush(settings.UiColor);
            this.DockTop();
        }

        private void ColorCompenents()
        {
            windowBorder.Background = settings.UiForeBrushGlass;
            Background = settings.UiGradientBrushV;
            ucClock.Foreground = settings.UiForeBrush;
            btnSettings.Background = settings.UiForeBrushGlassL;
            btnSettings.Foreground = settings.UiForeBrush;

            btnShutdown.Background = settings.UiForeBrushGlassL;
            btnShutdown.Foreground = settings.UiForeBrush;

            btnLocation.Background = settings.UiForeBrushGlassL;
            btnLocation.Foreground = settings.UiForeBrush;

            btnPrograms.Background = settings.UiForeBrushGlassL;
            btnPrograms.Foreground = settings.UiForeBrush;
        }

        private bool settingsOpen;
        private SlideMenu settingsMenu;

        private void SettingsClick(object sender, RoutedEventArgs e)
        {
            if (settingsOpen)
            {
                settingsOpen = false;
                return;
            } 
            var settingsControl = new SettingsControl();
            settingsMenu = new SlideMenu(Dock.Top, settingsControl.Width) { InnerControl = settingsControl };
            settingsOpen = true;
        }

        private bool shutdownOpen;
        private SlideMenu shutdownMenu;

        private void ShutdownClick(object sender, RoutedEventArgs e)
        {
            if (shutdownOpen)
            {
                shutdownOpen = false;
                return;
            } 
            var shutdown = new ShutdownMenu();
            shutdownMenu = new SlideMenu(Dock.Top, shutdown.Width) { InnerControl = shutdown };
            shutdownOpen = true;
        }

        private void SettingsMouseEnter(object sender, MouseEventArgs e)
        {
            if (shutdownMenu != null)
                shutdownOpen = !shutdownMenu.IsClosed;
            if (settingsMenu != null)
                settingsOpen = !settingsMenu.IsClosed;
            if (locationMenu != null)
                locationOpen = !locationMenu.IsClosed;
            if (programMenu != null)
                programOpen = !programMenu.IsClosed;
        }

        private bool locationOpen;
        private SlideMenu locationMenu;
        private void LocationClick(object sender, RoutedEventArgs e)
        {
            if (locationOpen)
            {
                locationOpen = false;
                return;
            }
            var locations = new LocationMenu();
            locationMenu = new SlideMenu(Dock.Top, locations.Width) { InnerControl = locations };
            locationOpen = true;
        }

        private bool programOpen;
        private SlideMenu programMenu;
        private void ProgramsClick(object sender, RoutedEventArgs e)
        {
            if (programOpen)
            {
                programOpen = false;
                return;
            }
            var programs = new ProgramMenu();
            programMenu = new SlideMenu(Dock.Top) { InnerControl = programs };
            programOpen = true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
