using System.Collections.Generic;
using InvvardDev.EZLayoutDisplay.Desktop.Model.Enum;

namespace InvvardDev.EZLayoutDisplay.Desktop.Model.Dictionary
{
    internal class KeyCategoryDictionary
    {
        /// <summary>
        /// Gets the list of <see cref="KeyCategoryDetail"/>.
        /// </summary>
        public List<KeyCategoryDetail> KeyCategoryDetails { get; private set; }

        public KeyCategoryDictionary()
        {
            InitializeKeyCategoryDetails();
        }

        private void InitializeKeyCategoryDetails()
        {
            KeyCategoryDetails = new List<KeyCategoryDetail> {
                                                                 new KeyCategoryDetail(KeyCategory.AutoShift, "Auto shift"),
                                                                 new KeyCategoryDetail(KeyCategory.Digit, "Digits"),
                                                                 new KeyCategoryDetail(KeyCategory.Letters, "Letters"),
                                                                 new KeyCategoryDetail(KeyCategory.DualFunction, "Dual-function keys"),
                                                                 new KeyCategoryDetail(KeyCategory.Fn, "Fn keys"),
                                                                 new KeyCategoryDetail(KeyCategory.Fw, "Firmware"),
                                                                 new KeyCategoryDetail(KeyCategory.Lang, "Language"),
                                                                 new KeyCategoryDetail(KeyCategory.Layer, "Layer switch"),
                                                                 new KeyCategoryDetail(KeyCategory.LayerShortcuts, "Layer Shortcuts"),
                                                                 new KeyCategoryDetail(KeyCategory.Media, "Media"),
                                                                 new KeyCategoryDetail(KeyCategory.Modifier, "Modifiers"),
                                                                 new KeyCategoryDetail(KeyCategory.Momentary, "Momentary layer switch"),
                                                                 new KeyCategoryDetail(KeyCategory.Mouse, "Mouse control"),
                                                                 new KeyCategoryDetail(KeyCategory.Nav, "Navigational"),
                                                                 new KeyCategoryDetail(KeyCategory.French, "French"),
                                                                 new KeyCategoryDetail(KeyCategory.German, "German"),
                                                                 new KeyCategoryDetail(KeyCategory.Spanish, "Spanish"),
                                                                 new KeyCategoryDetail(KeyCategory.NumPad, "Num pad"),
                                                                 new KeyCategoryDetail(KeyCategory.Other, "Other"),
                                                                 new KeyCategoryDetail(KeyCategory.Punct, "Punctuation"),
                                                                 new KeyCategoryDetail(KeyCategory.ShiftedPunct, "Shifted punctuation"),
                                                                 new KeyCategoryDetail(KeyCategory.Shine, "EZ Shine control"),
                                                                 new KeyCategoryDetail(KeyCategory.Shortcuts, "Shortcuts"),
                                                                 new KeyCategoryDetail(KeyCategory.Spacing, "Spacing"),
                                                                 new KeyCategoryDetail(KeyCategory.Special, "Special Chars"),
                                                                 new KeyCategoryDetail(KeyCategory.System, "System control"),
                                                                 new KeyCategoryDetail(KeyCategory.Toggle, "Toggle layer")
                                                             };
        }
    }
}