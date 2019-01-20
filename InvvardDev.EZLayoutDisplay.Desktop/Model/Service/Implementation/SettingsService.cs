using System.Windows.Forms;
using System.Windows.Input;
using InvvardDev.EZLayoutDisplay.Desktop.Model.Enum;
using InvvardDev.EZLayoutDisplay.Desktop.Model.Service.Interface;
using InvvardDev.EZLayoutDisplay.Desktop.Properties;
using Newtonsoft.Json;
using ModifierKeys = NonInvasiveKeyboardHookLibrary.ModifierKeys;

namespace InvvardDev.EZLayoutDisplay.Desktop.Model.Service.Implementation
{
    public class SettingsService : ISettingsService
    {
        #region Constants

        private readonly Hotkey _defaultHotkey;

        #endregion

        #region Fields

        private readonly Settings _settings;

        #endregion

        #region Constructor

        public SettingsService(Settings settings)
        {
            _settings = settings;
            _defaultHotkey = new Hotkey(Keys.Space, ModifierKeys.Alt, ModifierKeys.Control, ModifierKeys.Shift, ModifierKeys.WindowsKey);
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
                Hotkey hotkey;
                try
                {
                    var setting = (string)_settings[SettingsName.HotkeyShowLayout];

                    hotkey = string.IsNullOrWhiteSpace(setting)
                                 ? _defaultHotkey
                                 : JsonConvert.DeserializeObject<Hotkey>(setting);
                }
                catch (System.Exception)
                {
                    hotkey = _defaultHotkey;
                }
                return hotkey;
            }
            set
            {
                var setting = JsonConvert.SerializeObject(value);
                _settings[SettingsName.HotkeyShowLayout] = setting;
            }
        }

        /// <inheritdoc />
        public string ErgodoxLayoutUrl
        {
            get => (string) _settings[SettingsName.ErgodoxLayoutUrl];
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