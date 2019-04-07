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

        [ Fact ]
        public void PrepareEZLayout_KeyCodeUnknown()
        {
            // Arrange
            var ergodoxKey = new ErgodoxKey() {
                                                  GlowColor = "",
                                                  Code = "KC_UNKNOWN_ADSLKFJ"
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
            Assert.Equal("", ezLayoutResult.EZLayers.First().EZKeys.First().Label.Content);
            Assert.False(ezLayoutResult.EZLayers.First().EZKeys.First().Label.IsGlyph);
        }

        [ Theory ]
        [ InlineData("KC_TRANSPARENT", "expectedColor") ]
        public void PrepareEZLayout_InitializeEZKey(string expectedKeyCode, string expectedColor)
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
        }

        [ Theory ]
        [ InlineData("KC_ASDN", "Autoshift timeout down", KeyCategory.AutoShift) ]
        [ InlineData("KC_0", "0", KeyCategory.Digit) ]
        [ InlineData("KC_A", "A", KeyCategory.Letters) ]
        [ InlineData("KC_F1", "F1", KeyCategory.Fn) ]
        [ InlineData("MAGIC_TOGGLE_NKRO", "NKRO", KeyCategory.Fw) ]
        [ InlineData("KC_LANG1", "LANG 1", KeyCategory.Lang) ]
        [ InlineData("KC_LSHIFT", "\u21e7", KeyCategory.Modifier) ]
        [ InlineData("KC_KP_0", "0", KeyCategory.NumPad) ]
        [ InlineData("KC_TRANSPARENT", "", KeyCategory.Other) ]
        [ InlineData("KC_PAUSE", "Pause", KeyCategory.Other) ]
        [ InlineData("KC_SCOLON", ";", KeyCategory.Punct) ]
        [ InlineData("KC_AT", "@", KeyCategory.ShiftedPunct) ]
        [ InlineData("KC_SYSTEM_POWER", "Power", KeyCategory.System) ]
        [ InlineData("FR_AMP", "&", KeyCategory.French) ]
        [ InlineData("NO_LESS", "<", KeyCategory.Nordic) ]
        [ InlineData("ES_PLUS", "+", KeyCategory.Spanish) ]
        public void PrepareEZLayout_KeyCategoryWithSimpleLabel(string keyCode, string expectedLabel, KeyCategory expectedCategory)
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
            Assert.Equal(expectedLabel, keyResult.Label.Content);
            Assert.False(keyResult.Label.IsGlyph);
            Assert.Equal(expectedCategory, keyResult.KeyCategory);
        }

        [ Theory ]
        [ InlineData("KC_LALT", "MOD_LSFT", "Left Alt", "", KeyDisplayType.SimpleLabel, KeyCategory.Modifier) ]
        [ InlineData("OSM", "", "OSM", "", KeyDisplayType.SimpleLabel, KeyCategory.Modifier) ]
        [ InlineData("OSM", "MOD_LSFT", "OSM", "\u21e7", KeyDisplayType.ModifierOnTop, KeyCategory.Modifier) ]
        public void PrepareEZLayout_KeyCategoryOSM(string keyCode, string command, string expectedLabel, string expectedSubLabel, KeyDisplayType expectedDisplayType, KeyCategory expectedCategory)
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
            Assert.Equal(expectedLabel, keyResult.Label.Content);
            if (expectedDisplayType == KeyDisplayType.SimpleLabel)
            {
                Assert.Null(keyResult.Modifier);
            }
            else
            {
                Assert.Equal(expectedSubLabel, keyResult.Modifier.Content);
                Assert.False(keyResult.Modifier.IsGlyph);
            }
            Assert.Equal(expectedDisplayType, keyResult.DisplayType);
            Assert.Equal(expectedCategory, keyResult.KeyCategory);
        }

        [ Theory ]
        [ InlineData("TG", "❐ 1", KeyCategory.Layer) ]
        [ InlineData("MO", "⟲ 1", KeyCategory.Layer) ]
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
            Assert.Equal(expectedLabel, keyResult.Label.Content);
            Assert.Equal(expectedCategory, keyResult.KeyCategory);
        }

        [ Theory ]
        [ InlineData("LT", "", "LT → 1", "", KeyDisplayType.SimpleLabel, KeyCategory.LayerShortcuts) ]
        [ InlineData("LT", "KC_0", "0", "LT → 1", KeyDisplayType.ModifierUnder, KeyCategory.LayerShortcuts) ]
        public void PrepareEZLayout_KeyCategoryLayerShortcut(string keyCode, string command, string expectedLabel, string expectedSubLabel, KeyDisplayType expectedDisplayType, KeyCategory expectedCategory)
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
            Assert.Equal(expectedLabel, keyResult.Label.Content);
            if (expectedDisplayType == KeyDisplayType.SimpleLabel)
            {
                Assert.Null(keyResult.Modifier);
            }
            else
            {
                Assert.Equal(expectedSubLabel, keyResult.Modifier.Content);
                Assert.False(keyResult.Modifier.IsGlyph);
            }
            Assert.Equal(expectedCategory, keyResult.KeyCategory);
        }

        [ Theory ]
        [ InlineData("KC_AUDIO_MUTE", "\ue913", KeyDisplayType.SimpleLabel, KeyCategory.Media, true) ]
        [ InlineData("KC_MEDIA_EJECT", "\ue90c", KeyDisplayType.SimpleLabel, KeyCategory.Media, true) ]
        [ InlineData("KC_MS_UP", "\ue91c", KeyDisplayType.SimpleLabel, KeyCategory.Mouse, true) ]
        [ InlineData("KC_MS_BTN4", "Button 4", KeyDisplayType.SimpleLabel, KeyCategory.Mouse, false) ]
        [ InlineData("KC_APPLICATION", "\ue90f", KeyDisplayType.SimpleLabel, KeyCategory.Nav, true) ]
        [ InlineData("KC_PGDOWN", "PgDn", KeyDisplayType.SimpleLabel, KeyCategory.Nav, false) ]
        [ InlineData("KC_BSPACE", "\ue918", KeyDisplayType.SimpleLabel, KeyCategory.Spacing, true) ]
        [ InlineData("KC_ESCAPE", "Esc", KeyDisplayType.SimpleLabel, KeyCategory.Spacing, false) ]
        [ InlineData("RGB_MOD", "\ue916", KeyDisplayType.SimpleLabel, KeyCategory.Shine, true) ]
        public void PrepareEZLayout_KeyCategoryWithGlyphs(string keyCode, string expectedLabel, KeyDisplayType expectedDisplayType, KeyCategory expectedCategory, bool expectedIsGlyph)
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
            Assert.Equal(expectedLabel, keyResult.Label.Content);
            Assert.Equal(expectedIsGlyph, keyResult.Label.IsGlyph);
            Assert.Equal(expectedDisplayType, keyResult.DisplayType);
            Assert.Equal(expectedCategory, keyResult.KeyCategory);
        }

        [ Theory ]
        [ InlineData("ALL_T", "KC_6", "6", "Hyper", KeyCategory.DualFunction, KeyDisplayType.ModifierUnder) ]
        [ InlineData("ALL_T", "", "Hyper", "", KeyCategory.Modifier, KeyDisplayType.SimpleLabel) ]
        public void PrepareEZLayout_KeyCategoryDualFunction(string keyCode, string command, string expectedLabel, string expectedSubLabel, KeyCategory expectedCategory, KeyDisplayType expectedDisplayType)
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
            Assert.Equal(expectedLabel, keyResult.Label.Content);
            if (expectedDisplayType == KeyDisplayType.SimpleLabel)
            {
                Assert.Null(keyResult.Modifier);
            }
            else
            {
                Assert.Equal(expectedSubLabel, keyResult.Modifier.Content);
            }
            Assert.Equal(expectedCategory, keyResult.KeyCategory);
        }

        [ Theory ]
        [ InlineData("LALT", "KC_3", "Alt + 3", KeyCategory.Shortcuts) ]
        [ InlineData("LALT", "", "Alt", KeyCategory.Shortcuts) ]
        public void PrepareEZLayout_KeyCategoryShortcuts(string keyCode, string command, string expectedLabel, KeyCategory expectedCategory)
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
            Assert.Equal(expectedLabel, keyResult.Label.Content);
            Assert.Null(keyResult.Modifier);
            Assert.Equal(expectedCategory, keyResult.KeyCategory);
        }

        [ Theory ]
        [ InlineData(false, false, false, false, false, false, false, false, "", KeyDisplayType.SimpleLabel) ]
        [ InlineData(true, false, false, false, false, false, false, false, "Alt", KeyDisplayType.ModifierOnTop) ]
        [ InlineData(false, true, false, false, false, false, false, false, "Ctrl", KeyDisplayType.ModifierOnTop) ]
        [ InlineData(false, false, true, false, false, false, false, false, "Shift", KeyDisplayType.ModifierOnTop) ]
        [ InlineData(false, false, false, true, false, false, false, false, "Win", KeyDisplayType.ModifierOnTop) ]
        [ InlineData(false, false, false, false, true, false, false, false, "Alt", KeyDisplayType.ModifierOnTop) ]
        [ InlineData(false, false, false, false, false, true, false, false, "Ctrl", KeyDisplayType.ModifierOnTop) ]
        [ InlineData(false, false, false, false, false, false, true, false, "Shift", KeyDisplayType.ModifierOnTop) ]
        [ InlineData(false, false, false, false, false, false, false, true, "Win", KeyDisplayType.ModifierOnTop) ]
        [ InlineData(true, true, false, false, false, false, false, false, "ALT+CTL", KeyDisplayType.ModifierOnTop) ]
        [ InlineData(true, false, true, false, false, false, false, false, "ALT+SFT", KeyDisplayType.ModifierOnTop) ]
        [ InlineData(true, false, false, true, false, false, false, false, "ALT+WIN", KeyDisplayType.ModifierOnTop) ]
        [ InlineData(false, false, false, false, true, true, false, false, "ALT+CTL", KeyDisplayType.ModifierOnTop) ]
        [ InlineData(false, false, false, false, true, false, true, false, "ALT+SFT", KeyDisplayType.ModifierOnTop) ]
        [ InlineData(false, false, false, false, true, false, false, true, "ALT+WIN", KeyDisplayType.ModifierOnTop) ]
        [ InlineData(false, true, false, false, true, false, false, false, "CTL+ALT", KeyDisplayType.ModifierOnTop) ]
        [ InlineData(false, false, false, true, true, false, false, false, "WIN+ALT", KeyDisplayType.ModifierOnTop) ]
        [ InlineData(false, true, false, true, true, false, false, false, "C+W+A", KeyDisplayType.ModifierOnTop) ]
        [ InlineData(false, true, false, false, true, false, false, true, "C+A+W", KeyDisplayType.ModifierOnTop) ]
        [ InlineData(false, true, true, false, false, false, false, true, "C+S+W", KeyDisplayType.ModifierOnTop) ]
        [ InlineData(false, true, false, true, false, false, true, false, "C+W+S", KeyDisplayType.ModifierOnTop) ]
        [ InlineData(false, true, true, true, false, false, false, false, "C+W+S", KeyDisplayType.ModifierOnTop) ]
        [ InlineData(true, true, true, true, false, false, false, false, "A+C+W+S", KeyDisplayType.ModifierOnTop) ]
        [ InlineData(false, false, false, false, true, true, true, true, "A+C+W+S", KeyDisplayType.ModifierOnTop) ]
        [ InlineData(true, false, true, false, false, true, false, true, "A+S+C+W", KeyDisplayType.ModifierOnTop) ]
        [ InlineData(false, true, false, true, true, false, true, false, "C+W+A+S", KeyDisplayType.ModifierOnTop) ]
        public void PrepareEZLayout_ProcessModifiers(bool           leftAlt,
                                                     bool           leftCtrl,
                                                     bool           leftShift,
                                                     bool           leftWin,
                                                     bool           rightAlt,
                                                     bool           rightCtrl,
                                                     bool           rightShift,
                                                     bool           rightWin,
                                                     string         expectedSubLabel,
                                                     KeyDisplayType expectedDisplayType)
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
            Assert.Equal("A", keyResult.Label.Content);
            if (expectedDisplayType == KeyDisplayType.SimpleLabel)
            {
                Assert.Null(keyResult.Modifier);
            }
            else
            {
                Assert.Equal(expectedSubLabel, keyResult.Modifier.Content);
                Assert.False(keyResult.Modifier.IsGlyph);
            }
            Assert.Equal(expectedDisplayType, keyResult.DisplayType);
        }
    }
}