using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AzureAuthorizationFunctionApp.Controllers
{
    public class AzTokenController
    {
        public static readonly AzTokenController Instance = new AzTokenController();

        private AzTokenController() { }

        public async Task<UserDetails> GetTokenDetails(string accessToken)
        {
            var url = $"https://graph.microsoft.com/v1.0/me";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var response = await client.GetAsync(url);

                return JsonConvert.DeserializeObject<UserDetails>(await response.Content.ReadAsStringAsync());
            }
        }

    }

    public class UserDetails
    {

        public string userPrincipalName { get; set; }
        public string id { get; set; }

        public string mail { get; set; }


    }
}
