using System.Collections.Generic;

namespace InvvardDev.EZLayoutDisplay.Desktop.Model {
    public class Revision
    {
        /// <summary>
        /// Gets or sets the revision hash identifier.
        /// </summary>
        public string HashId { get; set; }

        /// <summary>
        /// Gets or sets the keyboard model.
        /// </summary>
        public string Model { get; set; }
        
        /// <summary>
        /// Gets or sets the list of <see cref="Layer"/>.
        /// </summary>
        public List<Layer> Layers { get; set; }
    }
}