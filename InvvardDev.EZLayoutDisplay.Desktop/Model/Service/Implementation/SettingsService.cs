using System;
using System.Configuration;
using InvvardDev.EZLayoutDisplay.Desktop.Model.Enum;
using InvvardDev.EZLayoutDisplay.Desktop.Model.Service.Interface;
using InvvardDev.EZLayoutDisplay.Desktop.Properties;

namespace InvvardDev.EZLayoutDisplay.Desktop.Model.Service.Implementation
{
    internal class SettingsService : ApplicationSettingsBase, ISettingsService
    {
        #region Fields

        private readonly Settings _settings;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the HotkeyShowLayout setting.
        /// </summary>
        [ UserScopedSetting ]
        [ SettingsSerializeAs(SettingsSerializeAs.Xml) ]
        [ DefaultSettingValue("") ]
        private Hotkey HotkeyShowLayout
        {
            get => (Hotkey) this[SettingsName.HotkeyShowLayout];
            set => this[SettingsName.HotkeyShowLayout] = value;
        }

        /// <summary>
        /// Gets or sets the HotkeyShowLayout setting.
        /// </summary>
        [ UserScopedSetting ]
        [ SettingsSerializeAs(SettingsSerializeAs.Xml) ]
        [ DefaultSettingValue("https://configure.ergodox-ez.com/layouts/default/latest/0") ]
        private string ErgodoxLayoutUrl
        {
            get => (string) this[SettingsName.ErgodoxLayoutUrl];
            set => this[SettingsName.ErgodoxLayoutUrl] = value;
        }

        #endregion

        #region Constructor

        public SettingsService()
        {
            //_settings = settings;
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