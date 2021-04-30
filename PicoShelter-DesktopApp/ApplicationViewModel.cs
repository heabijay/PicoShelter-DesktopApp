using PicoShelter_DesktopApp.DTOs;
using PicoShelter_DesktopApp.Models;
using PicoShelter_DesktopApp.Pages;
using PicoShelter_DesktopApp.Services;
using PicoShelter_DesktopApp.Services.AppSettings;
using PicoShelter_DesktopApp.Services.AppSettings.Enums;
using PicoShelter_DesktopApp.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PicoShelter_DesktopApp
{
    public class ApplicationViewModel : ViewModelBase
    {
        public ApplicationViewModel()
        {
            var settings = AppSettingsProvider.Provide();
            settings.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(settings.Locale))
                    Language = settings.Locale;
            };
            Language = settings.Locale;

            mainPage = new MainPage(this);
            settingsPage = new SettingsPage(this);

            if (settings.AccessToken != null)
            {
                if (settings.AccessToken == "")
                {
                    CurrentPage = mainPage;
                }
                else
                {
                    HttpService.Current.AccessToken = settings.AccessToken;

                    var page = new LoginPage(this);
                    page.ViewModel.SignInSessionCommand.Execute(null);
                    CurrentPage = page;
                }
            }
            else
            {
                CurrentPage = new WelcomePage(this);
            }
        }

        public ApplicationViewModel(MainWindow owner) : this()
        {
            this.View = owner;
        }

        private MainWindow view { get; set; }
        public MainWindow View
        {
            get => view;
            set
            {
                view = value;
                OnPropertyChanged();
            }
        }

        private Page mainPage { get; set; }
        private Page settingsPage { get; set; }

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
                OnPropertyChanged(nameof(IsCurrentUserAnonymous));
                OnPropertyChanged(nameof(CurrentUserString));
            }
        }

        public bool IsCurrentUserAnonymous => CurrentUser == null;

        public string CurrentUserString
        {
            get
            {
                if (IsCurrentUserAnonymous)
                    return LanguageResolve("Shared.Anonymous").ToString();

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
            if (View.navFrame.CanGoBack)
                View.navFrame.NavigationService.GoBack();
            else if (CurrentPage is LoginPage)
                CurrentPage = new WelcomePage(this);
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
            CurrentPage = settingsPage;
        }

        public virtual void ConsoleArgsExecute(string[] args)
        {
            (mainPage as MainPage)?.ViewModel?.AddUploadTasks(args.Skip(1).ToArray());
        }

        public void PipeService_CommandReceived(PipeCommand command)
        {
            App.Current.MainWindow.Activate();

            switch (command.Type)
            {
                case PipeCommand.CommandType.OpenSecondInstance:
                    string[] args = command.CommandLineArgs;
                    ConsoleArgsExecute(args);
                    break;
            }
        }

        public static event EventHandler LanguageChanged;

        public static LocaleOptions Language
        {
            get
            {
                return Enum.Parse<LocaleOptions>(System.Threading.Thread.CurrentThread.CurrentUICulture.Name.Replace("-", "_"));
            }
            set
            {
                var localeName = value.ToString().Replace("_", "-");

                if (localeName == System.Threading.Thread.CurrentThread.CurrentUICulture.Name) return;


                System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo(localeName);

                ResourceDictionary dict = new ResourceDictionary();
                switch (localeName)
                {
                    case "en-US":
                        dict.Source = new Uri("Resources/Locales/lang.xaml", UriKind.Relative);
                        break;
                    default:
                        dict.Source = new Uri(String.Format("Resources/Locales/lang.{0}.xaml", localeName), UriKind.Relative);
                        break;
                }

                LanguageDictionary = dict;

                LanguageChanged?.Invoke(Application.Current, new EventArgs());
            }
        }

        private static ResourceDictionary languageDictionary { get; set; }
        private static ResourceDictionary LanguageDictionary
        {
            get
            {
                return languageDictionary ??= (from d in Application.Current.Resources.MergedDictionaries
                                               where d.Source != null && d.Source.OriginalString.StartsWith("Resources/Locales/lang.")
                                               select d).Skip(1).FirstOrDefault();
            }
            set
            {
                if (LanguageDictionary == value)
                    return;

                if (LanguageDictionary != null)
                {
                    int ind = Application.Current.Resources.MergedDictionaries.IndexOf(LanguageDictionary);
                    Application.Current.Resources.MergedDictionaries.Remove(LanguageDictionary);
                    Application.Current.Resources.MergedDictionaries.Insert(ind, value);
                }
                else
                {
                    Application.Current.Resources.MergedDictionaries.Add(value);
                }

                languageDictionary = value;
            }
        }

        public static object LanguageResolve(string key)
        {
            var val = LanguageDictionary[key];

            if (val != null)
                return val;

            val = (from d in Application.Current.Resources.MergedDictionaries
                    where d.Source != null && d.Source.OriginalString.StartsWith("Resources/Locales/lang.")
                    select d).LastOrDefault(t => t[key] != null)[key];

            return val;
        }
    }
}
