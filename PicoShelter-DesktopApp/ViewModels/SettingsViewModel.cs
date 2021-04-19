using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicoShelter_DesktopApp.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        public SettingsViewModel()
        {
            
        }

        public SettingsViewModel(ApplicationViewModel owner) : this()
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
    }
}
