using InvvardDev.EZLayoutDisplay.Desktop.Model.Enum;

namespace InvvardDev.EZLayoutDisplay.Desktop.Model
{
    public class EZKey
    {
        /// <summary>
        /// Gets or sets the key position on layout.
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// Gets or sets the key main label.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets the key secondary label.
        /// </summary>
        public string SubLabel { get; set; }

        /// <summary>
        /// Gets or sets the key glowing color.
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// Gets or sets the key category.
        /// </summary>
        public KeyCategory KeyCategory { get; set; }
    }
}