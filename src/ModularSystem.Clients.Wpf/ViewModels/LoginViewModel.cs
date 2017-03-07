using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using IdentityModel.Client;
using ModularSystem.Common.Wpf.Context;
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

        public bool IsLoggedIn => ClientAppContext.CurrentContext?.AuthenticationContext?.AccessToken != null;

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
                var tokenResponse = await AuthorizationHelper.GetTokenAsync("http://localhost:5005", 
                    ClientAppContext.CurrentContext.AuthenticationContext.ClientId, 
                    ClientAppContext.CurrentContext.AuthenticationContext.ClientPassword,
                    UserName, SecureStringToString(UserPassword));

                if (!tokenResponse.IsError)
                {
                    ClientAppContext.CurrentContext.AuthenticationContext.UserName = UserName;
                    ClientAppContext.CurrentContext.AuthenticationContext.AccessToken = tokenResponse.AccessToken;
                }
            }
            catch (Exception e)
            {
                // ignored
            }
            IsBusy = false;
        }
    }
}
