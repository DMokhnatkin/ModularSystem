using System.Net.Http;
using System.Threading.Tasks;
using ModularSystem.Common;
using ModularSystem.Communication.Data.Dto;
using Newtonsoft.Json;

namespace ModularSystem.Сonfigurator.Proxies
{
    public class ModulesProxy : BaseProxy
    {
        /// <inheritdoc />
        public ModulesProxy(string baseUrl) : base(baseUrl)
        { }

        public async Task<HttpResponseMessage> InstallModuleAsync(ModuleDto module)
        {
            return await client.PostAsync($"{BaseUrl}/api/modules/install", new ObjectContent(typeof(ModuleDto), module, MediaTypeFormatter));
        }

        public async Task<HttpResponseMessage> RemoveModuleAsync(ModuleIdentity identity)
        {
            return await client.PutAsync($"{BaseUrl}/api/modules/remove", new ObjectContent(typeof(ModuleIdentityDto), identity, MediaTypeFormatter));
        }

        public async Task<ModuleIdentityDto[]> GetModulesListAsync()
        {
            var res = await client.GetAsync($"{BaseUrl}/api/modules");
            return JsonConvert.DeserializeObject<ModuleIdentityDto[]>(await res.Content.ReadAsStringAsync());
        }
    }
}
