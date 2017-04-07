using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Threading.Tasks;
using ModularSystem.Communication.Proxies;
using Newtonsoft.Json;

namespace ModularSystem.Сonfigurator.Proxies
{
    public class ModulesProxy : BaseProxy
    {
        public MediaTypeFormatter MediaTypeFormatter { get; set; } = new JsonMediaTypeFormatter()
        {
            SerializerSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }
        };

        public ModulesProxy(string baseUrl) : base(baseUrl)
        { }

        public async Task<HttpResponseMessage> InstallModulePackageAsync(FileStream modulePackage)
        {
            var content = new StreamContent(modulePackage);
            content.Headers.ContentType = new MediaTypeHeaderValue(MediaTypeNames.Application.Zip);
            return await Client.PostAsync($"{BaseUrl}/api/modulesconfig/install", content);
        }

        public async Task<HttpResponseMessage> RemoveModuleAsync(string identity)
        {
            return await Client.PutAsync($"{BaseUrl}/api/modulesconfig/remove", new ObjectContent(typeof(string), identity, MediaTypeFormatter));
        }

        public async Task<string[]> GetModulesListAsync()
        {
            var res = await Client.GetAsync($"{BaseUrl}/api/modulesconfig");
            return JsonConvert.DeserializeObject<string[]>(await res.Content.ReadAsStringAsync());
        }

        public async Task<HttpResponseMessage> AddUserModules(string userId, string clientId, string[] dtos)
        {
            return await Client.PostAsync($"{BaseUrl}/api/modulesconfig/user/{userId}/{clientId}", new ObjectContent(typeof(string[]), dtos, MediaTypeFormatter));
        }

        public async Task<HttpResponseMessage> RemoveUserModules(string userId, string clientId, string[] dtos)
        {
            UriBuilder builder = new UriBuilder($"{BaseUrl}/api/modulesconfig/user/{userId}/{clientId}");
            // It looks terrible. I can't find any query builder.
            builder.Query = string.Concat(dtos.Select(x => $"moduleIdentities={x}&")).TrimEnd('&');
            return await Client.DeleteAsync(builder.ToString());
        }

        public async Task<string[]> GetUserModules(string userId)
        {
            var res = await Client.GetAsync($"{BaseUrl}/api/modulesconfig/user/{userId}");
            return JsonConvert.DeserializeObject<string[]>(await res.Content.ReadAsStringAsync());
        }
    }
}
