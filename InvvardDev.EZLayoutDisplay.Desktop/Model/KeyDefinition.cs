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
        public string KeyCode { get; private set; }

        /// <summary>
        /// Gets the key label.
        /// </summary>
        [JsonProperty("label")]
        public string Label { get; private set; }

        /// <summary>
        /// Gets the key category.
        /// </summary>
        [JsonProperty("category")]
        public KeyCategory KeyCategory { get; private set; }

        /// <summary>
        /// Gets the key description.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; private set; }

        /// <summary>
        /// Gets the key secondary command.
        /// </summary>
        [JsonProperty("commad")]
        public KeyDefinition SecondaryCommand { get; private set; }

        /// <summary>
        /// Gets the key glyph name to display.
        /// </summary>
        [JsonProperty("glyph")]
        public string GlyphName { get; private set; }

        /// <summary>
        /// Gets the key hide label indicator (Default = false).
        /// </summary>
        [JsonProperty("hideLabel")]
        public bool HideLabel { get; private set; }

        /// <summary>
        /// Gets the key preceding key indicator (blocks the targeted layer).
        /// </summary>
        [JsonProperty("precedingKey")]
        public bool PrecedingKey { get; private set; }

        #endregion
    }
}