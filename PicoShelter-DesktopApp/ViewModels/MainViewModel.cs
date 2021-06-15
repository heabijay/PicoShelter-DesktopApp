using Microsoft.Win32;
using PicoShelter_DesktopApp.Commands;
using PicoShelter_DesktopApp.Exceptions;
using PicoShelter_DesktopApp.Models;
using PicoShelter_DesktopApp.Services;
using PicoShelter_DesktopApp.Services.AppSettings;
using PicoShelter_DesktopApp.Services.AppSettings.Enums;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static PicoShelter_DesktopApp.Statics;

namespace PicoShelter_DesktopApp.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            AddCommand = new RelayCommand(obj =>
            {
                OpenFileDialog dialog = new OpenFileDialog()
                {
                    Multiselect = true,
                    Filter = "Pictures (*.bmp;*.gif;*.jpg;*.jpeg;*.png)|*.bmp;*.gif;*.jpg;*.jpeg;*.png"
                };

                if (dialog.ShowDialog() == true)
                    AddUploadTasks(dialog.FileNames);
            });

            RemoveCommand = new RelayCommand<UploadTask>(task =>
            {
                UploadTasks.Remove(task);
            }, obj => !IsUploading);

            RemoveAllCommand = new RelayCommand(obj =>
            {
                UploadTasks.Clear();
            }, obj => !IsUploading && UploadTasks.Count > 0);

            UploadCommand = new AsyncRelayCommand(
                UploadCallback,
                UploadException,
                () => !IsUploading &&
                    UploadTasks.Any(t => !t.IsUploaded) &&
                    !UploadTasks.Any(t => !t.IsUploaded && (t[nameof(t.UploadQuality)] != null) || (t[nameof(t.MakePublic)] != null) || (t[nameof(t.UploadLifetime)] != null)));

            OpenLinkCommand = new RelayCommand<string>(link =>
            {
                WindowsLaunchManager.OpenUrlInBrowser(link);
            });

            CopyLinkCommand = new RelayCommand<UploadTask>(async t =>
            {
                Clipboard.SetText(t.UploadUrl);
                t.CopyLinkPopupIsOpen = true;
                await Task.Delay(1000);
                t.CopyLinkPopupIsOpen = false;
            });

            CopyDirectLinkCommand = new RelayCommand<UploadTask>(async t =>
            {
                Clipboard.SetText(t.UploadDirectUrl);
                t.CopyLinkPopupIsOpen = true;
                await Task.Delay(1000);
                t.CopyLinkPopupIsOpen = false;
            });

            SetDefaultOptionsCommand = new RelayCommand(obj =>
            {
                int i = 0;
                var settings = AppSettingsProvider.Provide();
                while (i < UploadTasks.Count)
                {
                    var t = UploadTasks[i];
                    if (!t.IsUploaded && !t.IsUploading)
                    {
                        t.MakePublic = settings.DefaultMakePublic;
                        t.UploadQuality = settings.DefaultQuality;
                        t.UploadLifetime = settings.DefaultLifetime;
                    }

                    i++;
                }
            }, obj => UploadTasks.Any(t => !t.IsUploaded && !t.IsUploading));

            // AddUploadTasks(@"C:\Users\heabi\Downloads\Telegram Desktop\@iWallpaper (6).jpg");
        }

        public MainViewModel(ApplicationViewModel owner) : this()
        {
            Owner = owner;
        }

        private ApplicationViewModel _owner { get; set; }
        public ApplicationViewModel Owner
        {
            get => _owner;
            set
            {
                _owner = value;
                OnPropertyChanged();
            }
        }

        public static readonly string[] ImageExtensions = new string[] { ".bmp", ".gif", ".jpg", ".jpeg", ".png" };

        private ObservableCollection<UploadTask> _uploadTasks = new ObservableCollection<UploadTask>();
        public ObservableCollection<UploadTask> UploadTasks
        {
            get => _uploadTasks;
            set
            {
                _uploadTasks = value;
                OnPropertyChanged();
            }
        }

        public void AddUploadTasks(params UploadTask[] tasks)
        {
            foreach (var task in tasks)
            {
                if (!UploadTasks.Any(t => t.Filepath == task.Filepath))
                    UploadTasks.Add(task);
            }
        }

        public string GetDefaultTitle(string filepath)
        {
            return AppSettingsProvider.Provide().DefaultTitle switch
            {
                DefaultTitleOptions.SAME_AS_FILENAME => Path.GetFileName(filepath).Length > 64 ? Path.GetFileName(filepath)[0..64] : Path.GetFileName(filepath),
                DefaultTitleOptions.EMPTY => "",
                _ => null,
            };
        }

        public void AddUploadTasks(params string[] filepaths)
        {
            int unsupportedCount = 0;
            int sizeOverflowCount = 0;
            foreach (var path in filepaths)
            {
                try
                {
                    if (!ImageExtensions.Contains(Path.GetExtension(path).ToLower()))
                        throw new NotSupportedException();

                    if (new FileInfo(path).Length > 10 * 1024 * 1024)
                        throw new OverflowException();

                    var task = new UploadTask(path)
                    {
                        UploadQuality = AppSettingsProvider.Provide().DefaultQuality,
                        UploadLifetime = AppSettingsProvider.Provide().DefaultLifetime,
                        MakePublic = AppSettingsProvider.Provide().DefaultMakePublic
                    };

                    AddUploadTasks(task);
                }
                catch (NotSupportedException)
                {
                    unsupportedCount++;
                }
                catch (OverflowException)
                {
                    sizeOverflowCount++;
                }
            }

            if (unsupportedCount > 0)
                MessageBox.Show($"{unsupportedCount} {ApplicationViewModel.LanguageResolve("MainPage.FilesWasntAddedDueUnsupported")}");

            if (sizeOverflowCount > 0)
                MessageBox.Show($"{sizeOverflowCount} {ApplicationViewModel.LanguageResolve("MainPage.FilesWasntAddedDueSize")}");
        }

        public ICommand AddCommand { get; private set; }
        public ICommand RemoveCommand { get; private set; }
        public ICommand RemoveAllCommand { get; private set; }
        public ICommand SetDefaultOptionsCommand { get; private set; }
        public ICommand UploadCommand { get; private set; }
        public ICommand OpenLinkCommand { get; private set; }
        public ICommand CopyLinkCommand { get; private set; }
        public ICommand CopyDirectLinkCommand { get; private set; }

        public async Task UploadCallback()
        {
            IsUploading = true;

            int i = 0;
            while (i < UploadTasks.Count)
            {
                var t = UploadTasks[i];
                if (!t.IsUploaded)
                {
                    t.IsUploading = true;
                    Stream fs = null;
                    try
                    {
                        fs = File.OpenRead(t.Filepath);
                        var r = await HttpService.Current.UploadImageAsync(
                            GetDefaultTitle(t.Filepath),
                            (int)t.UploadLifetime,
                            t.MakePublic,
                            (int)t.UploadQuality,
                            fs
                            );

                        t.UploadInfo = r;
                    }
                    catch (HttpResponseException ex)
                    {
                        MessageBox.Show($"{ApplicationViewModel.LanguageResolve("Shared.SomethingWentWrong")}: {ex.Message}");
                    }
                    catch (HttpRequestException ex) when (ex?.InnerException is SocketException exSocket)
                    {
                        if (exSocket.SocketErrorCode == SocketError.ConnectionRefused)
                        {
                            MessageBox.Show(ApplicationViewModel.LanguageResolve("MainPage.ServerUnavaliable").ToString());
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"{ApplicationViewModel.LanguageResolve("Shared.SomethingWentWrong")} [{t.Filename} ({t.Filepath})]: {ex.Message}");
                    }
                    finally
                    {
                        fs?.Close();
                        fs?.Dispose();

                        t.IsUploading = false;
                    }
                }

                i++;
            }

            IsUploading = false;
            CommandManager.InvalidateRequerySuggested();
        }

        public void UploadException(Exception ex)
        {
            IsUploading = false;
        }


        private bool _isUploading { get; set; }
        public bool IsUploading
        {
            get => _isUploading;
            set
            {
                _isUploading = value;
                OnPropertyChanged();
            }
        }
    }
}
