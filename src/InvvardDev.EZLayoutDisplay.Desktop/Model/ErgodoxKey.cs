using Newtonsoft.Json;

namespace InvvardDev.EZLayoutDisplay.Desktop.Model
{
    public class ErgodoxKey
    {
        /// <summary>
        /// Gets or sets the Tap key feature.
        /// </summary>
        [JsonProperty("tap")]
        public ErgodoxKeyFeature Tap { get; set; }

        /// <summary>
        /// Gets or sets the Hold key feature.
        /// </summary>
        [JsonProperty("hold")]
        public ErgodoxKeyFeature Hold { get; set; }

        /// <summary>
        /// Gets or sets the Tap-Hold key feature.
        /// </summary>
        [JsonProperty("tapHold")]
        public ErgodoxKeyFeature TapHold { get; set; }

        /// <summary>
        /// Gets or sets the Double Tap key feature.
        /// </summary>
        [JsonProperty("doubleTap")]
        public ErgodoxKeyFeature DoubleTap { get; set; }

        /// <summary>
        /// Gets or sets the GlowColor.
        /// </summary>
        [JsonProperty("glowColor", NullValueHandling = NullValueHandling.Ignore)]
        public string GlowColor { get; set; }

        /// <summary>
        /// Gets or sets the custom label.
        /// </summary>
        [JsonProperty("customLabel", NullValueHandling = NullValueHandling.Ignore)]
        public string CustomLabel { get; set; }
    }
}