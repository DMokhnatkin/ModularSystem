using System;
using System.Threading.Tasks;
using System.Windows;
using ModularSystem.Clients.Wpf.ViewModels;

namespace ModularSystem.Clients.Wpf.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        internal MainWindowViewModel ViewModel
        {
            get { return (MainWindowViewModel)DataContext; }
            set { SetValue(DataContextProperty, value); }
        }

        private async void MainWindow_OnInitialized(object sender, EventArgs e)
        {
            await OpenLogin();
        }

        private async Task OpenLogin()
        {
            LoginView v = new LoginView { DataContext = ViewModel.LoginViewModel };
            v.ShowDialog();
            ViewModel.IsBusy = true;
            if (ViewModel.LoginViewModel.IsLoggedIn)
                await ViewModel.DownloadModules();
            ViewModel.IsBusy = false;
        }
    }
}
