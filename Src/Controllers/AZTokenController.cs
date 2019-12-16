using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AzureAuthorizationFunctionApp.Controllers
{
    /// <summary>A class to manage Azure Token functions</summary>
    public class AzTokenController
    {
        /// <summary>  Generates an instance of the object AzTokenController</summary>
        public static readonly AzTokenController Instance = new AzTokenController();

        /// <summary>Prevents a default instance of the <see cref="AzTokenController"/> class from being created.</summary>
        private AzTokenController() { }

        /// <summary>Gets the token details.</summary>
        /// <param name="accessToken">The access token.</param>
        /// <returns></returns>
        public async Task<UserDetails> GetTokenDetails(string accessToken)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var response = await client.GetAsync("https://graph.microsoft.com/v1.0/me");
                return JsonConvert.DeserializeObject<UserDetails>(await response.Content.ReadAsStringAsync());
            }
        }
    }
}