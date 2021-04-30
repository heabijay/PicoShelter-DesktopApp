using PicoShelter_DesktopApp.Commands;
using PicoShelter_DesktopApp.DTOs;
using PicoShelter_DesktopApp.Exceptions;
using PicoShelter_DesktopApp.Infrastructure;
using PicoShelter_DesktopApp.Pages;
using PicoShelter_DesktopApp.Services;
using PicoShelter_DesktopApp.Services.AppSettings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
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
            SignInCommand = new AsyncRelayCommand(SignInCallback, SignInException, () => !string.IsNullOrWhiteSpace(Login) && SecurePassword?.Length > 0 );

            SignInSessionCommand = new AsyncRelayCommand(SignInSessionCallback, SignInException);
        }

        public LoginViewModel(ApplicationViewModel owner) : this()
        {
            this.Owner = owner;
        }

        private ApplicationViewModel owner { get; set; }
        public ApplicationViewModel Owner
        {
            get => owner;
            set
            {
                owner = value;
                OnPropertyChanged();
            }
        }

        private string login = "";
        public string Login
        {
            get => login;
            set
            {
                login = value;
                OnPropertyChanged();
            }
        }

        private SecureString securePassword;
        public SecureString SecurePassword
        {
            get => securePassword;
            set
            {
                securePassword = value;
                OnPropertyChanged();
            }
        }


        private bool isLoading = false;
        public bool IsLoading
        {
            get => isLoading;
            set
            {
                isLoading = value;
                OnPropertyChanged();
            }
        }

        private bool invalidCredentialsWarn = false;
        public bool InvalidCredentialsWarn
        {
            get => invalidCredentialsWarn;
            set
            {
                invalidCredentialsWarn = value;
                OnPropertyChanged();
            }
        }

        public ICommand SignInCommand { get; private set; }
        public ICommand SignInSessionCommand { get; private set; }

        private async Task SignInCallback()
        {
            this.IsLoading = true;

            LoginResponseDto response;

            var isEmail = Regex.IsMatch(login, @"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$");
            if (isEmail)
            {
                response = await HttpService.Current.LoginByEmailAsync(login, new System.Net.NetworkCredential(string.Empty, SecurePassword).Password);
            }
            else
            {
                response = await HttpService.Current.LoginAsync(login, new System.Net.NetworkCredential(string.Empty, SecurePassword).Password);
            }

            this.IsLoading = false;

            var settings = await AppSettingsProvider.ProvideAsync();
            settings.AccessToken = response.access_token;

            Owner.CurrentUser = response.user;
            Owner.GoMain();
        }

        private async Task SignInSessionCallback()
        {
            this.IsLoading = true;

            AccountInfoDto response = await HttpService.Current.GetCurrentUserAsync();

            this.IsLoading = false;

            Owner.CurrentUser = response;
            Owner.GoMain();
        }

        private void SignInException(Exception e)
        {
            {
                this.IsLoading = false;

                if (e is HttpResponseException ex)
                {
                    if (ex.Details?.type != null)
                    {
                        var exType = Enum.Parse<ExceptionType>(ex.Details.type);

                        switch (exType)
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
