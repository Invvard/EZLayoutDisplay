namespace InvvardDev.EZLayoutDisplay.Desktop.Model.Enum
{
    public enum KeyDisplayType
    {
        /// <summary>
        /// Nothing to display (KC_TRANSPARENT, KC_NO).
        /// </summary>
        None = 0,

        /// <summary>
        /// Display the main label.
        /// </summary>
        SimpleLabel = 1,

        /// <summary>
        /// Display a label with a sub label under.
        /// </summary>
        LabelWithSubLabelUnder = 2,

        /// <summary>
        /// Display a label with a sub label on top.
        /// </summary>
        LabelWithSubLabelOnTop = 3,

        /// <summary>
        /// Display a glyph and no label.
        /// </summary>
        SimpleGlyph = 4,

        /// <summary>
        /// Display a glyph with sub label under.
        /// </summary>
        GlyphWithSubLabelUnder = 5,

        /// <summary>
        /// Display a glyph with sub label on top.
        /// </summary>
        GlyphWithSubLabelOnTop = 6,

        /// <summary>
        /// Display a label with the international indicator at the top right.
        /// </summary>
        LabelInternational = 7,
    }
}