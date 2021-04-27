using PicoShelter_DesktopApp.Models;
using PicoShelter_DesktopApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
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
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
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

        public MainViewModel ViewModel => this.DataContext as MainViewModel;

        public MainPage()
        {
            InitializeComponent();
        }

        public MainPage(ApplicationViewModel owner) : this()
        {
            this.Owner = owner;
        }

        private void MainDrop_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                ViewModel.AddUploadTasks(files);
            }
        }

        private void imageItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && e.ClickCount == 2)
            {
                Grid grid = sender as Grid;
                UploadTask image = grid.DataContext as UploadTask;

                if (image != null)
                    Statics.WindowsLaunchManager.OpenFile(image.Filepath);
            }
        }
    }
}
