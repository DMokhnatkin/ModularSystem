using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Newtonsoft.Json.Linq;

namespace ModularSystem.Сonfigurator
{
    class Program
    {
        static void Main(string[] args)
        {
            TestAsync().Wait();
            Console.ReadLine();
        }

        static async Task<TokenResponse> GetTokenAsync()
        {
            var disco = await DiscoveryClient.GetAsync("http://localhost:5005");

            // request token
            var tokenClient = new TokenClient(disco.TokenEndpoint, "client", "secret");
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync("api1");

            return tokenResponse;
        } 

        static async Task TestAsync()
        {
            // call api
            var client = new HttpClient();
            client.SetBearerToken((await GetTokenAsync()).AccessToken);

            var response = await client.GetAsync("http://localhost:5005/api/modules/test");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);
            }
        }
    }
}
