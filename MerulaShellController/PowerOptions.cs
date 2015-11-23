using System.Diagnostics;

namespace MerulaShellController
{
    /// <summary>
    /// Powerdown options for the computer
    /// shutdown, hibernate and restart
    /// </summary>
    public static class PowerOptions
    {
        public static void Shutdown()
        {
            Process.Start(new ProcessStartInfo("shutdown.exe", "-s -t 0") { CreateNoWindow = true, WindowStyle = ProcessWindowStyle.Hidden });
        }
        public static void Restart()
        {
            Process.Start(new ProcessStartInfo("shutdown.exe", "-r -t 0") { CreateNoWindow = true, WindowStyle = ProcessWindowStyle.Hidden });
        }
        public static void Hibernate()
        {
            Process.Start(new ProcessStartInfo("shutdown.exe", "-h") { CreateNoWindow = true, WindowStyle = ProcessWindowStyle.Hidden });
        }
    }
}
