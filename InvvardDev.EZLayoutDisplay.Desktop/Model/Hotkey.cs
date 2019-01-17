using System;
using System.Collections.Generic;
using NonInvasiveKeyboardHookLibrary;

namespace InvvardDev.EZLayoutDisplay.Desktop.Model
{
    [Serializable]
    public class Hotkey
    {
        /// <summary>
        /// Gets or sets the list of <see cref="ModifierKeys"/>.
        /// </summary>
        public List<ModifierKeys> ModifierKeys { get; set; }

        /// <summary>
        /// Gets or sets the complementary key code.
        /// </summary>
        public string KeyCode { get; set; }
    }
}