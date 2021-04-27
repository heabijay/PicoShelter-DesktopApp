using PicoShelter_DesktopApp.DTOs;
using PicoShelter_DesktopApp.Services.AppSettings.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Media.Imaging;

namespace PicoShelter_DesktopApp.Models
{
    public class UploadTask : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        private string filepath { get; set; }
        public string Filepath
        {
            get => filepath;
            set
            {
                filepath = value;
                OnPropertyChanged();

                Filename = Path.GetFileName(Filepath);

                var bitmap = new BitmapImage();
                var stream = File.OpenRead(Filepath);
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.DecodePixelWidth = 50;
                bitmap.StreamSource = stream;
                bitmap.EndInit();
                stream.Close();
                stream.Dispose();

                BitmapImage = bitmap;
            }
        }

        private string filename { get; set; }
        public string Filename
        {
            get => filename;
            private set
            {
                filename = value;
                OnPropertyChanged();
            }
        }

        private BitmapImage bitmapImage { get; set; }
        public BitmapImage BitmapImage
        {
            get => bitmapImage;
            private set
            {
                bitmapImage = value;
                OnPropertyChanged();
            }
        }

        private QualityOptions uploadQuality { get; set; }
        public QualityOptions UploadQuality
        {
            get => uploadQuality;
            set
            {
                uploadQuality = value;
                OnPropertyChanged();
            }
        }

        private LifetimeOptions uploadLifetime { get; set; }
        public LifetimeOptions UploadLifetime
        {
            get => uploadLifetime;
            set
            {
                uploadLifetime = value;
                OnPropertyChanged();
            }
        }

        private bool makePublic { get ; set; }
        public bool MakePublic
        {
            get => makePublic;
            set
            {
                makePublic = value;
                OnPropertyChanged();
            }
        }

        private ImageInfoDto uploadInfo { get; set; }
        public ImageInfoDto UploadInfo
        {
            get => uploadInfo;
            set
            {
                uploadInfo = value;
                OnPropertyChanged();
            }
        }

        public bool IsUploaded => uploadInfo != null;

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

        private bool copyPopupIsOpen { get; set; }
        public bool CopyPopupIsOpen
        {
            get => copyPopupIsOpen;
            set
            {
                copyPopupIsOpen = value;
                OnPropertyChanged();
            }
        }


        public UploadTask() 
        {

        }

        public UploadTask(string filepath) : this()
        {
            Filepath = filepath;
        }
    }
}
