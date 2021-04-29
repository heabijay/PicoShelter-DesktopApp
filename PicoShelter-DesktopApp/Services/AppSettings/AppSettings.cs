using PicoShelter_DesktopApp.Services.AppSettings.Enums;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace PicoShelter_DesktopApp.Services.AppSettings
{
    public class AppSettings : INotifyPropertyChanged, IDataErrorInfo
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        private string accessToken;
        public string AccessToken
        {
            get => accessToken;
            set
            {
                accessToken = value;
                OnPropertyChanged();
            }
        }

        private DefaultTitleOptions defaultTitle { get; set; } = DefaultTitleOptions.EMPTY;
        public DefaultTitleOptions DefaultTitle
        {
            get => defaultTitle;
            set
            {
                defaultTitle = value;
                OnPropertyChanged();
            }
        }

        private QualityOptions defaultQuality { get; set; } = QualityOptions.ORIGINAL;
        public QualityOptions DefaultQuality
        {
            get => defaultQuality;
            set
            {
                defaultQuality = value;
                OnPropertyChanged();
            }
        }

        private LifetimeOptions defaultLifetime { get; set; } = LifetimeOptions.LIFETIME;
        public LifetimeOptions DefaultLifetime
        {
            get => defaultLifetime;
            set
            {
                defaultLifetime = value;
                OnPropertyChanged();
            }
        }

        private bool defaultMakePublic { get; set; } = true;
        public bool DefaultMakePublic
        {
            get => defaultMakePublic;
            set
            {
                defaultMakePublic = value;
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
                    case nameof(DefaultLifetime):
                        if (isAnonymous && DefaultLifetime == LifetimeOptions.LIFETIME)
                            return "Lifetime forbidden for anonymous upload";
                        break;
                    case nameof(DefaultQuality):
                        if (isAnonymous && DefaultQuality == QualityOptions.ORIGINAL)
                            return "Original quality forbidden for anonymous upload";
                        break;
                    case nameof(DefaultMakePublic):
                        if (isAnonymous && DefaultMakePublic == false)
                            return "Private accessforbidden for anonymous upload";
                        break;
                }

                return null;
            }
        }
    }
}
