using System.Collections.Generic;
using Newtonsoft.Json;

namespace App2.Models
{
    public class ConversationMessages
    {
        [JsonProperty("messages")]
        public IList<Message> Messages { get; set; }

        [JsonProperty("watermark")]
        public string Watermark { get; set; }
    }
}
