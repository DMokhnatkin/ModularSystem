using System;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using IdentityModel.Client;
using ModularSystem.Communication.Proxies;
using Prism.Commands;
using Prism.Mvvm;

namespace ModularSystem.Clients.Wpf.ViewModels
{
    class LoginViewModel : BindableBase
    {
        public LoginViewModel()
        {
        }

        private string _userName;
        private SecureString _userPassword;
        private string _token;
        private bool _isBusy;

        public string UserName
        {
            get { return _userName; }
            set { SetProperty(ref _userName, value); }
        }

        public SecureString UserPassword
        {
            get { return _userPassword; }
            set { SetProperty(ref _userPassword, value); }
        }

        public string Token
        {
            get { return _token; }
            set
            {
                SetProperty(ref _token, value);
                OnPropertyChanged(nameof(IsLoggedIn));
            }
        }

        public bool IsLoggedIn => _token != null;

        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(ref _isBusy, value); }
        }

        public async Task LoginExecute()
        {
            IsBusy = true;

            try
            {
                var tokenResponse = await AuthorizationHelper.GetTokenAsync("http://localhost:5005", "wpfclient", "g6wCBw2",
                    "alice", "password");

                if (!tokenResponse.IsError)
                    Token = tokenResponse.AccessToken;
            }
            catch (Exception e)
            {
                // ignored
            }
            IsBusy = false;
        }
    }
}
