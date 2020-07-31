using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvvardDev.EZLayoutDisplay.Desktop.Model;
using InvvardDev.EZLayoutDisplay.Desktop.Model.Enum;
using InvvardDev.EZLayoutDisplay.Desktop.Service.Implementation;
using InvvardDev.EZLayoutDisplay.Desktop.Service.Interface;
using Xunit;

namespace InvvardDev.EZLayoutDisplay.Tests.Service
{
    public class LayoutServiceTest
    {
        private static ErgodoxLayout InitializeDataTree()
        {
            return new ErgodoxLayout {
                                         Title = "",
                                         HashId = "",
                                         Revision = new Revision { 
                                                                        HashId = "hashId-1",
                                                                        Layers = new List<ErgodoxLayer> {
                                                                                                            new ErgodoxLayer() {
                                                                                                                                Color = "",
                                                                                                                                Title = "",
                                                                                                                                Position = 0,
                                                                                                                                Keys = new List<ErgodoxKey>()
                                                                                                                            }
                                                                                                        }
                                                                   }
                                     };
        }

        [ Theory ]
        [ InlineData("EOEb", true) ]
        [ InlineData("default", true) ]
        [ InlineData("test", false) ]
        public async Task GetLayoutInfo(string layoutHashId, bool exist)
        {
            // Arrange
            ILayoutService layoutService = new LayoutService();

            // Act
            ErgodoxLayout response = null;

            if (exist)
            {
                response = await layoutService.GetLayoutInfo(layoutHashId);
            }
            else
            {
                await Assert.ThrowsAsync<ArgumentException>(() => layoutService.GetLayoutInfo(layoutHashId));
            }

            // Assert
            if (exist)
            {
                Assert.NotNull(response);
                Assert.IsType<ErgodoxLayout>(response);
                Assert.NotNull(response.Revision);
                Assert.False(string.IsNullOrWhiteSpace(response.HashId));
                Assert.False(string.IsNullOrWhiteSpace(response.Title));
            }
        }

        [ Theory ]
        [ InlineData("EOEb", true) ]
        [ InlineData("default", true) ]
        [ InlineData("test", false) ]
        public async Task GetErgodoxLayout(string layoutHashId, bool exist)
        {
            // Arrange
            ILayoutService layoutService = new LayoutService();

            // Act
            ErgodoxLayout response = null;

            if (exist)
            {
                response = await layoutService.GetErgodoxLayout(layoutHashId);
            }
            else
            {
                await Assert.ThrowsAsync<ArgumentException>(() => layoutService.GetErgodoxLayout(layoutHashId));
            }

            // Assert
            if (exist)
            {
                Assert.NotNull(response);
                Assert.IsType<ErgodoxLayout>(response);
                Assert.NotNull(response.Revision);
                Assert.False(string.IsNullOrWhiteSpace(response.HashId));
                Assert.False(string.IsNullOrWhiteSpace(response.Title));
            }
        }

        [ Fact ]
        public void PrepareEZLayout_OneLayer_ManyKeys()
        {
            // Arrange
            ILayoutService layoutService = new LayoutService();
            ErgodoxLayout ergodoxLayout = InitializeDataTree();
            var keys = ergodoxLayout.Revision.Layers.First().Keys;
            keys.Add(new ErgodoxKey() {
                                          GlowColor = "",
                                          Code = "KC_A"
                                      });
            keys.Add(new ErgodoxKey() {
                                          GlowColor = "",
                                          Code = "KC_0"
                                      });
            keys.Add(new ErgodoxKey() {
                                          GlowColor = "",
                                          Code = "KC_TRANSPARENT"
                                      });
            EZLayout ezLayoutResult;

            // Act
            ezLayoutResult = layoutService.PrepareEZLayout(ergodoxLayout, ergodoxLayout.Revision.HashId);

            // Assert
            Assert.Single(ezLayoutResult.EZLayers);
            Assert.Equal(3, ezLayoutResult.EZLayers.First().EZKeys.Count);

            var keyResults = ezLayoutResult.EZLayers.First().EZKeys;
            Assert.Equal("A", keyResults[0].Label.Content);
            Assert.Equal(KeyCategory.Letters, keyResults[0].KeyCategory);
            Assert.Equal("0", keyResults[1].Label.Content);
            Assert.Equal(KeyCategory.Digit, keyResults[1].KeyCategory);
            Assert.Equal("", keyResults[2].Label.Content);
            Assert.Equal(KeyCategory.Other, keyResults[2].KeyCategory);
        }

        [ Fact ]
        public void PrepareEZLayout_TwoLayer_ManyKeys()
        {
            // Arrange
            ILayoutService layoutService = new LayoutService();
            ErgodoxLayout ergodoxLayout = InitializeDataTree();
            var layer0Keys = ergodoxLayout.Revision.Layers.First().Keys;
            layer0Keys.Add(new ErgodoxKey() {
                                                GlowColor = "",
                                                Code = "KC_A"
                                            });
            layer0Keys.Add(new ErgodoxKey() {
                                                GlowColor = "",
                                                Code = "KC_0"
                                            });
            layer0Keys.Add(new ErgodoxKey() {
                                                GlowColor = "",
                                                Code = "KC_TRANSPARENT"
                                            });

            ergodoxLayout.Revision
                         .Layers.Add(new ErgodoxLayer {
                                                          Color = "color",
                                                          Title = "Layer 2",
                                                          Position = 1,
                                                          Keys = new List<ErgodoxKey>()
                                                      });
            var layer1Keys = ergodoxLayout.Revision.Layers[1].Keys;
            layer1Keys.Add(new ErgodoxKey() {
                                                GlowColor = "",
                                                Code = "KC_F1"
                                            });
            layer1Keys.Add(new ErgodoxKey() {
                                                GlowColor = "",
                                                Code = "KC_SPACE"
                                            });
            EZLayout ezLayoutResult;

            // Act
            ezLayoutResult = layoutService.PrepareEZLayout(ergodoxLayout, ergodoxLayout.Revision.HashId);

            // Assert
            Assert.Equal(2, ezLayoutResult.EZLayers.Count);
            Assert.Equal(3, ezLayoutResult.EZLayers[0].EZKeys.Count);
            Assert.Equal(2, ezLayoutResult.EZLayers[1].EZKeys.Count);

            var layer0KeyResults = ezLayoutResult.EZLayers[0].EZKeys;
            Assert.Equal("A", layer0KeyResults[0].Label.Content);
            Assert.Equal(KeyCategory.Letters, layer0KeyResults[0].KeyCategory);
            Assert.Equal("0", layer0KeyResults[1].Label.Content);
            Assert.Equal(KeyCategory.Digit, layer0KeyResults[1].KeyCategory);
            Assert.Equal("", layer0KeyResults[2].Label.Content);
            Assert.Equal(KeyCategory.Other, layer0KeyResults[2].KeyCategory);

            var layer1KeyResults = ezLayoutResult.EZLayers[1].EZKeys;
            Assert.Equal("F1", layer1KeyResults[0].Label.Content);
            Assert.Equal(KeyCategory.Fn, layer1KeyResults[0].KeyCategory);
            Assert.Equal("\u23b5", layer1KeyResults[1].Label.Content);
            Assert.Equal(KeyCategory.Spacing, layer1KeyResults[1].KeyCategory);
        }

        [ Fact ]
        public async Task GetErgodoxLayout_HashIdNull()
        {
            // Arrange
            ILayoutService layoutService = new LayoutService();

            // Act
            await Assert.ThrowsAsync<ArgumentNullException>(() => layoutService.GetErgodoxLayout(""));
        }
    }
}