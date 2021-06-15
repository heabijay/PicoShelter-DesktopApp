using PicoShelter_DesktopApp.Services.AppSettings;
using System.Windows;
using System.Windows.Controls;

namespace PicoShelter_DesktopApp.Pages
{
    /// <summary>
    /// Interaction logic for WelcomePage.xaml
    /// </summary>
    public partial class WelcomePage : Page
    {
        private ApplicationViewModel owner;
        public ApplicationViewModel Owner
        {
            get => owner;
            set => owner = value;
        }

        public WelcomePage()
        {
            InitializeComponent();
        }

        public WelcomePage(ApplicationViewModel owner) : this()
        {
            this.Owner = owner;
        }

        private void btnContinueAnonymous_Click(object sender, RoutedEventArgs e)
        {
            AppSettingsProvider.Provide().AccessToken = "";
            Owner.GoMain();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            Owner.GoLogin();
        }
    }
}
