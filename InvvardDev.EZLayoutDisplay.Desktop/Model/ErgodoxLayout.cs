using System.Collections.Generic;
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
        /// Gets or sets the layout title.
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the list of <see cref="Revision"/>.
        /// </summary>
        [JsonProperty("revisions")]
        public List<Revision> Revisions { get; set; }
    }
}