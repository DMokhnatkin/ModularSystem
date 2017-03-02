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
using ModularSystem.Clients.Wpf.ViewModels;
using ModularSystem.Clients.Wpf.Views;

namespace ModularSystem.Clients.Wpf
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

        private void CommandBinding_OnExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ((MainWindowViewModel)DataContext).DownloadExecute();
        }

        private void MainWindow_OnInitialized(object sender, EventArgs e)
        {
            OpenLogin();
        }

        private void OpenLogin()
        {
            LoginView v = new LoginView { DataContext = ViewModel.LoginViewModel };
            var r = v.ShowDialog();
        }
    }
}
