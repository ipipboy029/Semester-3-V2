using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class PlayerService
    {
        public static async Task GetUserDataAsync(string accessToken, int userId)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

                string userApiUrl = $"https://osu.ppy.sh/api/v2/users/{userId}";
                HttpResponseMessage response = await client.GetAsync(userApiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var userData = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(userData);
                }
                else
                {
                    Console.WriteLine("Failed to retrieve user data.");
                }
            }
        }
    }
}
