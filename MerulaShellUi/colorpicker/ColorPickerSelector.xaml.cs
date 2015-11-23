using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MerulaShellUi.dock;

namespace MerulaShellUi.colorpicker
{
    /// <summary>
    /// Interaction logic for ColorPickerSelector.xaml
    /// </summary>
    public partial class ColorPickerSelector : UserControl
    {
        private readonly SharedSettings settings;

        public ColorPickerSelector()
        {
            InitializeComponent();
            OldColor = Colors.Black;
            settings = SharedSettings.GetInstance();
            settings.ColorsUpdated += SettingsColorsUpdated;
            ColorComponents();
        }

        void SettingsColorsUpdated(object sender, EventArgs e)
        {
            ColorComponents();
        }

        private void ColorComponents()
        {
            
            oldColor.BorderBrush = settings.UiForeBrush;
            newColor.BorderBrush = settings.UiForeBrush;
            btnOk.Foreground = settings.UiForeBrush;
            btnOk.Background = settings.UiGradientBrushV;
            btnCancel.Foreground = settings.UiForeBrush;
            btnCancel.Background = settings.UiGradientBrushV;
        }

        public event EventHandler SelectedColorChanged;

        public event EventHandler ColorSelected;

        public event EventHandler Cancelled;

        public void InvokeCancelled(EventArgs e)
        {
            EventHandler handler = Cancelled;
            if (handler != null) handler(this, e);
        }

        public void InvokeColorSelected(EventArgs e)
        {
            EventHandler handler = ColorSelected;
            if (handler != null) handler(this, e);
        }

        public void InvokeSelectedColorChanged(EventArgs e)
        {
            EventHandler handler = SelectedColorChanged;
            if (handler != null) handler(this, e);
        }

        public Color OldColor
        {
            get { return ((SolidColorBrush) oldColor.Background).Color;    }
            set
            {
                oldColor.Background = new SolidColorBrush(value);
                SelectedColor = value;
            }
        }

        public Color SelectedColor
        {
            get { return ((SolidColorBrush)newColor.Background).Color; }
            private set { newColor.Background = new SolidColorBrush(value); }
        }

        private void ColorPickerColorChanged(object sender, EventArgs e)
        {
            SelectedColor = ColorPicker.GetRgbColor();
            InvokeSelectedColorChanged(new EventArgs());
        }

        public bool Selected { get; set; }
        private void BtnOkClick(object sender, RoutedEventArgs e)
        {
            Selected = true;
            InvokeColorSelected(new EventArgs());
            Close();
        }

        private void BtnCancelClick(object sender, RoutedEventArgs e)
        {
            InvokeCancelled(new EventArgs());
            Close();
        }

        private void Close()
        {
            ((SlideMenu)((Decorator)Parent).Parent).SlideUp();
        }
    }
}
