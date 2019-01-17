using InvvardDev.EZLayoutDisplay.Desktop.Model.Service.Interface;
using InvvardDev.EZLayoutDisplay.Desktop.Properties;

namespace InvvardDev.EZLayoutDisplay.Desktop.Model.Service.Implementation
{
    internal class SettingsService : ISettingsService
    {
        private readonly Settings _settings;

        public SettingsService(Settings settings)
        {
            _settings = settings;
        }

        /// <inheritdoc />
        public void UpdateSetting(string key, string value)
        {
            _settings[key] = value;
        }

        /// <inheritdoc/>
        public string GetSetting(string key)
        {
            var settingValue = _settings[key]?.ToString();

            return settingValue;
        }
    }
}