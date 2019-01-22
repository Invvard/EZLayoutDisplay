using System.Collections.Generic;

namespace InvvardDev.EZLayoutDisplay.Desktop.Model
{
    public class Layout
    {
        /// <summary>
        /// Gets or sets the layout hash identifier.
        /// </summary>
        public string HashId { get; set; }

        /// <summary>
        /// Gets or sets the layout title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the list of <see cref="Revision"/>.
        /// </summary>
        public List<Revision> Revisions { get; set; }
    }
}