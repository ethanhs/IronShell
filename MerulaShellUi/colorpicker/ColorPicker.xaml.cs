using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MerulaShellUi.colorpicker
{
    /// <summary>
    /// Interaction logic for ColorPicker.xaml
    /// </summary>
    public partial class ColorPicker : UserControl
    {
        public ColorPicker()
        {
            InitializeComponent();
        }

        public event EventHandler ColorChanged;

        public void InvokeColorChanged(EventArgs e)
        {
            EventHandler handler = ColorChanged;
            if (handler != null) handler(this, e);
        }

        private void CsColorChangedHandler(object sender, EventArgs e)
        {
            bdrMainColor.Background = new SolidColorBrush(colorSelector.GetRgbColor());
        }

        private bool dragging;

        public double S;

        public double B;

        private void BorderMouseDown(object sender, MouseButtonEventArgs e)
        {
            dragging = true;
            bdrOverlay.CaptureMouse();
        }

        private void BorderMouseUp(object sender, MouseButtonEventArgs e)
        {
            dragging = false;
            bdrOverlay.ReleaseMouseCapture();
            UpdateColor(e.GetPosition(bdrOverlay));
        }

        private void BorderMouseMove(object sender, MouseEventArgs e)
        {
            if (!dragging) return;
            UpdateColor(e.GetPosition(bdrOverlay));
        }

        private void UpdateColor(Point currentPos)
        {
            B = Math.Round(1-(currentPos.Y / bdrOverlay.ActualHeight),2)*100;
            if (B > 100)
            {
                B = 100;
            }else if(B < 0)
            {
                B = 0;
            }
            B /= 100;
            S = Math.Round(currentPos.X / bdrOverlay.ActualWidth, 2)*100;
            if (S > 100)
            {
                S = 100;
            }else if(S < 0)
            {
                S = 0;
            }
            S /= 100;
            InvokeColorChanged(new EventArgs());
        }

        public Color GetRgbColor()
        {
            return HsbValueConverter.HsbToRgb(new HsbValueConverter.HsbColor
                                                  {
                                                      A = 1,
                                                      B = B,
                                                      H = colorSelector.H,
                                                      S = S
                                                  });
        }
    }
}
