using System;
using System.Timers;
using System.Windows.Controls;

namespace MerulaShellUi.menu
{
    /// <summary>
    /// Interaction logic for clock.xaml
    /// </summary>
    public partial class Clock : UserControl
    {
        private readonly Timer timeChecker;
        public Clock()
        {
            InitializeComponent();

            timeChecker = new Timer(1000);
            timeChecker.Elapsed += TimeCheckerElapsed;
            timeChecker.Start();
            SetTime();
        }

        void TimeCheckerElapsed(object sender, ElapsedEventArgs e)
        {
            Dispatcher.Invoke(new DelegateVoid(SetTime));
        }

        private delegate void DelegateVoid();

        private void SetTime()
        {
            tbTime.Text = DateTime.Now.ToShortTimeString();
            tbTime.ToolTip = DateTime.Now.ToLongDateString();
        }
    }
}
