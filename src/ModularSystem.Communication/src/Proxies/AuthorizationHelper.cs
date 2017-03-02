using System.Threading.Tasks;
using IdentityModel.Client;

namespace ModularSystem.Communication.Proxies
{
    public static class AuthorizationHelper
    {
        public static async Task<TokenResponse> GetTokenAsync(string baseUrl, string clientId, string clientSecret, string userName, string password, string scope = null)
        {
            var disco = await DiscoveryClient.GetAsync(baseUrl);

            // request token
            var tokenClient = new TokenClient(disco.TokenEndpoint, clientId, clientSecret);
            var tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync(userName, password, scope);

            return tokenResponse;
        }
    }
}
