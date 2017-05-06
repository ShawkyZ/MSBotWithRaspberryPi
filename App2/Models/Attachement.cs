using Newtonsoft.Json;

namespace App2.Models
{
    public class Attachement
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("contentType")]
        public string ContentType { get; set; }
    }
}
