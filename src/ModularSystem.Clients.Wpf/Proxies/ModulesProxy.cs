using System.Net.Http;
using System.Threading.Tasks;
using ModularSystem.Communication.Proxies;

namespace ModularSystem.Clients.Wpf.Proxies
{
    public class ModulesProxy : BaseProxy
    {
        /// <inheritdoc />
        public ModulesProxy(string baseUrl) : base(baseUrl)
        {
        }

        public async Task<HttpResponseMessage> DownloadModules()
        {
            return await Client.GetAsync($"{BaseUrl}/api/modules/download");
        }
    }
}
