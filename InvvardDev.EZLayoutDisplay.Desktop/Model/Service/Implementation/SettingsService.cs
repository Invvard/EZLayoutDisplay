using System.ComponentModel;
using InvvardDev.EZLayoutDisplay.Desktop.Model.Enum;
using InvvardDev.EZLayoutDisplay.Desktop.Model.Service.Interface;
using InvvardDev.EZLayoutDisplay.Desktop.Properties;
using Newtonsoft.Json;

namespace InvvardDev.EZLayoutDisplay.Desktop.Model.Service.Implementation
{
    public class SettingsService : ISettingsService
    {
        #region Fields

        private readonly Settings _settings;
        private readonly TypeConverter _hotkeyConverter;

        #endregion

        #region Constructor

        public SettingsService(Settings settings)
        {
            _settings = settings;
            _hotkeyConverter = TypeDescriptor.GetConverter(typeof(Hotkey));
        }

        #endregion

        #region Private methods

        #endregion

        #region ISettingService implementation

        #region Properties

        /// <inheritdoc />
        public Hotkey HotkeyShowLayout
        {
            get
            {
                var hotkey = JsonConvert.DeserializeObject<Hotkey>((string) _settings[SettingsName.HotkeyShowLayout]);

                return hotkey;
            }
            set
            {
                var setting = _hotkeyConverter.ConvertToString(value);
                _settings[SettingsName.HotkeyShowLayout] = setting;
            }
        }

        /// <inheritdoc />
        public string ErgodoxLayoutUrl
        {
            get => (string)_settings[SettingsName.ErgodoxLayoutUrl];
            set => _settings[SettingsName.ErgodoxLayoutUrl] = value;
        }

        #endregion

        /// <inheritdoc />
        public void Save()
        {
            _settings.Save();
        }

        /// <inheritdoc />
        public void Cancel()
        {
            _settings.Reload();
        }

        #endregion
    }
}