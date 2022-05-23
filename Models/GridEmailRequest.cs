using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Schedular.Models
{
    public class GridEmailRequest
    {

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("orderCode")]
        public string OrderCode { get; set; }

        public string templateIds { get; set; }
    }
}
