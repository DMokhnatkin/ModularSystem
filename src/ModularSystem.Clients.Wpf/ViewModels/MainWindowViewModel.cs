using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using System.Windows;
using ModularSystem.Clients.Wpf.Proxies;
using ModularSystem.Common.Wpf.Helpers;
using ModularSystem.Communication.Data.Files;
using Prism.Mvvm;

namespace ModularSystem.Clients.Wpf.ViewModels
{
    class MainWindowViewModel : BindableBase
    {
        public LoginViewModel LoginViewModel { get; } = new LoginViewModel();

        private FrameworkElement _content;
        public FrameworkElement Content
        {
            get { return _content; }
            set { SetProperty(ref _content, value); }
        }

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
            if (Directory.Exists(Path.Combine(AppContext.BaseDirectory, "curmodules")))
                Directory.Delete(Path.Combine(AppContext.BaseDirectory, "curmodules"), true);
            p.InstallToClient(Path.Combine(AppContext.BaseDirectory, "curmodules"));
        }
    }
}
