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
                var hotkey = _hotkeyConverter.ConvertFromString((string)_settings[SettingsName.HotkeyShowLayout]) as Hotkey;

                return hotkey;
            }
            set
            {
                var setting = _hotkeyConverter.ConvertToString(value);
                if (setting != null && (string)_settings[SettingsName.HotkeyShowLayout] == setting) return;

                IsDirty = true;
                _settings[SettingsName.HotkeyShowLayout] = setting;
            }
        }

        /// <inheritdoc />
        public string ErgodoxLayoutUrl
        {
            get => (string)_settings[SettingsName.ErgodoxLayoutUrl];
            set
            {
                if ((string)_settings[SettingsName.ErgodoxLayoutUrl] == value) return;

                IsDirty = true;
                _settings[SettingsName.ErgodoxLayoutUrl] = value;
            }
        }

        /// <inheritdoc />
        public bool IsDirty { get; private set; }

        #endregion

        /// <inheritdoc />
        public void Save()
        {
            _settings.Save();
            IsDirty = false;
        }

        /// <inheritdoc />
        public void Cancel()
        {
            _settings.Reload();
            IsDirty = false;
        }

        #endregion
    }
}