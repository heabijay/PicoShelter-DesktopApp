using PicoShelter_DesktopApp.Services.AppSettings.Enums;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PicoShelter_DesktopApp.Services.AppSettings
{
    public class AppSettings : INotifyPropertyChanged, IDataErrorInfo
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        private string _accessToken;
        public string AccessToken
        {
            get => _accessToken;
            set
            {
                _accessToken = value;
                OnPropertyChanged();
            }
        }

        private DefaultTitleOptions _defaultTitle { get; set; } = DefaultTitleOptions.SAME_AS_FILENAME;
        public DefaultTitleOptions DefaultTitle
        {
            get => _defaultTitle;
            set
            {
                _defaultTitle = value;
                OnPropertyChanged();
            }
        }

        private QualityOptions _defaultQuality { get; set; } = QualityOptions.GOOD;
        public QualityOptions DefaultQuality
        {
            get => _defaultQuality;
            set
            {
                _defaultQuality = value;
                OnPropertyChanged();
            }
        }

        private LifetimeOptions _defaultLifetime { get; set; } = LifetimeOptions.HOUR_1;
        public LifetimeOptions DefaultLifetime
        {
            get => _defaultLifetime;
            set
            {
                _defaultLifetime = value;
                OnPropertyChanged();
            }
        }

        private bool _defaultMakePublic { get; set; } = true;
        public bool DefaultMakePublic
        {
            get => _defaultMakePublic;
            set
            {
                _defaultMakePublic = value;
                OnPropertyChanged();
            }
        }

        private LocaleOptions _locale { get; set; } = LocaleOptions.en_US;
        public LocaleOptions Locale
        {
            get => _locale;
            set
            {
                _locale = value;
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
