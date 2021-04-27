﻿using Microsoft.Win32;
using PicoShelter_DesktopApp.Commands;
using PicoShelter_DesktopApp.Models;
using PicoShelter_DesktopApp.Pages;
using PicoShelter_DesktopApp.Services.AppSettings;
using PicoShelter_DesktopApp.Services.AppSettings.Enums;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

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
            }, obj => UploadTasks.Count > 0);

            AddUploadTasks(@"C:\Users\heabi\Downloads\Telegram Desktop\@iWallpaper (6).jpg");
        }

        public MainViewModel(ApplicationViewModel owner) : this()
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

        public static readonly string[] ImageExtensions = new string[] { ".bmp", ".gif", ".jpg", ".jpeg", ".png" };

        private ObservableCollection<UploadTask> uploadTasks = new ObservableCollection<UploadTask>();
        public ObservableCollection<UploadTask> UploadTasks
        {
            get => uploadTasks;
            set
            {
                uploadTasks = value;
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
                DefaultTitleOptions.SAME_AS_FILENAME => Path.GetFileName(filepath),
                DefaultTitleOptions.EMPTY => "",
                _ => null,
            };
        }

        public void AddUploadTasks(params string[] filepaths)
        {
            int unsupportedCount = 0;
            foreach (var path in filepaths)
            {
                try
                {
                    if (!ImageExtensions.Contains(Path.GetExtension(path).ToLower()))
                        throw new NotSupportedException();

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
            }

            if (unsupportedCount > 0)
                MessageBox.Show($"{unsupportedCount} files weren't added due to the unsupported type!");
        }

        public ICommand AddCommand { get; private set; }
        public ICommand RemoveCommand { get; private set; }
        public ICommand RemoveAllCommand { get; private set; }


        private bool isUploading { get; set; }
        public bool IsUploading
        {
            get => isUploading;
            set
            {
                isUploading = value;
                OnPropertyChanged();
            }
        }
    }
}
