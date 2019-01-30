using System.Collections.Generic;
using InvvardDev.EZLayoutDisplay.Desktop.Model.Enum;

namespace InvvardDev.EZLayoutDisplay.Desktop.Model.Dictionary
{
    public class KeyModifierDictionary
    {
        public List<EZModifier> EZModifiers { get; private set; }

        public KeyModifierDictionary()
        {
            InitializeKeyModifiers();
        }

        private void InitializeKeyModifiers()
        {
            EZModifiers = new List<EZModifier> {
                                                   new EZModifier(KeyModifier.LeftAlt, 0, "A", "ALT", "Alt"),
                                                   new EZModifier(KeyModifier.LeftCtrl, 1, "C", "CTL", "Ctrl"),
                                                   new EZModifier(KeyModifier.LeftWin, 2, "W", "WIN", "Win"),
                                                   new EZModifier(KeyModifier.LeftShift, 3, "S", "SFT", "Shift"),
                                                   new EZModifier(KeyModifier.RightAlt, 4, "A", "ALT", "Alt"),
                                                   new EZModifier(KeyModifier.RightCtrl, 5, "C", "CTL", "Ctrl"),
                                                   new EZModifier(KeyModifier.RightWin, 6, "W", "WIN", "Win"),
                                                   new EZModifier(KeyModifier.RightShift, 7, "S", "SFT", "Shift")
                                               };

        }
    }
}