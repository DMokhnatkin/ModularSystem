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
using System.Windows.Shapes;
using ModularSystem.Clients.Wpf.ViewModels;

namespace ModularSystem.Clients.Wpf.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private LoginViewModel ViewModel
        {
            get { return (LoginViewModel) DataContext; }
            set { SetValue(DataContextProperty, value); }
        }

        private void WatermarkPasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            ((LoginViewModel) DataContext).UserPassword = Password.SecurePassword;
        }

        private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            ViewModel.Token = null;
            await ViewModel.LoginExecute();
            if (ViewModel.IsLoggedIn)
                Close();
        }
    }
}
