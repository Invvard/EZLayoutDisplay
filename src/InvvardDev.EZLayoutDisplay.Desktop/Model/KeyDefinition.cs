using InvvardDev.EZLayoutDisplay.Desktop.Model.Enum;
using Newtonsoft.Json;

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
        public KeyCategory KeyCategory { get; set; }

        /// <summary>
        /// Gets the key description.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets the key secondary command.
        /// </summary>
        [JsonProperty("command")]
        public string SecondaryCommand { get; set; }

        /// <summary>
        /// Gets the key glyph name to display.
        /// </summary>
        [JsonProperty("isglyph")]
        public bool IsGlyph { get; set; }

        /// <summary>
        /// Gets the key preceding key indicator (blocks the targeted layer).
        /// </summary>
        [JsonProperty("precedingKey")]
        public bool PrecedingKey { get; set; }

        #endregion
    }
}