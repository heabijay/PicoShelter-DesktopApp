using PicoShelter_DesktopApp.Models;
using PicoShelter_DesktopApp.Services;
using System;
using System.Text.Json;
using System.Threading;
using System.Windows;

namespace PicoShelter_DesktopApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public const string MutName = "PicoShelter-DesktopApp";
        public NamedPipeService PipeService { get; set; }
        private Mutex _mutex { get; set; }
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            _mutex = new Mutex(true, MutName, out bool CreatedNew);
            PipeService = new NamedPipeService(MutName);

            if (CreatedNew)
            {
                PipeService.StartServer();
            }
            else
            {
                string[] args = Environment.GetCommandLineArgs();
                PipeCommand command = new PipeCommand { Type = PipeCommand.CommandType.OpenSecondInstance, CommandLineArgs = args };
                string msg = JsonSerializer.Serialize(command);
                PipeService.ClientWrite(msg);
                Environment.Exit(0);
            }
        }
    }
}
