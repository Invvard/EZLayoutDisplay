using System;
using System.Collections.Generic;
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
             * KeyCategory.Numpad
             * KeyCategory.Other
             * KeyCategory.Punct
             * KeyCategory.ShiftedPunct
             * KeyCategory.System
             *
             **/
            EZKey key = new EZKey {
                                      KeyCategory = keyDefinition.KeyCategory,
                                      Label = new KeyLabel(keyDefinition.Label, keyDefinition.IsGlyph),
                                      Color = ergodoxKey.GlowColor,
                                      DisplayType = KeyDisplayType.SimpleLabel
                                  };

            switch (keyDefinition.KeyCategory)
            {
                case KeyCategory.DualFunction:

                    AddCommandLabel(ergodoxKey, key);

                    break;
                case KeyCategory.Layer:
                case KeyCategory.LayerShortcuts:

                    key.Label.Content = string.Format(key.Label.Content, ergodoxKey.Layer.ToString());
                    AddCommandLabel(ergodoxKey, key);

                    break;
                case KeyCategory.Modifier:

                    if (ergodoxKey.Code == "OSM" && !string.IsNullOrWhiteSpace(ergodoxKey.Command))
                    {
                        var commandDefinition = GetKeyDefinition(ergodoxKey.Command);
                        key.Modifier = new KeyLabel(commandDefinition.Label);
                        key.DisplayType = KeyDisplayType.ModifierOnTop;
                    }

                    break;
                case KeyCategory.Media:
                case KeyCategory.Mouse:
                case KeyCategory.Nav:
                case KeyCategory.Spacing:
                case KeyCategory.Shine:
                    key.DisplayType = KeyDisplayType.SimpleLabel;

                    break;
                case KeyCategory.Shortcuts:

                    if (!string.IsNullOrWhiteSpace(ergodoxKey.Command))
                    {
                        var commandDefinition = GetKeyDefinition(ergodoxKey.Command);
                        key.Label.Content = $"{key.Label.Content} + {commandDefinition.Label}";
                    }

                    break;
                case KeyCategory.Nordic:

                    break;
                default:

                    break;
            }

            ProcessModifiers(ergodoxKey, key);

            return key;
        }

        private KeyDefinition GetKeyDefinition(string ergodoxKeyCode)
        {
            var keyDefinition = _keyDefinitionDictionary.KeyDefinitions.FirstOrDefault(k => k.KeyCode == ergodoxKeyCode) ?? GetKeyDefinition("KC_NO");

            return keyDefinition;
        }

        private void AddCommandLabel(ErgodoxKey ergodoxKey, EZKey key)
        {
            if (string.IsNullOrWhiteSpace(ergodoxKey.Command)) return;

            var commandDefinition = GetKeyDefinition(ergodoxKey.Command);
            key.Modifier = key.Label;
            key.Label = new KeyLabel(commandDefinition.Label, commandDefinition.IsGlyph);
            key.DisplayType = KeyDisplayType.ModifierOnTop;
        }

        private void ProcessModifiers(ErgodoxKey ergodoxKey, EZKey key)
        {
            if (ergodoxKey.Modifiers == null) return;

            var mods = GetModifiersApplied(ergodoxKey.Modifiers);

            if (!mods.Any()) return;

            key.Modifier = new KeyLabel(AggregateModifierLabels(mods));
            key.DisplayType = KeyDisplayType.ModifierOnTop;
        }

        private List<EZModifier> GetModifiersApplied(ErgodoxModifiers ergodoxModifiers)
        {
            var keyModifiers = new KeyModifierDictionary();
            var mods = new List<EZModifier>();

            if (ergodoxModifiers.LeftAlt)
            {
                mods.Add(keyModifiers.EZModifiers.First(m => m.KeyModifier == KeyModifier.LeftAlt));
            }

            if (ergodoxModifiers.LeftCtrl)
            {
                mods.Add(keyModifiers.EZModifiers.First(m => m.KeyModifier == KeyModifier.LeftCtrl));
            }

            if (ergodoxModifiers.LeftShift)
            {
                mods.Add(keyModifiers.EZModifiers.First(m => m.KeyModifier == KeyModifier.LeftShift));
            }

            if (ergodoxModifiers.LeftWin)
            {
                mods.Add(keyModifiers.EZModifiers.First(m => m.KeyModifier == KeyModifier.LeftWin));
            }

            if (ergodoxModifiers.RightAlt)
            {
                mods.Add(keyModifiers.EZModifiers.First(m => m.KeyModifier == KeyModifier.RightAlt));
            }

            if (ergodoxModifiers.RightCtrl)
            {
                mods.Add(keyModifiers.EZModifiers.First(m => m.KeyModifier == KeyModifier.RightCtrl));
            }

            if (ergodoxModifiers.RightShift)
            {
                mods.Add(keyModifiers.EZModifiers.First(m => m.KeyModifier == KeyModifier.RightShift));
            }

            if (ergodoxModifiers.RightWin)
            {
                mods.Add(keyModifiers.EZModifiers.First(m => m.KeyModifier == KeyModifier.RightWin));
            }

            return mods.OrderBy(m => m.Index).ToList();
        }

        private string AggregateModifierLabels(List<EZModifier> mods)
        {
            var subLabel = "";

            switch (mods.Count)
            {
                case 1:
                    subLabel = mods.First().Labels[EZModifier.LabelSize.Large];

                    break;
                case 2:
                    subLabel = mods.Select(m => m.Labels[EZModifier.LabelSize.Medium]).Aggregate((seed, inc) => $"{seed}+{inc}");

                    break;
                default:
                    subLabel = mods.Select(m => m.Labels[EZModifier.LabelSize.Small]).Aggregate((seed, inc) => $"{seed}+{inc}");

                    break;
            }

            return subLabel;
        }
    }
}