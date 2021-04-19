using PicoShelter_DesktopApp.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PicoShelter_DesktopApp.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        public SettingsViewModel()
        {
            InstallCommand = new RelayCommand<object>(InstallCallback);
        }

        public SettingsViewModel(ApplicationViewModel owner) : this()
        {
            this.Owner = owner;
        }

        private ApplicationViewModel owner { get; set; }
        public ApplicationViewModel Owner
        {
            get => owner;
            set
            {
                owner = value;
                OnPropertyChanged();
            }
        }

        public ICommand InstallCommand { get; private set; }

        public static string SystemInstallPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "PicoShelter-DesktopApp.exe");
        public void InstallCallback(object param)
        {
            string thisInstancePath = Assembly.GetExecutingAssembly().Location;
            File.Copy(thisInstancePath, SystemInstallPath, true);
        }
    }
}
