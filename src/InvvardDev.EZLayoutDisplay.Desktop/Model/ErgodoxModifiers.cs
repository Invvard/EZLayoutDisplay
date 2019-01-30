using Newtonsoft.Json;

namespace InvvardDev.EZLayoutDisplay.Desktop.Model
{
    public class ErgodoxModifiers
    {
        /// <summary>
        /// Gets or sets the Left Alt modifier indicator.
        /// </summary>
        [JsonProperty("leftAlt")]
        public bool LeftAlt { get; set; }

        /// <summary>
        /// Gets or sets the Left Cmd/Win modifier indicator.
        /// </summary>
        [JsonProperty("leftGui")]
        public bool LeftWin { get; set; }

        /// <summary>
        /// Gets or sets the Left Ctrl modifier indicator.
        /// </summary>
        [JsonProperty("leftCtrl")]
        public bool LeftCtrl { get; set; }

        /// <summary>
        /// Gets or sets the Right Alt modifier indicator.
        /// </summary>
        [JsonProperty("rightAlt")]
        public bool RightAlt { get; set; }

        /// <summary>
        /// Gets or sets the Right Cmd/Win modifier indicator.
        /// </summary>
        [JsonProperty("rightGui")]
        public bool RightWin { get; set; }

        /// <summary>
        /// Gets or sets the Left Shift modifier indicator.
        /// </summary>
        [JsonProperty("leftShift")]
        public bool LeftShift { get; set; }

        /// <summary>
        /// Gets or sets the Right Ctrl modifier indicator.
        /// </summary>
        [JsonProperty("rightCtrl")]
        public bool RightCtrl { get; set; }

        /// <summary>
        /// Gets or sets the Right Shift modifier indicator.
        /// </summary>
        [JsonProperty("rightShift")]
        public bool RightShift { get; set; }
    }
}