using InvvardDev.EZLayoutDisplay.Desktop.Model;
using Newtonsoft.Json;

namespace InvvardDev.EZLayoutDisplay.Tool.KeyDefinitionProvider.Models
{
    public class OryxKeyDefinition
    {
        [JsonProperty("key_category_id")]
        public int KeyCategoryId { get; set; }

        [JsonProperty("code")]
        public string? Code { get; set; }

        [JsonProperty("label")]
        public string? Label { get; set; }

        [JsonProperty("tag")]
        public string? Tag { get; set; }

        [JsonProperty("glyph")]
        public string? Glyph { get; set; }

        public string? GlyphCode { get; set; }

        public static implicit operator KeyDefinition(OryxKeyDefinition oryxKey)
        {
            return new KeyDefinition
            {
                Label = oryxKey.Label,
                GlyphCode = oryxKey.GlyphCode,
                KeyCode = oryxKey.Code
            };
        }
    }
}