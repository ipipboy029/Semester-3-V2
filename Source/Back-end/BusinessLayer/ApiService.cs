using BusinessLayer.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class ApiService
    {
        private readonly string tokenEndpoint = "https://osu.ppy.sh/oauth/token";
        private readonly string _clientId;
        private readonly string _clientSecret;

        private AuthToken _authToken;

        public ApiService(string clientId, string clientSecret)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
            _authToken = new AuthToken();
        }
        private async Task<AuthToken> GetAccessTokenAsync()
        {
            using (var httpClient = new HttpClient())
            {
                var body = new StringContent($"client_id={_clientId}&client_secret={_clientSecret}&grant_type=client_credentials&scope=public", Encoding.UTF8, "application/x-www-form-urlencoded");
                var response = await httpClient.PostAsync(tokenEndpoint, body);
                var responseContent = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Error fetching token:");
                    Console.WriteLine($"Status Code: {response.StatusCode}");
                    Console.WriteLine($"Response: {responseContent}");
                    Console.WriteLine(responseContent);
                    return null;
                }
                var tokenResponse = JsonConvert.DeserializeObject<Token>(responseContent);
                AuthToken token = new AuthToken
                {
                    token = tokenResponse.AccessToken,
                    expireDate = DateTime.Now.AddSeconds(tokenResponse.ExpiresIn)
                };
                return token;
            }
        }
        public async void Init()
        {
            _authToken = await GetAccessTokenAsync();
        }
    }
}
