using PicoShelter_DesktopApp.Services.AppSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
