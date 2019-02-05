namespace InvvardDev.EZLayoutDisplay.Desktop.Model.Enum
{
    public enum KeyDisplayType
    {
        /// <summary>
        /// Nothing to display (KC_TRANSPARENT).
        /// </summary>
        None = 0,

        /// <summary>
        /// Display the main label.
        /// </summary>
        SimpleLabel = 1,

        /// <summary>
        /// Display a label with the international indicator at the top right.
        /// </summary>
        LabelInternational = 2,

        /// <summary>
        /// Display a label with a sub label under.
        /// </summary>
        LabelWithSubLabelUnder = 3,

        /// <summary>
        /// Display a label with a sub label on top.
        /// </summary>
        LabelWithSubLabelOnTop = 4,

        /// <summary>
        /// Display a glyph and no label.
        /// </summary>
        Glyph = 5,
    }
}