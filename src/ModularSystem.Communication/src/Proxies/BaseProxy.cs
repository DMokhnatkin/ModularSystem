using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Newtonsoft.Json;

namespace ModularSystem.Communication.Proxies
{
    public abstract class BaseProxy
    {
        protected HttpClient Client;

        public string BaseUrl { get; set; }

        /// <summary>
        /// Don't initialize proxy. Before use this proxy call GetTokenAsync manually.
        /// </summary>
        protected BaseProxy(string baseUrl)
        {
            Client = new HttpClient();
            BaseUrl = baseUrl;
        }

        public void SetToken(string token)
        {
            Client.SetBearerToken(token);
        }

        public async Task<bool> GetTokenAsync(string clientId, string clientSecret, string userName, string password, string scope = null)
        {
            var disco = await DiscoveryClient.GetAsync(BaseUrl);

            // request token
            var tokenClient = new TokenClient(disco.TokenEndpoint, clientId, clientSecret);
            var tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync(userName, password, scope);

            if (tokenResponse.IsError)
                return false;

            Client.SetBearerToken(tokenResponse.AccessToken);
            return true;
        }
    }
}
