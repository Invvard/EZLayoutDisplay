using InvvardDev.EZLayoutDisplay.Desktop.Model.Ez;
using System.Collections.Generic;

namespace InvvardDev.EZLayoutDisplay.Desktop.Model
{
    public class EZLayer
    {
        /// <summary>
        /// Gets or sets the layer index.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Gets or sets the layer name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the layer list of keys.
        /// </summary>
        public List<Key> Keys { get; set; }

        /// <summary>
        /// Gets or sets the layer color.
        /// </summary>
        public string Color { get; set; }

        public EZLayer()
        {
            Keys = new List<Key>();
        }
    }
}