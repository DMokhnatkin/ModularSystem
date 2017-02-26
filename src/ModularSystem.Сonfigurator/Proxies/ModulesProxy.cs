using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Threading.Tasks;
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

        public async Task<HttpResponseMessage> RemoveModuleAsync(string identity)
        {
            return await client.PutAsync($"{BaseUrl}/api/modules/remove", new ObjectContent(typeof(string), identity, MediaTypeFormatter));
        }

        public async Task<string[]> GetModulesListAsync()
        {
            var res = await client.GetAsync($"{BaseUrl}/api/modules");
            return JsonConvert.DeserializeObject<string[]>(await res.Content.ReadAsStringAsync());
        }

        public async Task<HttpResponseMessage> AddUserModules(string userId, string[] dtos)
        {
            return await client.PostAsync($"{BaseUrl}/api/modules/user/{userId}", new ObjectContent(typeof(string[]), dtos, MediaTypeFormatter));
        }

        public async Task<HttpResponseMessage> RemoveUserModules(string userId, string[] dtos)
        {
            UriBuilder builder = new UriBuilder($"{BaseUrl}/api/modules/user/{userId}");
            // It looks terrible. I can't find any query builder.
            builder.Query = string.Concat(dtos.Select(x => $"moduleIdentities={x}&")).TrimEnd('&');
            return await client.DeleteAsync(builder.ToString());
        }

        public async Task<string[]> GetUserModules(string userId)
        {
            var res = await client.GetAsync($"{BaseUrl}/api/modules/user/{userId}");
            return JsonConvert.DeserializeObject<string[]>(await res.Content.ReadAsStringAsync());
        }
    }
}
