using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace InvvardDev.EZLayoutDisplay.Desktop.Model
{
    public class Revision
    {
        /// <summary>
        /// Gets or sets the revision hash identifier.
        /// </summary>
        [JsonProperty("hashId")]
        public string HashId { get; set; }

        /// <summary>
        /// Gets or sets the keyboard layout HEX file URL.
        /// </summary>
        [JsonProperty("hexUrl")]
        public Uri HexUrl { get; set; }

        /// <summary>
        /// Gets or sets the keyboard layout source zip URL.
        /// </summary>
        [JsonProperty("zipUrl")]
        public Uri SourceUrl { get; set; }

        /// <summary>
        /// Gets or sets the keyboard model.
        /// </summary>
        [JsonProperty("model")]
        public string Model { get; set; }

        /// <summary>
        /// Gets or sets the list of <see cref="ErgodoxLayer"/>.
        /// </summary>
        [JsonProperty("layers")]
        public List<ErgodoxLayer> Layers { get; set; }
    }
}