using InvvardDev.EZLayoutDisplay.Desktop.Model;
using InvvardDev.EZLayoutDisplay.Desktop.Model.Enum;
using Newtonsoft.Json;

namespace InvvardDev.EZLayoutDisplay.Tool.KeyDefinitionProvider.Models
{
    public class OryxKeyDefinition
    {
        [JsonProperty("code")]
        public string? Code { get; set; }

        [JsonProperty("label")]
        public string? Label { get; set; }

        [JsonProperty("tag")]
        public string? Tag { get; set; }

        [JsonProperty("glyph")]
        public string? GlyphName { get; set; }

        [JsonProperty("key_category_id")]
        public int? Category { get; set; }

        public string? GlyphCode { get; set; }

        public bool IsGlyph => !string.IsNullOrWhiteSpace(GlyphCode);

        public static implicit operator KeyDefinition(OryxKeyDefinition oryxKey)
        {
            return new KeyDefinition
            {
                KeyCode = oryxKey.Code,
                Label = !oryxKey.IsGlyph ? oryxKey.Label : oryxKey.GlyphCode,
                IsGlyph = oryxKey.IsGlyph,
                Tag = oryxKey.Tag,
                Category = (KeyCategory)oryxKey.Category!,
            };
        }
    }
}