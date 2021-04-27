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
                BitmapImage = new BitmapImage(new Uri(Filepath)) { DecodePixelWidth = 50 };
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

        private string uploadedUrl { get; set; }
        public string UploadedUrl
        {
            get => uploadedUrl;
            set
            {
                uploadedUrl = value;
                OnPropertyChanged();
                OnPropertyChanged("IsUploaded");
            }
        }

        public bool IsUploaded => !string.IsNullOrEmpty(UploadedUrl);

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


        public UploadTask() { }

        public UploadTask(string filepath)
        {
            Filepath = filepath;
        }
    }
}
