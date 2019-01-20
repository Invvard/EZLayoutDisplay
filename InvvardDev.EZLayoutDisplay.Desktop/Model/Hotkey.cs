using System.Collections.Generic;
using Newtonsoft.Json;
using NonInvasiveKeyboardHookLibrary;

namespace InvvardDev.EZLayoutDisplay.Desktop.Model
{
    public class Hotkey
    {
        /// <summary>
        /// Gets or sets the list of <see cref="ModifierKeys"/>.
        /// </summary>
        [JsonProperty("modifiers")]
        //[JsonConverter(typeof(ModifiersConverter))]
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

        public Hotkey()
        {
            
        }
    }
}