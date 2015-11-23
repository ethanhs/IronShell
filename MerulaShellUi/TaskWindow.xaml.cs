using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using MerulaShellController.ManageWindows;
using MerulaShellUi.dock;
using MerulaShellUi.workspace;

namespace MerulaShellUi
{
    /// <summary>
    /// Interaction logic for TaskWindow.xaml
    /// </summary>
    public partial class TaskWindow : Window
    {
        private bool dragging;
        private Point beginPoint;
        private const int MinnimumHeigth= 32;

        private SolidColorBrush UiBrush { get; set; }

        private readonly SharedSettings settings;
        private WorkspaceSetter setter;

        public TaskWindow()
        {
            InitializeComponent();
            settings = SharedSettings.GetInstance();

            ColorCompenents();
            settings.ColorsUpdated += SettingsColorsUpdated;
            Loaded += MainWindowLoaded;
            LocationChanged += TaskWindowLocationChanged;
            var menu = new MenuWindow();
            menu.Show();

            var desktop = new Desktop();
            desktop.Show();

            setter = new WorkspaceSetter(this, menu, desktop);

        }

        void TaskWindowLocationChanged(object sender, System.EventArgs e)
        {
            this.DockBottom();
        }

        void SettingsColorsUpdated(object sender, System.EventArgs e)
        {
            ColorCompenents();
        }

        private void MainWindowLoaded(object sender, RoutedEventArgs e)
        {
            settings.TaskWindowHandle = new WindowInteropHelper(this).Handle;
            settings.TaskWindow = this;

            var w = new MerulaShell.windows.Window(settings.TaskWindowHandle);
            w.SetTopMost();
            

            Height = settings.TaskbarHeight;
            var manager = new ManageWindows();
            manager.AddException(settings.TaskWindowHandle);
            UiBrush = new SolidColorBrush(settings.UiColor);
            this.DockBottom();
            //this.Topmost = true;

            
        }

        private void ColorCompenents()
        {
            ResizeGrip.Background = settings.UiForeBrushGlass;
            DragGrip.Background = settings.UiBrushGlass;
            Background = settings.UiGradientBrushV;

        }

        private void ResizeGripMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            beginPoint = e.GetPosition(this);
            dragging = true;
            // Ensure object receives all mouse events.
            ResizeGrip.CaptureMouse();
        }

        private void ResizeGripMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ResizeGrip.ReleaseMouseCapture();
            dragging = false;
            
        }

        private void ResizeGripMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (!dragging) return;

            var currentPos = e.GetPosition(this);
            var newYpos = currentPos.Y - beginPoint.Y;
            if ((Height - newYpos) < MinnimumHeigth)
                Height = MinnimumHeigth;
            else
                Height -= newYpos;

            settings.TaskbarHeight = (int) Height;
            this.DockBottom();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
