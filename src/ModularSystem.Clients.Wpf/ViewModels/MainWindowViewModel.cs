using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Windows;
using ModularSystem.Clients.Wpf.Proxies;
using ModularSystem.Common.Modules;
using ModularSystem.Common.Repositories;
using ModularSystem.Common.Wpf.Modules;
using ModularSystem.Communication.Data.Files;
using Prism.Mvvm;

namespace ModularSystem.Clients.Wpf.ViewModels
{
    class MainWindowViewModel : BindableBase
    {
        public LoginViewModel LoginViewModel { get; } = new LoginViewModel();

        /// <summary>
        /// Modules which was downloaded for cur user
        /// </summary>
        private readonly InstalledModuleCollection _sessionModules = new InstalledModuleCollection(Path.Combine(AppContext.BaseDirectory, "curmodules"));

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(ref _isBusy, value); }
        }

        public async Task DownloadModules()
        {
            ModulesProxy proxy = new ModulesProxy("http://localhost:5005");
            proxy.SetToken(LoginViewModel.Token);
            var t = await proxy.DownloadModules();
            var p = await ModulesPackage.Decompress(await t.Content.ReadAsStreamAsync());

            foreach (var packageModule in p.PackagedModules)
            {
                _sessionModules.InstallZipPackagedModule(packageModule);
            }

            _sessionModules.StartModules();
        }
    }
}
