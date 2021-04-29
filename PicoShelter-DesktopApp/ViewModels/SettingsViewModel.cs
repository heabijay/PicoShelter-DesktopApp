using PicoShelter_DesktopApp.Commands;
using PicoShelter_DesktopApp.Pages;
using PicoShelter_DesktopApp.Services.AppSettings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PicoShelter_DesktopApp.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        public SettingsViewModel()
        {
            InstallCommand = new RelayCommand(InstallCallback);
            LogoutCommand = new RelayCommand(obj =>
            {
                if (MessageBox.Show("Are you sure?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    AppSettingsProvider.Provide().AccessToken = null;
                    Owner.CurrentUser = null;
                    Owner.CurrentPage = new WelcomePage(Owner);
                }
            });

            settings = AppSettingsProvider.Provide();
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
        public ICommand LogoutCommand { get; private set; }

        private AppSettings settings { get; set; }
        public AppSettings Settings
        {
            get => settings;
            set
            {
                settings = value;
                OnPropertyChanged();
            }
        }

        public static string SystemInstallPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "PicoShelter-DesktopApp.exe");
        public void InstallCallback(object param)
        {
            string thisInstancePath = Assembly.GetExecutingAssembly().Location;
            File.Copy(thisInstancePath, SystemInstallPath, true);
        }
    }
}
