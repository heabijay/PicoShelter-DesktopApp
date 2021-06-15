using System.Diagnostics;

namespace PicoShelter_DesktopApp
{
    public static partial class Statics
    {
        public static class WindowsLaunchManager
        {
            public static void OpenUrlInBrowser(string command)
            {
                Process.Start(new ProcessStartInfo("cmd", $"/c start {command.Replace("&", "^&")}") { CreateNoWindow = true });
            }

            public static void OpenFile(string path)
            {
                Process.Start(new ProcessStartInfo("cmd", $"/c \"\"{path}\"\"") { CreateNoWindow = true });
            }
        }
    }
}
