using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MerulaShellUi.colorpicker
{
    /// <summary>
    /// Interaction logic for ColorSelector.xaml
    /// </summary>
    public partial class ColorSelector : UserControl
    {
        public ColorSelector()
        {
            InitializeComponent();
        }

        public event EventHandler ColorChanged;

        public void InvokeColorChanged(EventArgs e)
        {
            EventHandler handler = ColorChanged;
            if (handler != null) handler(this, e);
        }

        private bool dragging;

        public int H { get; set; }

        private void BorderMouseDown(object sender, MouseButtonEventArgs e)
        {
            dragging = true;
            bdrSelector.CaptureMouse();
        }

        private void BorderMouseUp(object sender, MouseButtonEventArgs e)
        {
            dragging = false;
            bdrSelector.ReleaseMouseCapture();
            UpdateColor(e.GetPosition(bdrSelector));
        }

        private void BorderMouseMove(object sender, MouseEventArgs e)
        {
            if (!dragging) return;
            UpdateColor(e.GetPosition(bdrSelector));
        }

        private void UpdateColor(Point currentPos)
        {
            
            H = (int)Math.Round((360 - ((currentPos.Y / bdrSelector.ActualHeight) * 360)));
            if (H > 360)
            {
                H = 360;
            }
            else if (H < 0)
            {
                H = 0;
            }
            InvokeColorChanged(new EventArgs());
        }

        /// <summary>
        /// Get the selected Color
        /// </summary>
        /// <returns></returns>
        public Color GetRgbColor()
        {
            return HsbValueConverter.HsbToRgb(new HsbValueConverter.HsbColor
                                           {
                                               A = 1,
                                               B = 1,
                                               S = 1,
                                               H = H
                                           });
        }
    }
}
