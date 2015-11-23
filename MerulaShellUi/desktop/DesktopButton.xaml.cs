using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MerulaShellProgramManager.programs;

namespace MerulaShellUi.desktop
{
    /// <summary>
    /// Interaction logic for DesktopButton.xaml
    /// </summary>
    public partial class DesktopButton : UserControl
    {
        private readonly IoItem desktopItem;
        private SharedSettings settings;

        public DesktopButton(IoItem desktopItem)
        {
            this.desktopItem = desktopItem;

            InitializeComponent();

            Background = new ImageBrush(desktopItem.GetLargeIcon()){Stretch = Stretch.None};
            DisplayName = desktopItem.Name;


            settings = SharedSettings.GetInstance();
            ColorComponents();
            settings.ColorsUpdated+=SettingsColorsUpdated;
        }

        private void SettingsColorsUpdated(object sender, EventArgs e)
        {
            ColorComponents();
        }

        private void ColorComponents()
        {
            bdrRoot.Background = settings.UiForeBrushGlassL;
            tbName.Foreground = settings.UiBrush;
        }

        private string DisplayName 
        {
            set { tbName.Text = value; }
        }

        private void UserControlMouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            VisualStateManager.GoToState(this, "hover",true);
        }

        private void UserControlMouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            VisualStateManager.GoToState(this, "normal", true);
        }

        private void UserControlMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            desktopItem.Start();
        }
    }
}
