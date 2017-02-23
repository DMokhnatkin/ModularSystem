using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Threading.Tasks;
using ModularSystem.Communication.Data.Dto;
using Newtonsoft.Json;

namespace ModularSystem.Сonfigurator.Proxies
{
    public class ModulesProxy : BaseProxy
    {
        /// <inheritdoc />
        public ModulesProxy(string baseUrl) : base(baseUrl)
        { }

        public async Task<HttpResponseMessage> InstallModulePackageAsync(FileStream modulePackage)
        {
            var content = new StreamContent(modulePackage);
            content.Headers.ContentType = new MediaTypeHeaderValue(MediaTypeNames.Application.Zip);
            return await client.PostAsync($"{BaseUrl}/api/modules/install", content);
        }

        public async Task<HttpResponseMessage> RemoveModuleAsync(ModuleIdentityDto identity)
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
