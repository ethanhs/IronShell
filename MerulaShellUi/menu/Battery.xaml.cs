using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
using System.Management;

namespace MerulaShellUi.menu
{
    /// <summary>
    /// Interaction logic for Battery.xaml
    /// </summary>
    public partial class Battery : UserControl
    {
        public Battery()
        {
            InitializeComponent();
            //inital text
            var settings = SharedSettings.GetInstance();
            ucBattery.Foreground= settings.UiForeBrush;
            ucBattery.Text = "Battery Remaining: " + GetBatteryPercent()+"%";
            //set up timer
            var timer = new Timer(60000);
            timer.Elapsed += Timer_Elapsed;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke((Action)(() => //assures that we can update out of thread?
            {
                ucBattery.Text = "Battery Remaining: " + GetBatteryPercent() + "%";
            }));
            
        }

        private string GetBatteryPercent()
        {
            try
            {
                var scope = new ManagementScope();
                SelectQuery query = new SelectQuery("Select EstimatedChargeRemaining From Win32_Battery");
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query))
                {
                    using (ManagementObjectCollection objectCollection = searcher.Get())
                    {
                        foreach (ManagementObject mObj in objectCollection)
                            //this nugget has a lot of info see here: https://msdn.microsoft.com/en-us/library/windows/desktop/aa394074%28v=vs.85%29.aspx
                        {
                            PropertyData pData = mObj.Properties["EstimatedChargeRemaining"];
                            var val = pData.Value;
                            if ((ushort) val > (ushort) 100)
                            {
                                return "100";
                            }
                            else
                            {
                                return val.ToString().Replace("%",""); //strip % in case it duplicates
                            }

                        }
                    }
                }
                return "Unk";
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return "Unk";
            }
        }
    }
}
