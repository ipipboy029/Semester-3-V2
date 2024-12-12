using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models
{
    public class PlayerData
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("avatar_url")]
        public string Avatar_url { get; set; }

        [JsonProperty("country_code")]
        public string Country_Code { get; set; }

        [JsonProperty("statistics")]
        public UserStatistics userStatistics { get; set; }
    }

}
