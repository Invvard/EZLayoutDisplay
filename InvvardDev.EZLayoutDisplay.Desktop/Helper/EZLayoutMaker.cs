using System;
using System.Linq;
using InvvardDev.EZLayoutDisplay.Desktop.Model;
using InvvardDev.EZLayoutDisplay.Desktop.Model.Dictionary;
using InvvardDev.EZLayoutDisplay.Desktop.Model.Enum;

namespace InvvardDev.EZLayoutDisplay.Desktop.Helper
{
    public class EZLayoutMaker
    {
        private readonly KeyCategoryDictionary _keyCategoryDictionary;
        private readonly KeyDefinitionDictionary _keyDefinitionDictionary;

        public EZLayoutMaker()
        {
            _keyCategoryDictionary = new KeyCategoryDictionary();
            _keyDefinitionDictionary = new KeyDefinitionDictionary();
        }

        public EZLayout PrepareEZLayout(ErgodoxLayout ergodoxLayout)
        {
            var ezLayout = new EZLayout {
                                            HashId = ergodoxLayout.HashId,
                                            Name = ergodoxLayout.Title
                                        };

            foreach (var ergodoxLayer in ergodoxLayout.Revisions.First().Layers)
            {
                EZLayer ezLayer = PrepareEZLayer(ergodoxLayer);
                ezLayout.EZLayers.Add(ezLayer);
            }


            return ezLayout;
        }

        private EZLayer PrepareEZLayer(ErgodoxLayer ergodoxLayer)
        {
            var layer = new EZLayer {
                                        Index = ergodoxLayer.Position,
                                        Name = ergodoxLayer.Title,
                                        Color = ergodoxLayer.Color
                                    };

            for (var index = 0 ; index < ergodoxLayer.Keys.Count ; index++)
            {
                EZKey key = PrepareKey(ergodoxLayer.Keys[index], index);

                layer.EZKeys.Add(key);
            }

            return layer;
        }

        private EZKey PrepareKey(ErgodoxKey ergodoxKey, int index)
        {
            EZKey key = new EZKey {
                                      Position = index,
                                      Color = ergodoxKey.GlowColor
                                  };

            PrepareKeyLabels(ergodoxKey, key);

            return key;
        }

        private void PrepareKeyLabels(ErgodoxKey ergodoxKey, EZKey key)
        {
            KeyDefinition keyDefinition = GetKeyDefinition(ergodoxKey.Code);
            key.KeyCategory = keyDefinition.KeyCategory;

            switch (keyDefinition.KeyCategory)
            {
                case KeyCategory.Autoshift:
                case KeyCategory.Digit:
                case KeyCategory.Letters:
                case KeyCategory.Fn:
                case KeyCategory.Fw:
                case KeyCategory.Lang:
                case KeyCategory.Modifier:
                case KeyCategory.Numpad:
                case KeyCategory.Punct:
                case KeyCategory.ShiftedPunct:
                case KeyCategory.System:
                    key.Label = keyDefinition.Label;
                    key.SubLabel = "";
                    break;
                case KeyCategory.DualFunction:

                    break;
                case KeyCategory.Layer:

                    break;
                case KeyCategory.LayerShortcuts:

                    break;
                case KeyCategory.Media:

                    break;
                case KeyCategory.Mouse:

                    break;
                case KeyCategory.Nav:

                    break;
                case KeyCategory.Nordic:

                    break;
                case KeyCategory.Other:

                    break;
                case KeyCategory.Shine:

                    break;
                case KeyCategory.Shortcuts:

                    break;
                case KeyCategory.Spacing:

                    break;
                default:
                    break;
            }
        }

        private KeyDefinition GetKeyDefinition(string ergodoxKeyCode)
        {
            var keyDefinition = _keyDefinitionDictionary.KeyDefinitions.First(k => k.KeyCode == ergodoxKeyCode);

            return keyDefinition;
        }
    }
}