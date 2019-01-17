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

        public void UpdateKey(string key, string value)
        {
            
        }
    }
}