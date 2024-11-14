using BusinessLayer.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace BusinessLayer
{
    public class ApiService
    {
        private readonly string _tokenEndpoint = "https://osu.ppy.sh/oauth/token";
        private readonly string _baseUrl = "https://osu.ppy.sh/api/v2/";
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly HttpClient _httpClient = new HttpClient();

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
                var response = await httpClient.PostAsync(_tokenEndpoint, body);
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

        public async Task<string> Request(string endPoint)
        {
            string jsonResponse;

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authToken.token);

                using HttpResponseMessage response = await httpClient.GetAsync(_baseUrl + endPoint);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Error fetching token:");
                    Console.WriteLine($"Status Code: {response.StatusCode}");
                    return null;
                }
                jsonResponse = await response.Content.ReadAsStringAsync();
            }
            return jsonResponse;
        }

        public ApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<string> GetOsuPerformanceRankingAsync()
        {
            var url = "https://osu.ppy.sh/api/v2/rankings/osu/performance";
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _authToken.token);

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new HttpRequestException($"Request failed with status code {response.StatusCode}");
            }
        }
        public async Task<PlayerData> GetPlayerDataAsync(string usernameOrId)
        {
            var requestUrl = $"https://osu.ppy.sh/api/v2/users/{usernameOrId}/osu";

            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            request.Headers.Add("Authorization", $"Bearer {_authToken.token}");

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var playerData = JsonConvert.DeserializeObject<PlayerData>(jsonString);
                return playerData;
            }
            else
            {
                Console.WriteLine($"Failed to retrieve player data: {response.StatusCode}");
                return null;
            }
        }
    }
}
