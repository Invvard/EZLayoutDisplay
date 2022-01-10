using System.Collections.Generic;
using InvvardDev.EZLayoutDisplay.Desktop.Model.Enum;

namespace InvvardDev.EZLayoutDisplay.Desktop.Model.Dictionary
{
    public class KeyModifierDictionary
    {
        public Dictionary<KeyModifier, EZModifier> EZModifiers { get; private set; }

        public KeyModifierDictionary()
        {
            EZModifiers = new Dictionary<KeyModifier, EZModifier> {
                { KeyModifier.LeftAlt, new EZModifier(0, "A", "ALT", "Alt") },
                { KeyModifier.LeftCtrl, new EZModifier(1, "C", "CTL", "Ctrl") },
                { KeyModifier.LeftWin, new EZModifier(2, "W", "WIN", "Win") },
                { KeyModifier.LeftShift, new EZModifier(3, "S", "SFT", "Shift") },
                { KeyModifier.RightAlt, new EZModifier(4, "A", "ALT", "Alt") },
                { KeyModifier.RightCtrl, new EZModifier(5, "C", "CTL", "Ctrl") },
                { KeyModifier.RightWin, new EZModifier(6, "W", "WIN", "Win") },
                { KeyModifier.RightShift, new EZModifier(7, "S", "SFT", "Shift")}
            };
        }
    }
}