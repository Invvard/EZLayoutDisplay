using System;
using System.Windows.Forms;
using InvvardDev.EZLayoutDisplay.Desktop.Model;
using InvvardDev.EZLayoutDisplay.Desktop.Model.Enum;
using InvvardDev.EZLayoutDisplay.Desktop.Properties;
using InvvardDev.EZLayoutDisplay.Desktop.Service.Interface;
using Newtonsoft.Json;
using NLog;
using ModifierKeys = NonInvasiveKeyboardHookLibrary.ModifierKeys;

namespace InvvardDev.EZLayoutDisplay.Desktop.Service.Implementation
{
    public class SettingsService : ISettingsService
    {
        #region Constants

        private readonly Hotkey _defaultHotkey;

        #endregion

        #region Fields

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
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
                    var setting = (string) _settings[SettingsName.HotkeyShowLayout];

                    hotkey = string.IsNullOrWhiteSpace(setting)
                                 ? _defaultHotkey
                                 : JsonConvert.DeserializeObject<Hotkey>(setting);
                }
                catch (Exception) { hotkey = _defaultHotkey; }

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

        /// <inheritdoc />
        public EZLayout EZLayout
        {
            get
            {
                var setting = (string) _settings[SettingsName.EZLayout];
                var ezLayout = string.IsNullOrWhiteSpace(setting)
                                   ? new EZLayout()
                                   : JsonConvert.DeserializeObject<EZLayout>(setting);

                return ezLayout;
            }

            set
            {
                var setting = JsonConvert.SerializeObject(value);
                _settings[SettingsName.EZLayout] = setting;
            }
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