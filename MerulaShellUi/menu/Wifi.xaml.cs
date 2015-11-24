using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MerulaShellUi.menu
{
    /// <summary>
    /// Interaction logic for Wifi.xaml
    /// </summary>
    public partial class Wifi : UserControl
    {
        public Wifi()
        {
            InitializeComponent();
            //inital text
            var settings = SharedSettings.GetInstance();
            ucWifi.Foreground = settings.UiForeBrush;
            ucWifi.Text = "Wifi Signal: " + GetWifiSignal();
            //set up timer
            var timer = new Timer(60000);
            timer.Elapsed += Timer_Elapsed;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke((Action) (() => //assures that we can update out of thread?
            {
                ucWifi.Text = "Battery Remaining: " + GetWifiSignal() + "%";
            }));
        }


        private string GetWifiSignal()
        {
            double signal=0;
            
            try
            {
                var netsh= new ProcessStartInfo("netsh", "wlan show interfaces");
                netsh.WorkingDirectory= Environment.GetFolderPath(Environment.SpecialFolder.System);
                netsh.RedirectStandardOutput = true;
                netsh.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                netsh.CreateNoWindow = true;
                netsh.UseShellExecute = false;
                var proc = new Process();
                proc.StartInfo = netsh;
                proc.Start();
                var text=proc.StandardOutput.ReadToEnd();
                text=text.Substring(text.IndexOf("Signal")+25,4);
                return text.Replace("\n","");


            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            return "N/A";
        }
    }
}
