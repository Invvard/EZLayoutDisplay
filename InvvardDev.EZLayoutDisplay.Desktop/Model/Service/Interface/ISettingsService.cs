namespace InvvardDev.EZLayoutDisplay.Desktop.Model.Service.Interface
{
    public interface ISettingsService
    {
        /// <summary>
        /// Gets or sets the Hotkey Show Layout.
        /// </summary>
        Hotkey HotkeyShowLayout { get; set; }

        /// <summary>
        /// Gets or sets the Ergodox Layout URL.
        /// </summary>
        string ErgodoxLayoutUrl { get; set; }

        /// <summary>
        /// Saves all settings to file.
        /// </summary>
        void Save();
    }
}