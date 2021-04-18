using PicoShelter_DesktopApp.Pages;
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
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public ApplicationViewModel()
        {
            currentPage = new WelcomePage(this);
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

        public void ToBack()
        {
            if (Owner.navFrame.CanGoBack)
                Owner.navFrame.NavigationService.GoBack();
        }

        public void ToLogin()
        {
            CurrentPage = new LoginPage(this);
        }
    }
}
