using System;
using System.Windows.Forms;
using InvvardDev.EZLayoutDisplay.Desktop.Helper;
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
            Logger.TraceConstructor();

            _settings = settings;
            _defaultHotkey = new Hotkey(Keys.Space, ModifierKeys.Alt, ModifierKeys.Control, ModifierKeys.Shift, ModifierKeys.WindowsKey);
        }

        #endregion

        #region ISettingService implementation

        #region Properties

        /// <inheritdoc />
        public Hotkey HotkeyShowLayout
        {
            get
            {
                Logger.TraceMethod();
                Hotkey hotkey;

                try
                {
                    var setting = (string) _settings[SettingsName.HotkeyShowLayout];
                    Logger.Debug("SettingsName.HotkeyShowLayout value = '{setting}'", setting);

                    hotkey = string.IsNullOrWhiteSpace(setting)
                                 ? _defaultHotkey
                                 : JsonConvert.DeserializeObject<Hotkey>(setting);
                    Logger.Debug("Loaded hotkey = {@hotkey}", hotkey);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "Exception catched in '{0}' getter :\n", nameof(HotkeyShowLayout));
                    hotkey = _defaultHotkey;
                }

                Logger.DebugOutputParam(nameof(hotkey), hotkey);

                return hotkey;
            }
            set
            {
                Logger.TraceMethod();
                Logger.DebugInputParam(nameof(HotkeyShowLayout), value);

                var setting = JsonConvert.SerializeObject(value);
                _settings[SettingsName.HotkeyShowLayout] = setting;
            }
        }

        /// <inheritdoc />
        public string ErgodoxLayoutUrl
        {
            get
            {
                Logger.TraceMethod();

                var url = (string) _settings[SettingsName.ErgodoxLayoutUrl];
                Logger.DebugOutputParam(nameof(ErgodoxLayoutUrl), url);

                return url;
            }
            set
            {
                Logger.TraceMethod();
                Logger.DebugInputParam(nameof(ErgodoxLayoutUrl), value);

                _settings[SettingsName.ErgodoxLayoutUrl] = value;
            }
        }

        /// <inheritdoc />
        public EZLayout EZLayout
        {
            get
            {
                Logger.TraceMethod();
                EZLayout ezLayout;

                try
                {
                    var setting = (string) _settings[SettingsName.EZLayout];
                    Logger.Debug("SettingsName.EZLayout value = '{setting}'", setting);

                    ezLayout = string.IsNullOrWhiteSpace(setting)
                                   ? new EZLayout()
                                   : JsonConvert.DeserializeObject<EZLayout>(setting);
                    Logger.Debug("Loaded Layout = {@ezLayout}", ezLayout);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex, "Exception catched in '{0}' getter :\n", nameof(EZLayout));
                    ezLayout = new EZLayout();
                }

                Logger.DebugOutputParam(nameof(EZLayout), ezLayout);

                return ezLayout;
            }

            set
            {
                Logger.TraceMethod();
                Logger.DebugInputParam(nameof(EZLayout), value);

                var setting = JsonConvert.SerializeObject(value);
                _settings[SettingsName.EZLayout] = setting;
            }
        }

        #endregion

        /// <inheritdoc />
        public void Save()
        {
            Logger.TraceMethod();
            _settings.Save();
        }

        /// <inheritdoc />
        public void Cancel()
        {
            Logger.TraceMethod();
            _settings.Reload();
        }

        #endregion

        #region Private methods

        #endregion
    }
}