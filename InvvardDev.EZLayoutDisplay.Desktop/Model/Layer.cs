using System.Collections.Generic;
using Newtonsoft.Json;

namespace InvvardDev.EZLayoutDisplay.Desktop.Model
{
    public class Layer
    {
        /// <summary>
        /// Gets or sets the layer hash identifier.
        /// </summary>
        [JsonProperty("hashId")]
        public string HashId { get; set; }

        /// <summary>
        /// Gets or sets the layer's list of <see cref="ErgodoxKey"/>.
        /// </summary>
        [JsonProperty("keys")]
        public List<ErgodoxKey> Keys { get; set; }

        /// <summary>
        /// Gets or sets the layer's position.
        /// </summary>
        [JsonProperty("position")]
        public int Position { get; set; }

        /// <summary>
        /// Gets or sets the layer's title.
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the layer's color.
        /// </summary>
        [JsonProperty("color")]
        public string Color { get; set; }
    }
}