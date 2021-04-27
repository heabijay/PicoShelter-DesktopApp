using PicoShelter_DesktopApp.DTOs;
using PicoShelter_DesktopApp.Pages;
using PicoShelter_DesktopApp.Services;
using PicoShelter_DesktopApp.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PicoShelter_DesktopApp
{
    public class ApplicationViewModel : ViewModelBase
    {
        public ApplicationViewModel()
        {
            mainPage = new MainPage(this);
            currentPage = new WelcomePage(this);

            currentPage = mainPage;
        }

        public ApplicationViewModel(MainWindow owner) : this()
        {
            this.Owner = owner;
        }

        private MainWindow owner { get; set; }
        public MainWindow Owner
        {
            get => owner;
            set
            {
                owner = value;
                OnPropertyChanged();
            }
        }

        private Page mainPage { get; set; }

        private Page currentPage { get; set; }
        public Page CurrentPage
        {
            get => currentPage;
            set
            {
                currentPage = value;
                OnPropertyChanged();
            }
        }

        private AccountInfoDto currentUser { get; set; }
        public AccountInfoDto CurrentUser
        {
            get => currentUser;
            set
            {
                currentUser = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CurrentUserString));
            }
        }

        public string CurrentUserString
        {
            get
            {
                if (CurrentUser == null)
                    return "Anonymous";

                string r = "";

                var isFirstnameExist = !string.IsNullOrEmpty(CurrentUser.profile.firstname);
                var isLastnameExist = !string.IsNullOrEmpty(CurrentUser.profile.lastname);

                if (isFirstnameExist)
                    r += CurrentUser.profile.firstname + ' ';

                if (isLastnameExist)
                    r += CurrentUser.profile.lastname + ' ';

                if (isFirstnameExist || isLastnameExist)
                    r += "(@" + CurrentUser.username + ')';
                else
                    r += CurrentUser.username;

                return r;
            }
        }

        public void GoBack()
        {
            if (Owner.navFrame.CanGoBack)
                Owner.navFrame.NavigationService.GoBack();
        }

        public void GoLogin()
        {
            if (!(CurrentPage is LoginPage))
                CurrentPage = new LoginPage(this);
        }

        public void GoMain()
        {
            CurrentPage = mainPage;
        }

        public void GoSettings()
        {
            if (!(CurrentPage is SettingsPage))
                CurrentPage = new SettingsPage(this);
        }
    }
}
