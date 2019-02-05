using System.Collections.Generic;
using System.Linq;
using InvvardDev.EZLayoutDisplay.Desktop.Helper;
using InvvardDev.EZLayoutDisplay.Desktop.Model;
using InvvardDev.EZLayoutDisplay.Desktop.Model.Enum;
using Xunit;
using Assert = Xunit.Assert;

namespace InvvardDev.EZLayoutDisplay.Tests.Helper
{
    public class EZLayoutMakerTest
    {
        private static ErgodoxLayout InitializeDataTree()
        {
            return new ErgodoxLayout {
                                         Title = "",
                                         HashId = "",
                                         Revisions = new List<Revision> {
                                                                            new Revision {
                                                                                             Layers = new List<ErgodoxLayer> {
                                                                                                                                 new ErgodoxLayer() {
                                                                                                                                                        Color = "",
                                                                                                                                                        Title = "",
                                                                                                                                                        Position = 0,
                                                                                                                                                        Keys = new List<ErgodoxKey>()
                                                                                                                                                    }
                                                                                                                             }
                                                                                         }
                                                                        }
                                     };
        }

        [ Theory ]
        [ InlineData("expectedTitle", "expectedHashId") ]
        public void PrepareEZLayout_InitializeEZLayout(string expectedTitle, string expectedHashId)
        {
            // Arrange
            var revision = new Revision {
                                            Layers = new List<ErgodoxLayer>()
                                        };
            ErgodoxLayout ergodoxLayout = new ErgodoxLayout {
                                                                Title = expectedTitle,
                                                                HashId = expectedHashId,
                                                                Revisions = new List<Revision>()
                                                            };
            ergodoxLayout.Revisions.Add(revision);
            EZLayout ezLayoutResult;

            // Act
            var ezLayoutMaker = new EZLayoutMaker();
            ezLayoutResult = ezLayoutMaker.PrepareEZLayout(ergodoxLayout);

            // Assert
            Assert.Equal(expectedTitle, ezLayoutResult.Name);
            Assert.Equal(expectedHashId, ezLayoutResult.HashId);
        }

        [ Theory ]
        [ InlineData(0, "expectedTitle", "expectedColor") ]
        public void PrepareEZLayout_InitializeEZLayer(int expectedIndex, string expectedTitle, string expectedColor)
        {
            // Arrange
            var ergodoxLayer = new ErgodoxLayer() {
                                                      Color = expectedColor,
                                                      Title = expectedTitle,
                                                      Position = expectedIndex,
                                                      Keys = new List<ErgodoxKey>()
                                                  };
            ErgodoxLayout ergodoxLayout = new ErgodoxLayout {
                                                                Title = "",
                                                                HashId = "",
                                                                Revisions = new List<Revision> {
                                                                                                   new Revision {
                                                                                                                    Layers = new List<ErgodoxLayer> {
                                                                                                                                                        ergodoxLayer
                                                                                                                                                    }
                                                                                                                }
                                                                                               }
                                                            };

            EZLayout ezLayoutResult;

            // Act
            var ezLayoutMaker = new EZLayoutMaker();
            ezLayoutResult = ezLayoutMaker.PrepareEZLayout(ergodoxLayout);

            // Assert
            Assert.Single(ezLayoutResult.EZLayers);
            Assert.Equal(expectedTitle, ezLayoutResult.EZLayers.First().Name);
            Assert.Equal(expectedIndex, ezLayoutResult.EZLayers.First().Index);
            Assert.Equal(expectedColor, ezLayoutResult.EZLayers.First().Color);
        }

        [ Theory ]
        [ InlineData("KC_TRANSPARENT", "expectedColor", 0) ]
        public void PrepareEZLayout_InitializeEZKey(string expectedKeyCode, string expectedColor, int expectedIndex)
        {
            // Arrange
            var ergodoxKey = new ErgodoxKey() {
                                                  GlowColor = expectedColor,
                                                  Code = expectedKeyCode
                                              };
            ErgodoxLayout ergodoxLayout = InitializeDataTree();
            ergodoxLayout.Revisions.First().Layers.First().Keys.Add(ergodoxKey);

            EZLayout ezLayoutResult;

            // Act
            var ezLayoutMaker = new EZLayoutMaker();
            ezLayoutResult = ezLayoutMaker.PrepareEZLayout(ergodoxLayout);

            // Assert
            Assert.Single(ezLayoutResult.EZLayers);
            Assert.Single(ezLayoutResult.EZLayers.First().EZKeys);
            Assert.Equal(expectedColor, ezLayoutResult.EZLayers.First().EZKeys.First().Color);
            Assert.Equal(expectedIndex, ezLayoutResult.EZLayers.First().EZKeys.First().Position);
        }

        [ Theory ]
        [ InlineData("KC_ASDN", "Autoshift timeout down", KeyCategory.Autoshift) ]
        [ InlineData("KC_0", "0", KeyCategory.Digit) ]
        [ InlineData("KC_A", "A", KeyCategory.Letters) ]
        [ InlineData("KC_F1", "F1", KeyCategory.Fn) ]
        [ InlineData("MAGIC_TOGGLE_NKRO", "NKRO", KeyCategory.Fw) ]
        [ InlineData("KC_LANG1", "LANG 1", KeyCategory.Lang) ]
        [ InlineData("KC_LSHIFT", "Shift", KeyCategory.Modifier) ]
        [ InlineData("KC_KP_0", "0", KeyCategory.Numpad) ]
        [ InlineData("KC_TRANSPARENT", "", KeyCategory.Other) ]
        [ InlineData("KC_PAUSE", "Pause", KeyCategory.Other) ]
        [ InlineData("KC_SCOLON", ";", KeyCategory.Punct) ]
        [ InlineData("KC_AT", "@", KeyCategory.ShiftedPunct) ]
        [ InlineData("KC_SYSTEM_POWER", "Power", KeyCategory.System) ]
        public void PrepareEZLayout_KeyCategoryWithDirectLabel(string keyCode, string expectedLabel, KeyCategory expectedCategory)
        {
            // Arrange
            var ergodoxKey = new ErgodoxKey() {
                                                  GlowColor = "",
                                                  Code = keyCode
                                              };
            ErgodoxLayout ergodoxLayout = InitializeDataTree();
            ergodoxLayout.Revisions.First().Layers.First().Keys.Add(ergodoxKey);

            EZLayout ezLayoutResult;

            // Act
            var ezLayoutMaker = new EZLayoutMaker();
            ezLayoutResult = ezLayoutMaker.PrepareEZLayout(ergodoxLayout);

            // Assert
            Assert.Single(ezLayoutResult.EZLayers);
            Assert.Single(ezLayoutResult.EZLayers.First().EZKeys);
            var keyResult = ezLayoutResult.EZLayers.First().EZKeys.First();
            Assert.Equal(expectedLabel, keyResult.Label);
            Assert.Equal(expectedCategory, keyResult.KeyCategory);
        }

        [ Theory ]
        [ InlineData("KC_LALT", "MOD_LSFT", "Option", "", false, KeyCategory.Modifier) ]
        [ InlineData("OSM", "", "OSM", "", false, KeyCategory.Modifier) ]
        [ InlineData("OSM", "MOD_LSFT", "OSM", "Shift", true, KeyCategory.Modifier) ]
        public void PrepareEZLayout_KeyCategoryOSM(string keyCode, string command, string expectedLabel, string expectedSubLabel, bool expectedIsSubLabelAbove, KeyCategory expectedCategory)
        {
            // Arrange
            var ergodoxKey = new ErgodoxKey() {
                                                  GlowColor = "",
                                                  Code = keyCode,
                                                  Command = command,
                                                  Layer = 1
                                              };
            ErgodoxLayout ergodoxLayout = InitializeDataTree();
            ergodoxLayout.Revisions.First().Layers.First().Keys.Add(ergodoxKey);

            EZLayout ezLayoutResult;

            // Act
            var ezLayoutMaker = new EZLayoutMaker();
            ezLayoutResult = ezLayoutMaker.PrepareEZLayout(ergodoxLayout);

            // Assert
            Assert.Single(ezLayoutResult.EZLayers);
            Assert.Single(ezLayoutResult.EZLayers.First().EZKeys);

            var keyResult = ezLayoutResult.EZLayers.First().EZKeys.First();
            Assert.Equal(expectedLabel, keyResult.Label);
            Assert.Equal(expectedSubLabel, keyResult.SubLabel);
            Assert.Equal(expectedIsSubLabelAbove, keyResult.IsSubLabelAbove);
            Assert.Equal(expectedCategory, keyResult.KeyCategory);
        }

        [ Theory ]
        [ InlineData("TG", "\u2750 1", KeyCategory.Layer) ] // Should display "❐ TG 1"
        [ InlineData("MO", "\u27F2 1", KeyCategory.Layer) ] // Should display "⟲ MO 1"
        [ InlineData("OSL", "OSL 1", KeyCategory.Layer) ]
        [ InlineData("TO", "TO 1", KeyCategory.Layer) ]
        [ InlineData("TT", "TT 1", KeyCategory.Layer) ]
        public void PrepareEZLayout_KeyCategoryLayer(string keyCode, string expectedLabel, KeyCategory expectedCategory)
        {
            // Arrange
            var ergodoxKey = new ErgodoxKey() {
                                                  GlowColor = "",
                                                  Code = keyCode,
                                                  Layer = 1
                                              };
            ErgodoxLayout ergodoxLayout = InitializeDataTree();
            ergodoxLayout.Revisions.First().Layers.First().Keys.Add(ergodoxKey);

            EZLayout ezLayoutResult;

            // Act
            var ezLayoutMaker = new EZLayoutMaker();
            ezLayoutResult = ezLayoutMaker.PrepareEZLayout(ergodoxLayout);

            // Assert
            Assert.Single(ezLayoutResult.EZLayers);
            Assert.Single(ezLayoutResult.EZLayers.First().EZKeys);
            var keyResult = ezLayoutResult.EZLayers.First().EZKeys.First();
            Assert.Equal(expectedLabel, keyResult.Label);
            Assert.Equal(expectedCategory, keyResult.KeyCategory);
        }

        [ Theory ]
        [ InlineData("LT", "", "LT → 1", "", KeyCategory.LayerShortcuts) ]      // Should display "LT → 1"
        [ InlineData("LT", "KC_0", "0", "LT → 1", KeyCategory.LayerShortcuts) ] // Should display "0LT → 1"
        public void PrepareEZLayout_KeyCategoryLayerShortcut(string keyCode, string command, string expectedLabel, string expectedSubLabel, KeyCategory expectedCategory)
        {
            // Arrange
            var ergodoxKey = new ErgodoxKey() {
                                                  GlowColor = "",
                                                  Code = keyCode,
                                                  Command = command,
                                                  Layer = 1
                                              };
            ErgodoxLayout ergodoxLayout = InitializeDataTree();
            ergodoxLayout.Revisions.First().Layers.First().Keys.Add(ergodoxKey);

            EZLayout ezLayoutResult;

            // Act
            var ezLayoutMaker = new EZLayoutMaker();
            ezLayoutResult = ezLayoutMaker.PrepareEZLayout(ergodoxLayout);

            // Assert
            Assert.Single(ezLayoutResult.EZLayers);
            Assert.Single(ezLayoutResult.EZLayers.First().EZKeys);
            var keyResult = ezLayoutResult.EZLayers.First().EZKeys.First();
            Assert.Equal(expectedLabel, keyResult.Label);
            Assert.Equal(expectedSubLabel, keyResult.SubLabel);
            Assert.Equal(expectedCategory, keyResult.KeyCategory);
        }

        [ Theory ]
        [ InlineData("KC_AUDIO_MUTE", "Mute", "volume-off", false, KeyCategory.Media) ]
        [ InlineData("KC_MEDIA_EJECT", "Eject", null, true, KeyCategory.Media) ]
        [ InlineData("KC_MS_UP", "Move up", "mouse-up", false, KeyCategory.Mouse) ]
        [ InlineData("KC_MS_BTN4", "Button 4", null, true, KeyCategory.Mouse) ]
        [ InlineData("KC_APPLICATION", "Application", "list-alt", false, KeyCategory.Nav) ]
        [ InlineData("KC_PGDOWN", "PgDn", null, true, KeyCategory.Nav) ]
        [ InlineData("KC_BSPACE", "Backspace", "backspace", false, KeyCategory.Spacing) ]
        [ InlineData("KC_ESCAPE", "Esc", null, true, KeyCategory.Spacing) ]
        [ InlineData("RGB_MOD", "Animate", "air", false, KeyCategory.Shine) ]
        public void PrepareEZLayout_KeyCategoryWithGlyphs(string keyCode, string expectedLabel, string expectedGlyph, bool expectedIsDisplayedLabel, KeyCategory expectedCategory)
        {
            // Arrange
            var ergodoxKey = new ErgodoxKey() {
                                                  GlowColor = "",
                                                  Code = keyCode
                                              };
            ErgodoxLayout ergodoxLayout = InitializeDataTree();
            ergodoxLayout.Revisions.First().Layers.First().Keys.Add(ergodoxKey);

            EZLayout ezLayoutResult;

            // Act
            var ezLayoutMaker = new EZLayoutMaker();
            ezLayoutResult = ezLayoutMaker.PrepareEZLayout(ergodoxLayout);

            // Assert
            Assert.Single(ezLayoutResult.EZLayers);
            Assert.Single(ezLayoutResult.EZLayers.First().EZKeys);
            var keyResult = ezLayoutResult.EZLayers.First().EZKeys.First();
            Assert.Equal(expectedLabel, keyResult.Label);
            Assert.Equal(expectedGlyph, keyResult.GlyphName);
            Assert.Equal(expectedIsDisplayedLabel, keyResult.IsLabelDisplayed);
            Assert.Equal(expectedCategory, keyResult.KeyCategory);
        }

        [ Theory ]
        [ InlineData("ALL_T", "KC_6", "6", "Hyper", KeyCategory.DualFunction) ]
        [ InlineData("ALL_T", "", "Hyper", "", KeyCategory.DualFunction) ]
        public void PrepareEZLayout_KeyCategoryDualFunction(string keyCode, string command, string expectedLabel, string expectedSubLabel, KeyCategory expectedCategory)
        {
            // Arrange
            var ergodoxKey = new ErgodoxKey() {
                                                  GlowColor = "",
                                                  Code = keyCode,
                                                  Command = command
                                              };
            ErgodoxLayout ergodoxLayout = InitializeDataTree();
            ergodoxLayout.Revisions.First().Layers.First().Keys.Add(ergodoxKey);

            EZLayout ezLayoutResult;

            // Act
            var ezLayoutMaker = new EZLayoutMaker();
            ezLayoutResult = ezLayoutMaker.PrepareEZLayout(ergodoxLayout);

            // Assert
            Assert.Single(ezLayoutResult.EZLayers);
            Assert.Single(ezLayoutResult.EZLayers.First().EZKeys);
            var keyResult = ezLayoutResult.EZLayers.First().EZKeys.First();
            Assert.Equal(expectedLabel, keyResult.Label);
            Assert.Equal(expectedSubLabel, keyResult.SubLabel);
            Assert.Equal(expectedCategory, keyResult.KeyCategory);
        }

        [ Theory ]
        [ InlineData("LALT", "KC_3", "Alt + 3", "", KeyCategory.Shortcuts) ]
        [ InlineData("LALT", "", "Alt", "", KeyCategory.Shortcuts) ]
        public void PrepareEZLayout_KeyCategoryShortcuts(string keyCode, string command, string expectedLabel, string expectedSubLabel, KeyCategory expectedCategory)
        {
            // Arrange
            var ergodoxKey = new ErgodoxKey() {
                                                  GlowColor = "",
                                                  Code = keyCode,
                                                  Command = command
                                              };
            ErgodoxLayout ergodoxLayout = InitializeDataTree();
            ergodoxLayout.Revisions.First().Layers.First().Keys.Add(ergodoxKey);

            EZLayout ezLayoutResult;

            // Act
            var ezLayoutMaker = new EZLayoutMaker();
            ezLayoutResult = ezLayoutMaker.PrepareEZLayout(ergodoxLayout);

            // Assert
            Assert.Single(ezLayoutResult.EZLayers);
            Assert.Single(ezLayoutResult.EZLayers.First().EZKeys);
            var keyResult = ezLayoutResult.EZLayers.First().EZKeys.First();
            Assert.Equal(expectedLabel, keyResult.Label);
            Assert.Equal(expectedSubLabel, keyResult.SubLabel);
            Assert.Equal(expectedCategory, keyResult.KeyCategory);
        }

        [ Theory ]
        [ InlineData(false, false, false, false, false, false, false, false, "", false) ]
        [ InlineData(true, false, false, false, false, false, false, false, "Alt", true) ]
        [ InlineData(false, true, false, false, false, false, false, false, "Ctrl", true) ]
        [ InlineData(false, false, true, false, false, false, false, false, "Shift", true) ]
        [ InlineData(false, false, false, true, false, false, false, false, "Win", true) ]
        [ InlineData(false, false, false, false, true, false, false, false, "Alt", true) ]
        [ InlineData(false, false, false, false, false, true, false, false, "Ctrl", true) ]
        [ InlineData(false, false, false, false, false, false, true, false, "Shift", true) ]
        [ InlineData(false, false, false, false, false, false, false, true, "Win", true) ]
        [ InlineData(true, true, false, false, false, false, false, false, "ALT+CTL", true) ]
        [ InlineData(true, false, true, false, false, false, false, false, "ALT+SFT", true) ]
        [ InlineData(true, false, false, true, false, false, false, false, "ALT+WIN", true) ]
        [ InlineData(false, false, false, false, true, true, false, false, "ALT+CTL", true) ]
        [ InlineData(false, false, false, false, true, false, true, false, "ALT+SFT", true) ]
        [ InlineData(false, false, false, false, true, false, false, true, "ALT+WIN", true) ]
        [ InlineData(false, true, false, false, true, false, false, false, "CTL+ALT", true) ]
        [ InlineData(false, false, false, true, true, false, false, false, "WIN+ALT", true) ]
        [ InlineData(false, true, false, true, true, false, false, false, "C+W+A", true) ]
        [ InlineData(false, true, false, false, true, false, false, true, "C+A+W", true) ]
        [ InlineData(false, true, true, false, false, false, false, true, "C+S+W", true) ]
        [ InlineData(false, true, false, true, false, false, true, false, "C+W+S", true) ]
        [ InlineData(false, true, true, true, false, false, false, false, "C+W+S", true) ]
        [ InlineData(true, true, true, true, false, false, false, false, "A+C+W+S", true) ]
        [ InlineData(false, false, false, false, true, true, true, true, "A+C+W+S", true) ]
        [ InlineData(true, false, true, false, false, true, false, true, "A+S+C+W", true) ]
        [ InlineData(false, true, false, true, true, false, true, false, "C+W+A+S", true) ]
        public void PrepareEZLayout_ProcessModifiers(bool   leftAlt,
                                                     bool   leftCtrl,
                                                     bool   leftShift,
                                                     bool   leftWin,
                                                     bool   rightAlt,
                                                     bool   rightCtrl,
                                                     bool   rightShift,
                                                     bool   rightWin,
                                                     string expectedSubLabel,
                                                     bool   expectedIsSubLabelAbove)
        {
            // Arrange
            var modifiers = new ErgodoxModifiers {
                                                     LeftAlt = leftAlt,
                                                     LeftCtrl = leftCtrl,
                                                     LeftShift = leftShift,
                                                     LeftWin = leftWin,
                                                     RightAlt = rightAlt,
                                                     RightCtrl = rightCtrl,
                                                     RightShift = rightShift,
                                                     RightWin = rightWin
                                                 };
            var ergodoxKey = new ErgodoxKey() {
                                                  GlowColor = "",
                                                  Code = "KC_A",
                                                  Modifiers = modifiers
                                              };
            ErgodoxLayout ergodoxLayout = InitializeDataTree();
            ergodoxLayout.Revisions.First().Layers.First().Keys.Add(ergodoxKey);

            EZLayout ezLayoutResult;

            // Act
            var ezLayoutMaker = new EZLayoutMaker();
            ezLayoutResult = ezLayoutMaker.PrepareEZLayout(ergodoxLayout);

            // Assert
            Assert.Single(ezLayoutResult.EZLayers);
            Assert.Single(ezLayoutResult.EZLayers.First().EZKeys);
            var keyResult = ezLayoutResult.EZLayers.First().EZKeys.First();
            Assert.Equal("A", keyResult.Label);
            Assert.Equal(expectedSubLabel, keyResult.SubLabel);
            Assert.Equal(expectedIsSubLabelAbove, keyResult.IsSubLabelAbove);
        }
    }
}