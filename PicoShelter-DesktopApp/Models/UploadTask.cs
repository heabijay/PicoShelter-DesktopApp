using PicoShelter_DesktopApp.DTOs;
using PicoShelter_DesktopApp.Services.AppSettings.Enums;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;

namespace PicoShelter_DesktopApp.Models
{
    public class UploadTask : INotifyPropertyChanged, IDataErrorInfo
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        private string _filepath { get; set; }
        public string Filepath
        {
            get => _filepath;
            set
            {
                _filepath = value;
                OnPropertyChanged();

                Filename = Path.GetFileName(Filepath);

                var bitmap = new BitmapImage();
                var stream = File.OpenRead(Filepath);
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.DecodePixelWidth = 70;
                bitmap.StreamSource = stream;
                bitmap.EndInit();
                stream.Close();
                stream.Dispose();

                BitmapImage = bitmap;
            }
        }

        private string _filename { get; set; }
        public string Filename
        {
            get => _filename;
            private set
            {
                _filename = value;
                OnPropertyChanged();
            }
        }

        private BitmapImage _bitmapImage { get; set; }
        public BitmapImage BitmapImage
        {
            get => _bitmapImage;
            private set
            {
                _bitmapImage = value;
                OnPropertyChanged();
            }
        }

        private QualityOptions _uploadQuality { get; set; }
        public QualityOptions UploadQuality
        {
            get => _uploadQuality;
            set
            {
                _uploadQuality = value;
                OnPropertyChanged();
            }
        }

        private LifetimeOptions _uploadLifetime { get; set; }
        public LifetimeOptions UploadLifetime
        {
            get => _uploadLifetime;
            set
            {
                _uploadLifetime = value;
                OnPropertyChanged();
            }
        }

        private bool _makePublic { get; set; }
        public bool MakePublic
        {
            get => _makePublic;
            set
            {
                _makePublic = value;
                OnPropertyChanged();
            }
        }

        private ImageInfoDto _uploadInfo { get; set; }
        public ImageInfoDto UploadInfo
        {
            get => _uploadInfo;
            set
            {
                _uploadInfo = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsUploaded));
                OnPropertyChanged(nameof(UploadUrl));
                OnPropertyChanged(nameof(UploadDirectUrl));
            }
        }

        public string UploadUrl => ServerRouting.WebAppRouting.ImageUrlEndpoint + _uploadInfo?.imageCode;
        public string UploadDirectUrl => ServerRouting.ImageUrlEndpoint + _uploadInfo?.imageCode + '.' + UploadInfo?.imageType?.Replace("jpeg", "jpg");

        public bool IsUploaded => _uploadInfo != null;

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

        private bool _copyLinkPopupIsOpen { get; set; }
        public bool CopyLinkPopupIsOpen
        {
            get => _copyLinkPopupIsOpen;
            set
            {
                _copyLinkPopupIsOpen = value;
                OnPropertyChanged();
            }
        }


        public string Error => throw new NotImplementedException();

        public string this[string columnName]
        {
            get
            {
                var isAnonymous = (App.Current.MainWindow as MainWindow)?.ViewModel?.IsCurrentUserAnonymous ?? true;
                switch (columnName)
                {
                    case nameof(UploadLifetime):
                        if (isAnonymous && UploadLifetime == LifetimeOptions.LIFETIME)
                            return "Lifetime forbidden for anonymous upload";
                        break;
                    case nameof(UploadQuality):
                        if (isAnonymous && UploadQuality == QualityOptions.ORIGINAL)
                            return "Original quality forbidden for anonymous upload";
                        break;
                    case nameof(MakePublic):
                        if (isAnonymous && MakePublic == false)
                            return "Private accessforbidden for anonymous upload";
                        break;
                }

                return null;
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
