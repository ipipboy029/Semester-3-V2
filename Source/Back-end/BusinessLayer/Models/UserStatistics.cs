using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Models
{
    public class UserStatistics
    {
        [JsonProperty("global_rank")]
        public int? GlobalRank { get; set; } // Nullable, as it can be null in some cases

        [JsonProperty("country_rank")]
        public int? CountryRank { get; set; } // Nullable, as it can be null
    }
}
