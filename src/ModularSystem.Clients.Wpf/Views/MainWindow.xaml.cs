using System;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Practices.Unity;
using ModularSystem.Clients.Wpf.ViewModels;
using ModularSystem.Common.Wpf;
using ModularSystem.Common.Wpf.Context;
using ModularSystem.Common.Wpf.UI;

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

        private void UpdateContent()
        {
            ContentWrapper.Content = ClientAppContext.CurrentContext.Container.Resolve<MainUi>().MainContent;
        }

        private async Task OpenLogin()
        {
            LoginView v = new LoginView { DataContext = ViewModel.LoginViewModel };
            v.ShowDialog();
            ViewModel.IsBusy = true;
            if (ViewModel.LoginViewModel.IsLoggedIn)
                await ViewModel.DownloadModules();
            UpdateContent();
            ViewModel.IsBusy = false;
        }
    }
}
