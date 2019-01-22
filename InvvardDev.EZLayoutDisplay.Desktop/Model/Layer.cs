using System.Collections.Generic;

namespace InvvardDev.EZLayoutDisplay.Desktop.Model {
    public class Layer
    {
        /// <summary>
        /// Gets or sets the layer hash identifier.
        /// </summary>
        public string HashId { get; set; }

        /// <summary>
        /// Gets or sets the layer's list of keys.
        /// </summary>
        public List<Key> Keys { get; set; }

        /// <summary>
        /// Gets or sets the layer's position.
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// Gets or sets the layer's title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the layer's color.
        /// </summary>
        public string Color { get; set; }
    }
}