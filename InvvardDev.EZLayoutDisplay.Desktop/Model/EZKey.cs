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
        /// Gets or sets the Glyph name.
        /// </summary>
        public string GlyphName { get; set; }

        /// <summary>
        /// Gets or sets the key glowing color.
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// Gets or sets the key category.
        /// </summary>
        public KeyCategory KeyCategory { get; set; }

        /// <summary>
        /// Gets or sets the indicator wether the <see cref="SubLabel"/>
        /// is below or above <see cref="Label"/>.
        /// </summary>
        public bool IsSubLabelBelow { get; set; }

        /// <summary>
        /// Gets the indicator if <see cref="Label"/> must be displayed.
        /// </summary>
        public bool IsLabelDisplayed => string.IsNullOrWhiteSpace(GlyphName);
    }
}