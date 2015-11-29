using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MerulaShellUi.start
{
    class Search
    {
        public static string[] GetSearchResults(string search)
        {
            Process.Start("Everything.exe");
            var proc = new Process();
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.FileName = "es.exe";
            proc.StartInfo.Arguments = "-w"; //dummy argument that gets full results
            proc.Start();
            var output = proc.StandardOutput.ReadToEnd();
            proc.WaitForExit();
            return output.Split(Convert.ToChar("\n"));
        }
    }
}
