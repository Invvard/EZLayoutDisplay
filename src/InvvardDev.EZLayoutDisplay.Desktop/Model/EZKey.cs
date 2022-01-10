using InvvardDev.EZLayoutDisplay.Desktop.Model.Enum;

namespace InvvardDev.EZLayoutDisplay.Desktop.Model
{
    public class EZKey
    {
        public KeyFeature Primary { get; set; }

        public KeyFeature Secondary { get; set; }

        /// <summary>
        /// Gets or sets the key glowing color.
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// Gets or sets the display mode for this key.
        /// </summary>
        public KeyDisplayMode DisplayMode { get; set; }
    }

   public class KeyFeature
    {
        /// <summary>
        /// Gets or sets the key main label.
        /// </summary>
        public KeyLabel Content { get; set; }

        /// <summary>
        /// Gets or sets the key modifier.
        /// </summary>
        public string Modifier { get; set; }

        /// <summary>
        /// Gets or sets the key tag.
        /// </summary>
        public string Tag { get; set; }

        public KeyFeature(string content, bool isGlyph = false, string tag = "", string modifier = "")
        {
            Content = new KeyLabel(content, isGlyph);
            Modifier = modifier;
            Tag = tag;
        }
    }

    public class KeyLabel
    {
        public string Label { get; set; }

        public bool IsGlyph { get; }

        public KeyLabel(string content, bool isGlyph = false)
        {
            Label = content;
            IsGlyph = isGlyph;
        }
    }
}