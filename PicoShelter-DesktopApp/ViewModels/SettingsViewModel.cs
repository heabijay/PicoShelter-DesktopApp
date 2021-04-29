using Microsoft.Win32;
using PicoShelter_DesktopApp.Commands;
using PicoShelter_DesktopApp.Pages;
using PicoShelter_DesktopApp.Services.AppSettings;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public string SystemInstallPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "PicoShelter-DesktopApp.exe");
        public string CurrentAssemblyPath => Process.GetCurrentProcess().MainModule.FileName;
        public string ExecutingVersion => FileVersionInfo.GetVersionInfo(CurrentAssemblyPath).FileVersion;

        public bool IsInstalled => File.Exists(SystemInstallPath) && FileVersionInfo.GetVersionInfo(SystemInstallPath).ProductName == "PicoShelter-DesktopApp";

        public string InstalledVersion
        {
            get
            {
                if (IsInstalled)
                    return FileVersionInfo.GetVersionInfo(SystemInstallPath).FileVersion;

                return null;
            }
        }

        public bool NeedUpdate
        {
            get
            {
                if (!IsInstalled)
                    return true;

                var info = FileVersionInfo.GetVersionInfo(SystemInstallPath);
                var current = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);

                if (info.FileMajorPart >= current.FileMajorPart)
                    if (info.FileMinorPart >= current.FileMinorPart)
                        if (info.FileBuildPart >= current.FileBuildPart)
                            if (info.FilePrivatePart >= current.FilePrivatePart)
                                return false;
                return true;
            }
        }

        public bool IntegrateToContextMenu
        {
            get
            {
                try
                {
                    bool ContextAllBinding = true;
                    RegistryKey RegSysFileAssoc = Registry.ClassesRoot.OpenSubKey("SystemFileAssociations");
                    foreach (string ext in MainViewModel.ImageExtensions)
                    {
                        RegistryKey key = RegSysFileAssoc?.OpenSubKey(ext)?.OpenSubKey("shell")?.OpenSubKey("PicoShelterUploader");
                        ContextAllBinding &= key != null;
                    }

                    return IsInstalled && ContextAllBinding;
                }
                catch
                {
                    return false;
                }
            }
            set
            {
                try
                {
                    if (value)
                    {
                        RegistryKey RegSysFileAssoc = Registry.ClassesRoot.CreateSubKey("SystemFileAssociations");
                        foreach (string ext in MainViewModel.ImageExtensions)
                        {
                            RegistryKey key = RegSysFileAssoc.CreateSubKey(ext)?.CreateSubKey("shell")?.CreateSubKey("PicoShelterUploader");
                            key?.SetValue("", "Upload via PicoShelter");
                            key?.SetValue("Icon", SystemInstallPath);
                            key?.CreateSubKey("command")?.SetValue("", '"' + SystemInstallPath + "\" \"%1\"");
                        }
                    }
                    else
                    {
                        RegistryKey RegSysFileAssoc = Registry.ClassesRoot.CreateSubKey("SystemFileAssociations");
                        foreach (string ext in MainViewModel.ImageExtensions)
                        {
                            RegistryKey key = RegSysFileAssoc.OpenSubKey(ext)?.OpenSubKey("shell", true);
                            if (key?.GetSubKeyNames().Any(t => t == "PicoShelterUploader") ?? false)
                                key.DeleteSubKeyTree("PicoShelterUploader");
                        }
                    }
                }
                catch { }
            }
        }

        public bool IntegrateToStartMenu
        {
            get
            {
                string StartShortcutPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Microsoft", "Windows", "Start Menu", "Programs", "PicoShelter Uploader.lnk");
                return IsInstalled && File.Exists(StartShortcutPath);
            }
            set
            {
                try
                {
                    string StartShortcutPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Microsoft", "Windows", "Start Menu", "Programs", "PicoShelter Uploader.lnk");
                    if (value)
                    {
                        if (!File.Exists(StartShortcutPath))
                        {
                            object shStartMenu = "AllUsersPrograms";
                            IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();
                            string shortcutAddress = (string)shell.SpecialFolders.Item(ref shStartMenu) + @"\PicoShelter Uploader.lnk";
                            IWshRuntimeLibrary.IWshShortcut shortcut = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(shortcutAddress);
                            shortcut.TargetPath = SystemInstallPath;
                            shortcut.Save();
                        }
                    }
                    else
                    {
                        if (File.Exists(StartShortcutPath))
                            File.Delete(StartShortcutPath);
                    }
                }
                catch { }
            }
        }

        public void InstallCallback(object param)
        {
            try
            {
                File.Copy(CurrentAssemblyPath, SystemInstallPath, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong: " + ex.Message);
            }
            finally
            {
                OnPropertyChanged(nameof(IsInstalled));
                OnPropertyChanged(nameof(InstalledVersion));
                OnPropertyChanged(nameof(NeedUpdate));
                OnPropertyChanged(nameof(IntegrateToContextMenu));
                OnPropertyChanged(nameof(IntegrateToStartMenu));
            }
        }
    }
}
