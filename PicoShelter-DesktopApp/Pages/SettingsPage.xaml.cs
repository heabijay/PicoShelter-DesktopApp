using PicoShelter_DesktopApp.ViewModels;
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

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            Owner.GoLogin();
        }
    }
}
