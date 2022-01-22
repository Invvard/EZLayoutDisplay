using Newtonsoft.Json;

namespace InvvardDev.EZLayoutDisplay.Desktop.Model.ZsaModels
{
    public class ErgodoxKeyFeature
    {
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
        [JsonProperty("macro", NullValueHandling = NullValueHandling.Ignore)]
        public object Macro { get; set; }

        /// <summary>
        /// Gets or sets the modifiers.
        /// </summary>
        [JsonProperty("modifiers", NullValueHandling = NullValueHandling.Ignore)]
        public ErgodoxModifiers Modifiers { get; set; }

    }
}
