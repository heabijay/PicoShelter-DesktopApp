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
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        private ApplicationViewModel owner;
        public ApplicationViewModel Owner
        {
            get => owner;
            set => owner = value;
        }
        
        public LoginPage()
        {
            InitializeComponent();
        }

        public LoginPage(ApplicationViewModel owner) : this()
        {
            this.Owner = owner;
        }


        private void btnGoBack_Click(object sender, RoutedEventArgs e)
        {
            Owner.ToBack();
        }
    }
}
