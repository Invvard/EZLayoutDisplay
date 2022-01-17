using Newtonsoft.Json;

namespace InvvardDev.EZLayoutDisplay.Tool.KeyDefinitionProvider.Models
{
    public class OryxMetadataModel
    {
        [JsonProperty("keys")]
        public List<OryxKeyDefinition> Keys { get; set; } = new List<OryxKeyDefinition>();

        [JsonProperty("categories")]
        public List<OryxCategory> Categories { get; set; } = new List<OryxCategory>();
    }
}
