using System.Collections.Generic;

namespace InvvardDev.EZLayoutDisplay.Desktop.Model.Enum
{
    public class KeyCategoryDetails
    {
        public List<KeyCategoryDetail> CategoryDetails { get; private set; }

        public KeyCategoryDetails()
        {
            InitializeCategoryDetails();
        }

        private void InitializeCategoryDetails()
        {
            CategoryDetails = new List<KeyCategoryDetail> {
                                                              new KeyCategoryDetail(KeyCategory.Autoshift, "Auto shift"),
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
                                                              new KeyCategoryDetail(KeyCategory.Nordic, "Nordic"),
                                                              new KeyCategoryDetail(KeyCategory.Numpad, "Numpad"),
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

        public struct KeyCategoryDetail
        {
            public KeyCategoryDetail(KeyCategory category, string title)
            {
                Category = category;
                Title = title;
            }

            public KeyCategory Category { get; private set; }
            public string Title { get; private set; }
        }
    }
}