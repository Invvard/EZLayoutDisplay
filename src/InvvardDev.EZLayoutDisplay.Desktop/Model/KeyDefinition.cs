using Newtonsoft.Json;

namespace InvvardDev.EZLayoutDisplay.Desktop.Model
{
    public class KeyDefinition
    {
        /// <summary>
        /// Gets the key code.
        /// </summary>
        [JsonProperty("code")]
        public string KeyCode { get; set; }

        /// <summary>
        /// Gets the key label.
        /// </summary>
        [JsonProperty("label")]
        public string Label { get; set; }
        
        /// <summary>
        /// Gets the key glyph code.
        /// </summary>
        [JsonProperty("glyph_code", NullValueHandling = NullValueHandling.Ignore)]
        public string GlyphCode { get; set; }

        /// <summary>
        /// Gets indicator that this key is a glyph or not.
        /// </summary>
        [JsonIgnore]
        public bool IsGlyph => !string.IsNullOrEmpty(GlyphCode);
    }
}