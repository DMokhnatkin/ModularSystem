using System;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Practices.Unity;
using ModularSystem.Clients.Wpf.Proxies;
using ModularSystem.Common;
using ModularSystem.Common.Modules;
using ModularSystem.Common.PackedModules.Zip;
using ModularSystem.Common.Repositories;
using ModularSystem.Common.Wpf.Context;
using ModularSystem.Common.Wpf.Modules;
using ModularSystem.Common.Wpf.UI;
using Prism.Mvvm;

namespace ModularSystem.Clients.Wpf.ViewModels
{
    class MainWindowViewModel : BindableBase
    {
        public MainWindowViewModel()
        {
            ClientAppContext.CurrentContext = new ClientAppContext(_sessionModules, new AuthenticationContext());
            ClientAppContext.CurrentContext.AuthenticationContext.ClientId = "wpfclient";
            ClientAppContext.CurrentContext.AuthenticationContext.ClientPassword = "g6wCBw2";

            ClientAppContext.CurrentContext.Container.RegisterInstance(new MainUi());
        }

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
            proxy.SetToken(ClientAppContext.CurrentContext.AuthenticationContext.AccessToken);
            var t = await proxy.DownloadModules();

            using (var s = await t.Content.ReadAsStreamAsync())
            using (var ss = new BinaryReader(s))
            {
                var b = new MemoryBatchedModules(ss.ReadBytes((int)s.Length));
                MemoryPackedModule[] innerModules;
                b.UnbatchModules(out innerModules);

                foreach (var packageModule in innerModules)
                {
                    _sessionModules.InstallZipPackagedModule(packageModule);
                }
            }
            ClientAppContext.CurrentContext.InstalledModules = _sessionModules;

            _sessionModules.StartModules();
        }
    }
}
