using Newtonsoft.Json;

namespace InvvardDev.EZLayoutDisplay.Desktop.Model
{
    public class ErgodoxKey
    {
        /// <summary>
        /// Gets or sets the supported OS name.
        /// </summary>
        [JsonProperty("os", NullValueHandling = NullValueHandling.Ignore)]
        public string SupportedOsName { get; set; }

        /// <summary>
        /// Gets or sets the key code.
        /// </summary>
        [JsonProperty("code")]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the Color.
        /// </summary>
        [JsonProperty("color", NullValueHandling = NullValueHandling.Ignore)]
        public string Color { get; set; }

        /// <summary>
        /// Gets or sets the layer index.
        /// </summary>
        [JsonProperty("layer", NullValueHandling = NullValueHandling.Ignore)]
        public int? Layer { get; set; }

        /// <summary>
        /// Gets or sets the primary command.
        /// </summary>
        [JsonProperty("command", NullValueHandling = NullValueHandling.Ignore)]
        public string Command { get; set; }

        /// <summary>
        /// Gets or sets the GlowColor.
        /// </summary>
        [JsonProperty("glowColor", NullValueHandling = NullValueHandling.Ignore)]
        public string GlowColor { get; set; }

        /// <summary>
        /// Gets or sets the modifiers.
        /// </summary>
        [JsonProperty("modifiers", NullValueHandling = NullValueHandling.Ignore)]
        public Modifiers Modifiers { get; set; }
    }
}