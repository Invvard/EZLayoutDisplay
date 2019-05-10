using Newtonsoft.Json;

namespace InvvardDev.EZLayoutDisplay.Desktop.Model
{
    public class ErgodoxTag
    {
        /// <summary>
        /// Gets or sets the tag name.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}