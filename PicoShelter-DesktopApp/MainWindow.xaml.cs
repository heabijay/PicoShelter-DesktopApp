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

namespace PicoShelter_DesktopApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ApplicationViewModel ViewModel => this.DataContext as ApplicationViewModel;
        
        public MainWindow()
        {
            InitializeComponent();
            ViewModel.Owner = this;
        }

        void mainHeader_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) this.DragMove();
        }

        void mainExit_Click(object sender, RoutedEventArgs e) => this.Close();

        void mainMinimize_Click(object sender, RoutedEventArgs e) => this.WindowState = WindowState.Minimized;
    }
}
