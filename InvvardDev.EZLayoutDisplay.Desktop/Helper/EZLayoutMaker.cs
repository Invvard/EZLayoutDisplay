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
                EZKey key = PrepareKeyLabels(ergodoxLayer.Keys[index], index);


                layer.EZKeys.Add(key);
            }

            return layer;
        }

        private EZKey PrepareKeyLabels(ErgodoxKey ergodoxKey, int keyIndex)
        {
            KeyDefinition keyDefinition = GetKeyDefinition(ergodoxKey.Code);

            /** Every category has a label, so no need to make a special case :
             *
             * KeyCategory.Autoshift
             * KeyCategory.Digit
             * KeyCategory.Letters
             * KeyCategory.Fn
             * KeyCategory.Fw
             * KeyCategory.Lang
             * KeyCategory.Modifier
             * KeyCategory.Numpad
             * KeyCategory.Other
             * KeyCategory.Punct
             * KeyCategory.ShiftedPunct
             * KeyCategory.System
             *
             **/
            EZKey key = new EZKey {
                                      KeyCategory = keyDefinition.KeyCategory,
                                      Label = keyDefinition.Label,
                                      SubLabel = "",
                                      Color = ergodoxKey.GlowColor
                                  };

            switch (keyDefinition.KeyCategory)
            {
                case KeyCategory.DualFunction:
                    AddCommandLabel(ergodoxKey, key);

                    break;
                case KeyCategory.Layer:
                case KeyCategory.LayerShortcuts:
                    key.Label = string.Format(key.Label, ergodoxKey.Layer.ToString());
                    AddCommandLabel(ergodoxKey, key);

                    break;
                case KeyCategory.Media:
                case KeyCategory.Mouse:
                case KeyCategory.Nav:
                case KeyCategory.Spacing:
                case KeyCategory.Shine:
                    key.GlyphName = keyDefinition.GlyphName;

                    break;
                case KeyCategory.Shortcuts:

                    if (!string.IsNullOrWhiteSpace(ergodoxKey.Command))
                    {
                        var commandDefinition = GetKeyDefinition(ergodoxKey.Command);
                        key.Label = $"{key.Label} + {commandDefinition.Label}";
                    }

                    break;
                case KeyCategory.Nordic:

                    break;
                default:

                    break;
            }

            return key;
        }

        private KeyDefinition GetKeyDefinition(string ergodoxKeyCode)
        {
            var keyDefinition = _keyDefinitionDictionary.KeyDefinitions.First(k => k.KeyCode == ergodoxKeyCode);

            return keyDefinition;
        }

        private void AddCommandLabel(ErgodoxKey ergodoxKey, EZKey key)
        {
            if (string.IsNullOrWhiteSpace(ergodoxKey.Command)) return;

            var commandDefinition = GetKeyDefinition(ergodoxKey.Command);
            key.SubLabel = key.Label;
            key.Label = commandDefinition.Label;
        }
    }
}