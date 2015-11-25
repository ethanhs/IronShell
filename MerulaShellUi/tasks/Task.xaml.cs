using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MerulaShell.windows;
using Window = MerulaShell.windows.Window;

namespace MerulaShellUi.tasks
{
    /// <summary>
    /// Interaction logic for Task.xaml
    /// </summary>
    public partial class Task : UserControl
    {
        private readonly Window window;
        private readonly SharedSettings settings;
        private bool last;
        public Task()
        {
            InitializeComponent();
        }

        public void SetColors()
        {
            btnMain.Background = Brushes.Transparent;
            btnClose.Background = settings.UiForeBrush;
            btnMain.BorderBrush = Brushes.Transparent;
            taskLabel.Foreground = settings.UiForeBrush;
        }

        void UpdateThumb(object sender, object e)
        {
            DrawThumb();
        }

        public Task(Window window)
        {
            
            InitializeComponent();
            this.window = window;

            settings = SharedSettings.GetInstance();
            Loaded += UpdateThumb;
            SizeChanged += UpdateThumb;
            SizeChanged += TaskSizeChanged;

            imgIcon16.Source = window.ProgramIcon;
            imgIcon32.Source = window.ProgramIcon;
            taskLabel.Text = window.Title;

            window.TitleChanged += WindowTitleChanged;

            SetColors();
            settings.ColorsUpdated += SettingsColorsUpdated;
        }

        void SettingsColorsUpdated(object sender, EventArgs e)
        {
            SetColors();
        }

        void WindowTitleChanged(object sender, EventArgs e)
        {
            Dispatcher.Invoke(new DelegateVoid(UpdateTitle));
        }

        private delegate void DelegateVoid();

        private void UpdateTitle()
        {
            taskLabel.Text = window.Title;
        }

        void TaskSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (!AllowDrawThumb() && imgIcon32.Visibility == System.Windows.Visibility.Collapsed)
            {
                imgIcon32.Visibility = System.Windows.Visibility.Visible;
                infoHolder.Visibility = System.Windows.Visibility.Collapsed;

    
                bdrThumb.Visibility = System.Windows.Visibility.Collapsed;
                Remove();
            }
            else if (AllowDrawThumb() && imgIcon32.Visibility == System.Windows.Visibility.Visible)
            {
                infoHolder.Visibility = System.Windows.Visibility.Visible;
                imgIcon32.Visibility = System.Windows.Visibility.Collapsed;

                bdrThumb.Visibility = System.Windows.Visibility.Visible;
            }
            Width = ActualHeight * 1.75;
        }
        /// <summary>
        /// Updates the Thumb
        /// </summary>
        private void DrawThumb()
        {
            if (!AllowDrawThumb() || window.Thumbnail == null) return;
            var point = bdrThumb.TranslatePoint(new Point(0, 0), settings.TaskWindow);
            var thumbSize = window.Thumbnail.GetThumb(settings.TaskWindowHandle, new Thumbnail.Rect
                                                        {
                                                            Top = (int)point.Y,
                                                            Bottom = (int)bdrThumb.ActualHeight + ((int)point.Y), 
                                                            Left = (int)point.X,
                                                            Right = (int)bdrThumb.ActualWidth  + (int)point.X
                                                        }, (int)bdrThumb.ActualWidth, (int)bdrThumb.ActualHeight);
        }

        /// <summary>
        /// Updates the task
        /// </summary>
        public void Update()
        {
            DrawThumb();
        }
        /// <summary>
        /// Removes the thumb
        /// </summary>
        public void Remove()
        {
            window.Thumbnail.Unregister();
        }

        /// <summary>
        /// Indicates if it is allowed to draw thumbs
        /// </summary>
        private bool AllowDrawThumb()
        {
            return ActualHeight > 32;
        }

        private void BtnMainClick(object sender, RoutedEventArgs e)
        {
            if (last)
            {
                window.MaximizeMinimize();
            }
            else
            {
                last = true;
                window.SetToForeground();
            }
            btnMain.IsChecked = true;
            InvokeWindowSetActive(new EventArgs());
        }

        /// <summary>
        /// Sets the control on none active
        /// </summary>
        public void SetNonActive()
        {
            last = false;
            btnMain.IsChecked = false;
        }

        public event EventHandler WindowSetActive;

        public void InvokeWindowSetActive(EventArgs e)
        {
            EventHandler handler = WindowSetActive;
            if (handler != null) handler(this, e);
        }

        private void BtnMainMouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            //btnClose.Visibility = System.Windows.Visibility.Visible;
        }

        private void BtnMainMouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            //btnClose.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void BtnCloseClick(object sender, RoutedEventArgs e)
        {
            window.Close();
        }
    }
}
