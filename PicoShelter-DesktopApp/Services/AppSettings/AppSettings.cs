using PicoShelter_DesktopApp.Services.AppSettings.Enums;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace PicoShelter_DesktopApp.Services.AppSettings
{
    public class AppSettings : INotifyPropertyChanged
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
    }
}
