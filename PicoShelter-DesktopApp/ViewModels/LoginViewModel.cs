using PicoShelter_DesktopApp.Commands;
using PicoShelter_DesktopApp.DTOs;
using PicoShelter_DesktopApp.Exceptions;
using PicoShelter_DesktopApp.Infrastructure;
using PicoShelter_DesktopApp.Services;
using PicoShelter_DesktopApp.Services.AppSettings;
using System;
using System.Net.Sockets;
using System.Security;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PicoShelter_DesktopApp.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        public LoginViewModel()
        {
            SignInCommand = new AsyncRelayCommand(SignInCallback, SignInException, () => !string.IsNullOrWhiteSpace(Login) && SecurePassword?.Length > 0);

            SignInSessionCommand = new AsyncRelayCommand(SignInSessionCallback, SignInException);
        }

        public LoginViewModel(ApplicationViewModel owner) : this()
        {
            this.Owner = owner;
        }

        private ApplicationViewModel _owner { get; set; }
        public ApplicationViewModel Owner
        {
            get => _owner;
            set
            {
                _owner = value;
                OnPropertyChanged();
            }
        }

        private string _login = "";
        public string Login
        {
            get => _login;
            set
            {
                _login = value;
                OnPropertyChanged();
            }
        }

        private SecureString _securePassword;
        public SecureString SecurePassword
        {
            get => _securePassword;
            set
            {
                _securePassword = value;
                OnPropertyChanged();
            }
        }


        private bool _isLoading = false;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }

        private bool _invalidCredentialsWarn = false;
        public bool InvalidCredentialsWarn
        {
            get => _invalidCredentialsWarn;
            set
            {
                _invalidCredentialsWarn = value;
                OnPropertyChanged();
            }
        }

        public ICommand SignInCommand { get; private set; }
        public ICommand SignInSessionCommand { get; private set; }

        private async Task SignInCallback()
        {
            IsLoading = true;

            LoginResponseDto response;

            var isEmail = Regex.IsMatch(_login, @"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$");
            if (isEmail)
            {
                response = await HttpService.Current.LoginByEmailAsync(_login, new System.Net.NetworkCredential(string.Empty, SecurePassword).Password);
            }
            else
            {
                response = await HttpService.Current.LoginAsync(_login, new System.Net.NetworkCredential(string.Empty, SecurePassword).Password);
            }

            IsLoading = false;

            var settings = await AppSettingsProvider.ProvideAsync();
            settings.AccessToken = response.access_token;

            Owner.CurrentUser = response.user;
            Owner.GoMain();
        }

        private async Task SignInSessionCallback()
        {
            IsLoading = true;

            AccountInfoDto response = await HttpService.Current.GetCurrentUserAsync();

            IsLoading = false;

            Owner.CurrentUser = response;
            Owner.GoMain();
        }

        private void SignInException(Exception e)
        {
            {
                IsLoading = false;

                if (e is HttpResponseException ex)
                {
                    if (ex.Details?.type > ExceptionType.UNTYPED)
                    {
                        switch (ex.Details.type)
                        {
                            case ExceptionType.CREDENTIALS_INCORRECT:
                                InvalidCredentialsWarn = true;
                                return;
                        }
                    }

                    if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                        return;
                }
                else if (e?.InnerException is SocketException exSocket)
                {
                    if (exSocket.SocketErrorCode == SocketError.ConnectionRefused)
                    {
                        MessageBox.Show(ApplicationViewModel.LanguageResolve("LoginPage.ServerUnavailable").ToString());
                        App.Current.Shutdown();
                    }
                }

                MessageBox.Show(ApplicationViewModel.LanguageResolve("Shared.SomethingWentWrong") + ": " + e?.Message);
            }
        }
    }
}
