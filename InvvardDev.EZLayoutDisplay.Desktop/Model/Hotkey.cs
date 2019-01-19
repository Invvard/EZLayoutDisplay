using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using InvvardDev.EZLayoutDisplay.Desktop.Helper;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NonInvasiveKeyboardHookLibrary;

namespace InvvardDev.EZLayoutDisplay.Desktop.Model
{
    [TypeConverter(typeof(HotkeyConverter))]
    [SettingsSerializeAs(SettingsSerializeAs.String)]
    public class Hotkey
    {
        /// <summary>
        /// Gets or sets the list of <see cref="ModifierKeys"/>.
        /// </summary>
        [JsonProperty("modifiers")]
        [JsonConverter(typeof(StringEnumConverter))]
        public List<ModifierKeys> ModifierKeys { get; set; }

        /// <summary>
        /// Gets or sets the complementary key code.
        /// </summary>
        [JsonProperty("keycode")]
        public int KeyCode { get; set; }

        public Hotkey(int keyCode, params ModifierKeys[] modifiers)
        {
            KeyCode = keyCode;
            ModifierKeys = new List<ModifierKeys>();
            ModifierKeys.AddRange(modifiers);
        }
    }
}