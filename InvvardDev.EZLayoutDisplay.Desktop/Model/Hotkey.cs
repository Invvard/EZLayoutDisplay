using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using InvvardDev.EZLayoutDisplay.Desktop.Helper;
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
        public List<ModifierKeys> ModifierKeys { get; set; }

        /// <summary>
        /// Gets or sets the complementary key code.
        /// </summary>
        public int KeyCode { get; set; }

        public Hotkey(int keyCode, params ModifierKeys[] modifiers)
        {
            KeyCode = keyCode;
            ModifierKeys = new List<ModifierKeys>();
            ModifierKeys.AddRange(modifiers);
        }
    }
}