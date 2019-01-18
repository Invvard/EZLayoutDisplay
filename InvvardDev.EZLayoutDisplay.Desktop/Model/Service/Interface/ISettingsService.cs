namespace InvvardDev.EZLayoutDisplay.Desktop.Model.Service.Interface
{
    public interface ISettingsService
    {
        /// <summary>
        /// Gets or sets the HotkeyShowLayout setting.
        /// </summary>
        Hotkey HotkeyShowLayout { get; set; }

        /// <summary>
        /// Gets or sets the ErgodoxLayoutUrl setting.
        /// </summary>
        string ErgodoxLayoutUrl { get; set; }
        
        /// <summary>
        /// Saves all settings to file.
        /// </summary>
        void Save();

        /// <summary>
        /// Cancel settings edition.
        /// </summary>
        void Cancel();
    }
}