using InvvardDev.EZLayoutDisplay.Desktop.Model.Enum;

namespace InvvardDev.EZLayoutDisplay.Desktop.Model
{
    public class EZKey
    {
        /// <summary>
        /// Gets or sets the key main label.
        /// </summary>
        public KeyLabel Label { get; set; }

        /// <summary>
        /// Gets or sets the key secondary label.
        /// </summary>
        public KeyLabel Modifier { get; set; }

        /// <summary>
        /// Gets or sets the key glowing color.
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// Gets or sets the key international hint.
        /// </summary>
        public string InternationalHint { get; set; }

        /// <summary>
        /// Gets or sets the key category.
        /// </summary>
        public KeyCategory KeyCategory { get; set; }

        /// <summary>
        /// Gets or sets the type of display for this key.
        /// </summary>
        public KeyDisplayType DisplayType { get; set; }
    }

    public class KeyLabel
    {
        public string Content { get; set; }

        public bool IsGlyph { get; set; }

        public KeyLabel(string content, bool isGlyph = false)
        {
            Content = content;
            IsGlyph = isGlyph;
        }
    }
}