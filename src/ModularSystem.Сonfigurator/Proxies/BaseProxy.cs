using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using IdentityModel.Client;
using Newtonsoft.Json;

namespace ModularSystem.Сonfigurator.Proxies
{
    public abstract class BaseProxy
    {
        protected HttpClient Client;

        public MediaTypeFormatter MediaTypeFormatter { get; set; } = new JsonMediaTypeFormatter()
        {
            SerializerSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            }
        };

        public string BaseUrl { get; set; }

        protected BaseProxy(string baseUrl, string clientId, string clientSecret, string userName, string password, string scope = null)
        {
            Client = new HttpClient();
            BaseUrl = baseUrl;

            GetTokenAsync(clientId, clientSecret, userName, password, scope).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Don't initialize proxy. Before use this proxy call GetTokenAsync manually.
        /// </summary>
        protected BaseProxy(string baseUrl)
        {
            Client = new HttpClient();
            BaseUrl = baseUrl;
        }

        public async Task<bool> GetTokenAsync(string clientId, string clientSecret, string userName, string password, string scope = null)
        {
            var disco = await DiscoveryClient.GetAsync(BaseUrl);

            // request token
            var tokenClient =new TokenClient(disco.TokenEndpoint, "configurator", "g6wCBw");
            var tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync(userName, password, scope);

            if (tokenResponse.IsError)
                return false;

            Client.SetBearerToken(tokenResponse.AccessToken);
            return true;
        }
    }
}
