
using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MerulaShellUi.colorpicker;
using Microsoft.Win32;

namespace MerulaShellUi.dock
{
    /// <summary>
    /// Interaction logic for SettingsControl.xaml
    /// </summary>
    public partial class SettingsControl : UserControl
    {
        private readonly SharedSettings settings;

        public SettingsControl()
        {
            InitializeComponent();

            settings = SharedSettings.GetInstance();
            settings.ColorsUpdated += SettingsColorsUpdated;
            ColorComponents();
            LoadOptions();

            slGlass.ValueChanged += slGlass_ValueChanged;
            cbHideTaskbar.Click += CbHideTaskbarClick;
        }

        void slGlass_ValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<double> e)
        {
            settings.Glass = (int) e.NewValue;
        }

        void SettingsColorsUpdated(object sender, EventArgs e)
        {
            ColorComponents();
        }

        void CbHideTaskbarClick(object sender, System.Windows.RoutedEventArgs e)
        {
            settings.TaskbarAlwaysVisible = !cbHideTaskbar.IsChecked.Value;
        }

        private void LoadOptions()
        {
            cpBackground.Background = settings.UiBrush;
            cpForeGround.Background = settings.UiForeBrush;
            slGlass.Value = settings.Glass;
            cbHideTaskbar.IsChecked = !settings.TaskbarAlwaysVisible;
        }

        private void ColorComponents()
        {
            icon.Foreground = settings.UiForeBrushGlassL;
            Foreground = settings.UiForeBrush;
            
            cbHideTaskbar.Foreground = settings.UiForeBrush;
            cpBackground.Foreground = settings.UiForeBrush;
            cpForeGround.Foreground = settings.UiForeBrush;
            tbGlass.Foreground = settings.UiForeBrush;

            btnWallpaper.Foreground = settings.UiForeBrush;
            btnWallpaper.Background = settings.UiGradientBrushV;
        }

        private void CpForeGroundClick(object sender, System.Windows.RoutedEventArgs e)
        {
            ((SlideMenu)((Decorator)Parent).Parent).FreezeSlideUp = true;
            var selector = new ColorPickerSelector {OldColor = settings.UiColorFore};
            selector.ColorSelected += SelectorForeColorSelected;
            selector.SelectedColorChanged += SelectorForeColorChanged;
            var slider = new SlideMenu(Dock.Top, 300) { InnerControl = selector };
            slider.Closed += SelectorForeCancelled;
        }

        void SelectorForeCancelled(object sender, System.EventArgs e)
        {
            var selector = (ColorPickerSelector)((SlideMenu)sender).InnerControl;
            if (selector.Selected) return; 
            SetForeColor(selector.OldColor);
            RemoveFreeze();
        }
        void SelectorBackCancelled(object sender, System.EventArgs e)
        {
            var selector = (ColorPickerSelector)((SlideMenu)sender).InnerControl;
            if (selector.Selected) return; 
            SetBackColor(selector.OldColor);
            RemoveFreeze();
        }

        void SelectorForeColorSelected(object sender, System.EventArgs e)
        {
            var selector = (ColorPickerSelector) sender;
            SetForeColor(selector.SelectedColor);
            RemoveFreeze();
        }
        void SelectorForeColorChanged(object sender, System.EventArgs e)
        {
            var selector = (ColorPickerSelector)sender;
            SetForeColor(selector.SelectedColor);
        }

        private void SetForeColor(Color c)
        {
            cpForeGround.Background = new SolidColorBrush(c);
            settings.UiColorFore = c;
        }

        private void RemoveFreeze()
        {
            ((SlideMenu)((Decorator)Parent).Parent).FreezeSlideUp = false;
            ((SlideMenu) ((Decorator) Parent).Parent).Activate();
        }

        private void CpBackgroundClick(object sender, System.Windows.RoutedEventArgs e)
        {
            ((SlideMenu)((Decorator)Parent).Parent).FreezeSlideUp = true;
            var selector = new ColorPickerSelector { OldColor = settings.UiColor };
            selector.ColorSelected += SelectorBackColorSelected;
            selector.SelectedColorChanged+=SelectorBackColorChanged;
            var slider = new SlideMenu(Dock.Top, 300) { InnerControl = selector };
            slider.Closed += SelectorBackCancelled;
        }

        private void SelectorBackColorSelected(object sender, EventArgs e)
        {
            var selector = (ColorPickerSelector)sender;
            SetBackColor(selector.SelectedColor);
            RemoveFreeze();
        }

        private void SelectorBackColorChanged(object sender, EventArgs e)
        {
           
            var selector = (ColorPickerSelector)sender;
            SetBackColor(selector.SelectedColor);
           
        }

        private void SetBackColor(Color c)
        {
            cpBackground.Background = new SolidColorBrush(c);
            settings.UiColor = c;
        }

        private void SetWallpaperClick(object sender, System.Windows.RoutedEventArgs e)
        {
            ((SlideMenu)((Decorator)Parent).Parent).FreezeSlideUp = true;
            var dialog = new OpenFileDialog()
                             {
                                 Filter = "Wallpapers|*.jpg;*.jpeg;*.png;*.gif;*.mpeg;*.wmv;*.mp4"
                             };
            dialog.ShowDialog();
            if (dialog.FileName != string.Empty)
                settings.Wallpaper = dialog.FileName;
            RemoveFreeze();
        }

        private void BtnPinTiles(object sender, RoutedEventArgs e)
        {
            ((SlideMenu)((Decorator)Parent).Parent).FreezeSlideUp = true;
            var tile= new ListDictionary();
            MessageBox.Show("Please select the file you would like to run.");
            var dialog = new OpenFileDialog();
            dialog.ShowDialog();
            if (dialog.FileName != string.Empty)
            {
                tile.Add("Program", dialog.FileName);
            }
            var res = MessageBox.Show("Please choose the image you would like to use. If you would like to use the normal program's icon, close this dialog.");
            if (res.Equals(MessageBoxResult.Cancel))
            {
                var colorpick = new colorpicker.ColorPickerSelector();
            }


        }
    }
}
