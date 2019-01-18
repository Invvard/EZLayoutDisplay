using System.ComponentModel;
using InvvardDev.EZLayoutDisplay.Desktop.Model.Enum;
using InvvardDev.EZLayoutDisplay.Desktop.Model.Service.Interface;
using InvvardDev.EZLayoutDisplay.Desktop.Properties;

namespace InvvardDev.EZLayoutDisplay.Desktop.Model.Service.Implementation
{
    public class SettingsService : ISettingsService
    {
        #region Fields

        private readonly Settings _settings;
        private readonly TypeConverter _hotkeyConverter;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the HotkeyShowLayout setting.
        /// </summary>
        public Hotkey HotkeyShowLayout
        {
            get
            {
                var hotkey = _hotkeyConverter.ConvertFromString((string)_settings[SettingsName.HotkeyShowLayout]) as Hotkey;

                return hotkey;
            }
            set => _settings[SettingsName.HotkeyShowLayout] = value;
        }

        /// <summary>
        /// Gets or sets the ErgodoxLayoutUrl setting.
        /// </summary>
        public string ErgodoxLayoutUrl
        {
            get => (string)_settings[SettingsName.ErgodoxLayoutUrl];
            set => _settings[SettingsName.ErgodoxLayoutUrl] = value;
        }

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

        /// <inheritdoc />
        public Hotkey GetHotKeyShowLayout()
        {
            var hotkey = HotkeyShowLayout;

            return hotkey;
        }

        /// <inheritdoc />
        public string GetErgodoxLayoutUrl()
        {
            var url = ErgodoxLayoutUrl;

            return url;
        }

        #endregion
    }
}