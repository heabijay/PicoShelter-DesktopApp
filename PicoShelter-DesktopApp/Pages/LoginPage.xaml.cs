using PicoShelter_DesktopApp.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
            set
            {
                owner = value;
                ViewModel.Owner = value;
            }
        }

        public LoginViewModel ViewModel => this.DataContext as LoginViewModel;


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
            Owner.GoBack();
        }

        private void btnGoResetPassword_Click(object sender, RoutedEventArgs e)
        {
            Statics.WindowsLaunchManager.OpenUrlInBrowser(ServerRouting.WebAppRouting.ResetPasswordUrl);
        }

        private void tbPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ViewModel.SecurePassword = (sender as PasswordBox)?.SecurePassword;

            CredentialsChanged(sender, e);
        }

        private void tbPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (ViewModel.SignInCommand.CanExecute(null))
                {
                    ViewModel.SignInCommand.Execute(null);
                    Keyboard.ClearFocus();
                }
            }
        }

        private void tbLogin_TextChanged(object sender, TextChangedEventArgs e)
        {
            CredentialsChanged(sender, e);
        }

        private void CredentialsChanged(object sender, EventArgs e)
        {
            if (ViewModel.InvalidCredentialsWarn)
                ViewModel.InvalidCredentialsWarn = false;
        }
    }
}
