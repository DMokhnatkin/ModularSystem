using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using IdentityModel.Client;

namespace ModularSystem.Сonfigurator.Proxies
{
    public abstract class BaseProxy
    {
        protected HttpClient client;

        public MediaTypeFormatter MediaTypeFormatter { get; set; } = new JsonMediaTypeFormatter();

        protected BaseProxy()
        {
            client = new HttpClient();
            client.SetBearerToken(GetTokenAsync().Result.AccessToken);
        }

        static async Task<TokenResponse> GetTokenAsync()
        {
            var disco = await DiscoveryClient.GetAsync("http://localhost:5005");

            // request token
            var tokenClient = new TokenClient(disco.TokenEndpoint, "configurator", "g6wCBw");
            var tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync("alice", "password", "modules");

            return tokenResponse;
        }
    }
}
