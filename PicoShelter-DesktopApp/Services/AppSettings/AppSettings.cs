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
    }
}
