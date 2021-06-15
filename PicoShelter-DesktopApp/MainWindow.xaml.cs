using System;
using System.Windows;
using System.Windows.Input;

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
            ViewModel.View = this;

            string[] args = Environment.GetCommandLineArgs();
            ViewModel.ConsoleArgsExecute(args);

            (App.Current as App).PipeService.CommandReceived += ViewModel.PipeService_CommandReceived;
        }

        void mainHeader_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) this.DragMove();
        }

        void mainExit_Click(object sender, RoutedEventArgs e) => this.Close();

        void mainMinimize_Click(object sender, RoutedEventArgs e) => this.WindowState = WindowState.Minimized;

        private void mainSettings_Click(object sender, RoutedEventArgs e) => ViewModel.GoSettings();
    }
}
