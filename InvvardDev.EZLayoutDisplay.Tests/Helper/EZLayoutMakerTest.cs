using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using InvvardDev.EZLayoutDisplay.Desktop.Helper;
using InvvardDev.EZLayoutDisplay.Desktop.Model;
using InvvardDev.EZLayoutDisplay.Desktop.Model.Enum;
using Xunit;
using static System.Text.Encoding;
using Assert = Xunit.Assert;

namespace InvvardDev.EZLayoutDisplay.Tests.Helper
{
    public class EZLayoutMakerTest
    {
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
        public void PrepareEZLayout_KeyCategoryWithDirectLabel(string expectedKeyCode, string expectedLabel, KeyCategory expectedCategory)
        {
            // Arrange
            var ergodoxKey = new ErgodoxKey() {
                                                  GlowColor = "",
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
            var keyResult = ezLayoutResult.EZLayers.First().EZKeys.First();
            Assert.Equal(expectedLabel, keyResult.Label);
            Assert.Equal(expectedCategory, keyResult.KeyCategory);
        }

        [Theory]
        [InlineData("TG", "â TG 1", KeyCategory.Layer)] // Should display "❐ TG 1"
        [InlineData("MO", "âŸ² MO 1", KeyCategory.Layer)] // Should display "⟲ MO 1"
        [InlineData("OSL", "OSL 1", KeyCategory.Layer)]
        [InlineData("TO", "TO 1", KeyCategory.Layer)]
        [InlineData("TT", "TT 1", KeyCategory.Layer)]
        public void PrepareEZLayout_KeyCategoryLayer(string expectedKeyCode, string expectedLabel, KeyCategory expectedCategory)
        {
            // Arrange
            var ergodoxKey = new ErgodoxKey()
                             {
                                 GlowColor = "",
                                 Code = expectedKeyCode,
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

        [Theory]
        [InlineData("LT", "", "LT â†’ 1", "", KeyCategory.LayerShortcuts)] // Should display "LT → 1"
        [InlineData("LT", "KC_0", "0", "LT â†’ 1", KeyCategory.LayerShortcuts)] // Should display "0LT → 1"
        public void PrepareEZLayout_KeyCategoryLayerShortcut(string keyCode, string command, string expectedLabel, string expectedSubLabel, KeyCategory expectedCategory)
        {
            // Arrange
            var ergodoxKey = new ErgodoxKey()
                             {
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

        [Theory]
        [InlineData("KC_AUDIO_MUTE", "Mute", "volume-off", KeyCategory.Media)]
        [InlineData("KC_MEDIA_EJECT", "Eject", null, KeyCategory.Media)]
        [InlineData("KC_MS_UP", "Move up", "mouse-up", KeyCategory.Mouse)]
        [InlineData("KC_MS_BTN4", "Button 4", null, KeyCategory.Mouse)]
        [InlineData("KC_APPLICATION", "Application", "list-alt", KeyCategory.Nav)]
        [InlineData("KC_PGDOWN", "PgDn", null, KeyCategory.Nav)]
        [InlineData("KC_BSPACE", "Backspace", "backspace", KeyCategory.Spacing)]
        [InlineData("KC_ESCAPE", "Esc", null, KeyCategory.Spacing)]
        [InlineData("RGB_MOD", "Animate", "air", KeyCategory.Shine)]
        public void PrepareEZLayout_KeyCategoryWithGlyphs(string expectedKeyCode, string expectedLabel, string expectedGlyph, KeyCategory expectedCategory)
        {
            // Arrange
            var ergodoxKey = new ErgodoxKey()
                             {
                                 GlowColor = "",
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
            var keyResult = ezLayoutResult.EZLayers.First().EZKeys.First();
            Assert.Equal(expectedLabel, keyResult.Label);
            Assert.Equal(expectedGlyph, keyResult.GlyphName);
            Assert.Equal(expectedCategory, keyResult.KeyCategory);
        }

        [Theory]
        [InlineData("ALL_T", "KC_6", "6", "Hyper", KeyCategory.DualFunction)]
        [InlineData("ALL_T", "", "Hyper", "", KeyCategory.DualFunction)]
        public void PrepareEZLayout_KeyCategoryDualFunction(string expectedKeyCode, string expectedCommand, string expectedLabel, string expectedSubLabel, KeyCategory expectedCategory)
        {
            // Arrange
            var ergodoxKey = new ErgodoxKey()
                             {
                                 GlowColor = "",
                                 Code = expectedKeyCode,
                                 Command = expectedCommand
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

        [Theory]
        [InlineData("LALT", "KC_3", "Alt + 3", "", KeyCategory.Shortcuts)]
        [InlineData("LALT", "", "Alt", "", KeyCategory.Shortcuts)]
        public void PrepareEZLayout_KeyCategoryShortcuts(string keyCode, string command, string expectedLabel, string expectedSubLabel, KeyCategory expectedCategory)
        {
            // Arrange
            var ergodoxKey = new ErgodoxKey()
                             {
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
    }
}