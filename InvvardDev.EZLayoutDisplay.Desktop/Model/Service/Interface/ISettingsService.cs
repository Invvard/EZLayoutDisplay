namespace InvvardDev.EZLayoutDisplay.Desktop.Model.Service.Interface
{
    public interface ISettingsService
    {
        /// <summary>
        /// Updates a setting value.
        /// </summary>
        /// <param name="key">Key to target.</param>
        /// <param name="value">Value to update.</param>
        void UpdateSetting(string key, string value);

        /// <summary>
        /// Gets a setting value.
        /// </summary>
        /// <param name="key">Key to target.</param>
        /// <returns>The wanted value.</returns>
        string GetSetting(string key);
    }
}