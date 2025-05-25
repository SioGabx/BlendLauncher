using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BlendLauncher
{
    public static class BlendStarter
    {
        public static void Launch(string Path)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = Path,
                    Arguments = string.Join(" ", Environment.GetCommandLineArgs().Skip(1).Select(arg => $"\"{arg}\"")),
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            Application.Current.Shutdown();
        }
    }
}
