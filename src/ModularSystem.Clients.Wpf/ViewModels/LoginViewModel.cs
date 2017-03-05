using System;
using System.Runtime.InteropServices;
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

        private string SecureStringToString(SecureString value)
        {
            IntPtr valuePtr = IntPtr.Zero;
            try
            {
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(value);
                return Marshal.PtrToStringUni(valuePtr);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }
        }

        public async Task LoginExecute()
        {
            IsBusy = true;

            try
            {
                var tokenResponse = await AuthorizationHelper.GetTokenAsync("http://localhost:5005", "wpfclient", "g6wCBw2",
                    UserName, SecureStringToString(UserPassword));

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
