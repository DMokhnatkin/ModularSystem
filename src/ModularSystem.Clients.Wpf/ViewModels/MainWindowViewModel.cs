using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using ModularSystem.Clients.Wpf.Proxies;
using Prism.Mvvm;

namespace ModularSystem.Clients.Wpf.ViewModels
{
    class MainWindowViewModel : BindableBase
    {
        public LoginViewModel LoginViewModel { get; } = new LoginViewModel();

        public async Task DownloadExecute()
        {
            ModulesProxy proxy = new ModulesProxy("http://localhost:5005");
            await proxy.GetTokenAsync("wpfclient", "g6wCBw2", "alice", "password");
            var t = await proxy.DownloadModules();
            using (ZipArchive z = new ZipArchive(await t.Content.ReadAsStreamAsync()))
            {
                z.ExtractToDirectory(Path.Combine(AppContext.BaseDirectory, "curmodules"));
            }
        }
    }
}
