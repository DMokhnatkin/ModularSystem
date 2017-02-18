using System.Net.Http;
using System.Threading.Tasks;
using ModularSystem.Common;
using ModularSystem.Communication.Data;
using ModularSystem.Communication.Data.Dto;

namespace ModularSystem.Сonfigurator.Proxies
{
    public class ModulesProxy : BaseProxy
    {
        public async Task<HttpResponseMessage> InstallModuleAsync(ModuleDto module)
        {
            return await client.PostAsync("http://localhost:5005/api/modules/install", new ObjectContent(typeof(ModuleDto), module, MediaTypeFormatter));
        }

        public async Task<HttpResponseMessage> RemoveModuleAsync(ModuleIdentity module)
        {
            return await client.PutAsync("http://localhost:5005/api/modules/remove", null);
        }
    }
}
