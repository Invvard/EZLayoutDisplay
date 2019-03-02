using InvvardDev.EZLayoutDisplay.Desktop.Model.Enum;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace InvvardDev.EZLayoutDisplay.Desktop.Model
{
    public class KeyDefinition
    {
        #region Properties

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
        /// Gets the key category.
        /// </summary>
        [JsonProperty("category")]
        [JsonConverter(typeof(StringEnumConverter))]
        public KeyCategory KeyCategory { get; set; }

        /// <summary>
        /// Gets the key description.
        /// </summary>
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        /// <summary>
        /// Gets the key secondary command.
        /// </summary>
        [JsonProperty("command", NullValueHandling = NullValueHandling.Ignore)]
        public string SecondaryCommand { get; set; }

        /// <summary>
        /// Gets the key glyph name to display.
        /// </summary>
        [JsonProperty("isglyph", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool IsGlyph { get; set; }

        /// <summary>
        /// Gets the key preceding key indicator (blocks the targeted layer).
        /// </summary>
        [JsonProperty("precedingKey", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool PrecedingKey { get; set; }

        #endregion
    }
}