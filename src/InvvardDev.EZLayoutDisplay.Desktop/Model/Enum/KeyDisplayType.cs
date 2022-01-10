namespace InvvardDev.EZLayoutDisplay.Desktop.Model.Enum
{
    public enum KeyDisplayMode
    {
        /// <summary>
        /// Nothing to display (KC_TRANSPARENT, KC_NO).
        /// </summary>
        None = 0,

        /// <summary>
        /// Display a custom label.
        /// </summary>
        CustomLabel,

        /// <summary>
        /// Display single feature key.
        /// </summary>
        SingleFeature,
        
        /// <summary>
        /// Display Tap & Hold label.
        /// </summary>
        DoubleFeature
    }
}