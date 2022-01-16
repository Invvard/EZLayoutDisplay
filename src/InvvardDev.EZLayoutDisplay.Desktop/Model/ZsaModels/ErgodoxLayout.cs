using System.Collections.Generic;
using InvvardDev.EZLayoutDisplay.Desktop.Model.ZsaModels;
using Newtonsoft.Json;

namespace InvvardDev.EZLayoutDisplay.Desktop.Model
{
    public class ErgodoxLayout
    {
        /// <summary>
        /// Gets or sets the layout hash identifier.
        /// </summary>
        [JsonProperty("hashId")]
        public string HashId { get; set; }

        /// <summary>
        /// Gets or sets the keyboard geometry.
        /// </summary>
        [JsonProperty("geometry")]
        public string Geometry { get; set; }

        /// <summary>
        /// Gets or sets the layout title.
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the keyboard tags.
        /// </summary>
        [JsonProperty("tags", NullValueHandling = NullValueHandling.Ignore)]
        public List<ErgodoxTag> Tags { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Revision"/>.
        /// </summary>
        [JsonProperty("revision")]
        public Revision Revision { get; set; }
    }

    public class DataRoot
    {
        [JsonProperty("data")]
        public ErgodoxLayoutRoot LayoutRoot { get; set; }
    }

    public class ErgodoxLayoutRoot
    {
        [JsonProperty("Layout")]
        public ErgodoxLayout Layout { get; set; }
    }
}