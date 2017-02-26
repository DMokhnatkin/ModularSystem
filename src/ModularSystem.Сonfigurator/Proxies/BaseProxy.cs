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
            InitializeAsync(baseUrl, clientId, clientSecret, userName, password, scope).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Don't initialize proxy. Before use this proxy call InitializeAsync manually.
        /// </summary>
        protected BaseProxy()
        {
        }

        public async Task InitializeAsync(string baseUrl, string clientId, string clientSecret, string userName, string password, string scope = null)
        {
            BaseUrl = baseUrl;

            var disco = await DiscoveryClient.GetAsync(BaseUrl);

            // request token
            var tokenClient =new TokenClient(disco.TokenEndpoint, "configurator", "g6wCBw");
            var tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync(userName, password, scope);

            Client = new HttpClient();
            Client.SetBearerToken(tokenResponse.AccessToken);
        }
    }
}
