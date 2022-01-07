namespace InvvardDev.EZLayoutDisplay.Desktop.Model.Enum
{
    public enum KeyDisplayType
    {
        /// <summary>
        /// Nothing to display (KC_TRANSPARENT, KC_NO).
        /// </summary>
        None = 0,

        /// <summary>
        /// Display a Tap label.
        /// </summary>
        Tap,

        /// <summary>
        /// Display a Hold label.
        /// </summary>
        Hold,

        /// <summary>
        /// Display a custom label.
        /// </summary>
        CustomLabel,

        /// <summary>
        /// Display Tap & Hold label.
        /// </summary>
        TapHold,

        TapMod,
        HoldMod,
        TapModHold,
        TapHoldMod,
        TapModHoldMod,
    }
}