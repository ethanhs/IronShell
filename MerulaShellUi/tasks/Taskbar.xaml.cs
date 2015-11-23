using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using MerulaShellController.ManageWindows;
using System.Linq;

namespace MerulaShellUi.tasks
{
    /// <summary>
    /// Interaction logic for Taskbar.xaml
    /// </summary>
    public partial class Taskbar : UserControl
    {
        private bool dragging = false;
        private Point beginPoint;
        private double animationValue;
        private readonly ManageWindows windowManager = new ManageWindows();

        private const int ScrollStepSize = 100;

        public Taskbar()
        {
            InitializeComponent();
            windowManager.WindowListChanged += WindowManagerWindowListChanged;
            pnlTasks.LayoutUpdated += PnlTasksLayoutUpdated;
            CreateTasks();
        }

        void PnlTasksLayoutUpdated(object sender, EventArgs e)
        {
            UpdateTasks();
        }

        private void WindowManagerWindowListChanged(object sender, EventArgs e)
        {
            Dispatcher.Invoke(new DelegateVoid(CreateTasks));
        }

        private delegate void DelegateVoid();

        private void CreateTasks()
        {
            RemoveTasks();
            foreach (var window in windowManager.GetWindows())
            {
                Console.Out.WriteLine(window.Handler);
                var task = new Task(window);
                task.WindowSetActive += TaskWindowSetActive;
                TaskHolder.Children.Add(task);
            }
        }

        void TaskWindowSetActive(object sender, EventArgs e)
        {
            var senderTask = (Task) sender;
            var tasks = from task in TaskHolder.Children.Cast<Task>()
                        where task != senderTask
                        select task;
            foreach (var t in tasks)
            {
                t.SetNonActive();
            }
        }

        private void TaskBarMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (pnlTasks.ActualWidth < this.ActualWidth) return;
            // Set the beginning position of the mouse.
            beginPoint = e.GetPosition(cvHolder);
            if (!double.IsNaN(Canvas.GetLeft(pnlTasks)))
                beginPoint.X -= Canvas.GetLeft(pnlTasks);
            dragging = true;

            // Ensure object receives all mouse events.
            pnlTasks.CaptureMouse();
        }

        private void TaskBarMouseUp(object sender, MouseButtonEventArgs e)
        {
            dragging = false;
            pnlTasks.ReleaseMouseCapture();
        }

        private void TaskBarMouseMove(object sender, MouseEventArgs e)
        {
            if (!dragging) return;
            
            var currentPos = e.GetPosition(cvHolder);
            var newXpos = currentPos.X - beginPoint.X;

            if (newXpos > 0)
                newXpos = 0;
            else if ((pnlTasks.ActualWidth - ActualWidth) < -newXpos)
                newXpos = -(pnlTasks.ActualWidth - ActualWidth);

            Canvas.SetLeft(pnlTasks, newXpos);

            // Update the beginning position of the mouse.
            //beginPoint = currentPos;
        }


        private void CenterTaskBar()
        {
            var newLeft = (this.ActualWidth / 2) - (pnlTasks.ActualWidth / 2);
            var from = Canvas.GetLeft(pnlTasks);
            if (double.IsNaN(from))
                from = 0;
            var ani = new DoubleAnimation(from, newLeft, TimeSpan.FromMilliseconds(300))
                             {
                                 FillBehavior = FillBehavior.Stop,
                                 EasingFunction = new PowerEase()
                             };

            ani.Completed += AniCompleted;
            animationValue = newLeft;
            pnlTasks.BeginAnimation(Canvas.LeftProperty, ani);
            //Canvas.SetLeft(pnlTasks,newLeft);
        }

        void AniCompleted(object sender, EventArgs e)
        {
            Canvas.SetLeft(pnlTasks, animationValue);
        }

        private void ControlSizeChanged(object sender, SizeChangedEventArgs e)
        { 
            CenterTaskBar();
        }
        /// <summary>
        /// Updates all the tasks
        /// </summary>
        private void UpdateTasks()
        {
            foreach (var child in TaskHolder.Children)
            {
                if(!(child is Task)) continue;
                ((Task)child).Update();
            }
        }

        /// <summary>
        /// Removes all the tasks
        /// </summary>
        private void RemoveTasks()
        {
            TaskHolder.Children.Clear();
        }

        private void PnlTasksMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (pnlTasks.ActualWidth < this.ActualWidth) return;

            int pixelsToMove = (e.Delta / 120) * ScrollStepSize;
            if (pixelsToMove == 0) return;

            var from = Canvas.GetLeft(pnlTasks);
            var newLeft = from + pixelsToMove;

            if (newLeft > 0)
                newLeft = 0;
            else if ((pnlTasks.ActualWidth - ActualWidth) < -newLeft)
                newLeft = -(pnlTasks.ActualWidth - ActualWidth);
           
            Canvas.SetLeft(pnlTasks,newLeft);
        }
    }
}
