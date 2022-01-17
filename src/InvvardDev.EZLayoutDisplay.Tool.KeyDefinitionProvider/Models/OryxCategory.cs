using Newtonsoft.Json;

namespace InvvardDev.EZLayoutDisplay.Tool.KeyDefinitionProvider.Models
{
    public class OryxCategory
    {
        [JsonProperty("id")]
        public int CategoryId { get; set; }

        [JsonProperty("code")]
        public string? Code { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("deleted")]
        public bool IsDeleted { get; set; }
    }
}
