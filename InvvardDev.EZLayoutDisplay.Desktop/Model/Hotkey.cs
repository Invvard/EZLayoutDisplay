using System.Collections.Generic;
using System.Windows.Documents;
using NonInvasiveKeyboardHookLibrary;

namespace InvvardDev.EZLayoutDisplay.Desktop.Model
{
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