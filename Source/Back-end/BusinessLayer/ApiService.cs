using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _apiUrl;

        public ApiService(string apiKey, string apiUrl)
        {
            _apiKey = apiKey;
            _apiUrl = apiUrl;
            _httpClient = new HttpClient();
        }

        private async Task<string> GetDataFromApiAsync(string endPoint)
        {
            var response = await _httpClient.GetAsync(_apiUrl + endPoint);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new HttpRequestException($"Request failed with status code {response.StatusCode}");
            }
        }
        public async Task<string> GetData()
        {
            string apiUrl = $"bridge?auth={_apiKey}&player=ipipboy029&platform=PC";
            var data = await GetDataFromApiAsync(apiUrl);
            return data;
        }
    }
}
