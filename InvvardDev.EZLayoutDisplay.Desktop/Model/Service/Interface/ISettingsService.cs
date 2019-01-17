using System;

namespace InvvardDev.EZLayoutDisplay.Desktop.Model.Service.Interface
{
    public interface ISettingsService
    {
        /// <summary>
        /// Gets the Show Layout Hotkey.
        /// </summary>
        /// <returns>The hotkey from Settings.</returns>
        Hotkey GetHotKeyShowLayout();
        
        /// <summary>
        /// Gets the Ergodox Layout URL.
        /// </summary>
        /// <returns>The URL to the Ergodox Layout.</returns>
        string GetErgodoxLayoutUrl();
    }
}