using PicoShelter_DesktopApp.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace PicoShelter_DesktopApp.Pages
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        private ApplicationViewModel owner;
        public ApplicationViewModel Owner
        {
            get => owner;
            set
            {
                owner = value;
                ViewModel.Owner = value;
            }
        }

        public SettingsViewModel ViewModel => this.DataContext as SettingsViewModel;

        public SettingsPage()
        {
            InitializeComponent();
        }

        public SettingsPage(ApplicationViewModel owner) : this()
        {
            this.Owner = owner;
        }

        private void btnGoBack_Click(object sender, RoutedEventArgs e)
        {
            Owner.GoBack();
        }
    }
}
