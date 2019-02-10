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
        /// Display a label with a modifier under.
        /// </summary>
        ModifierUnder = 2,

        /// <summary>
        /// Display a label with a modifier on top.
        /// </summary>
        ModifierOnTop = 3
    }
}