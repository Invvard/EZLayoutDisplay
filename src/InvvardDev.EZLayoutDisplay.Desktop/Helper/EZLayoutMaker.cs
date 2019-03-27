using System.Collections.Generic;
using System.Linq;
using InvvardDev.EZLayoutDisplay.Desktop.Model;
using InvvardDev.EZLayoutDisplay.Desktop.Model.Dictionary;
using InvvardDev.EZLayoutDisplay.Desktop.Model.Enum;
using NLog;

namespace InvvardDev.EZLayoutDisplay.Desktop.Helper
{
    public class EZLayoutMaker
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private const string NoCommand = "KC_NO";
        private const string TransparentKey = "KC_TRANSPARENT";
        private const string KeyCodeOsm = "OSM";
        private readonly KeyDefinitionDictionary _keyDefinitionDictionary;

        public EZLayoutMaker()
        {
            Logger.TraceConstructor();
            _keyDefinitionDictionary = new KeyDefinitionDictionary();
        }

        public EZLayout PrepareEZLayout(ErgodoxLayout ergodoxLayout)
        {
            Logger.TraceMethod();
            Logger.DebugInputParam(nameof(ergodoxLayout), ergodoxLayout);

            var ezLayout = new EZLayout {
                                            HashId = ergodoxLayout.HashId,
                                            Name = ergodoxLayout.Title
                                        };

            foreach (var ergodoxLayer in ergodoxLayout.Revisions.First().Layers)
            {
                EZLayer ezLayer = PrepareEZLayer(ergodoxLayer);
                ezLayout.EZLayers.Add(ezLayer);
            }

            Logger.DebugOutputParam(nameof(ezLayout), ezLayout);

            return ezLayout;
        }

        private EZLayer PrepareEZLayer(ErgodoxLayer ergodoxLayer)
        {
            Logger.TraceMethod();
            Logger.DebugInputParam(nameof(ergodoxLayer), ergodoxLayer);

            var layer = new EZLayer {
                                        Index = ergodoxLayer.Position,
                                        Name = ergodoxLayer.Title,
                                        Color = GetColor(ergodoxLayer.Color)
                                    };

            foreach (var ergodoxKey in ergodoxLayer.Keys)
            {
                EZKey key = PrepareKeyLabels(ergodoxKey);

                layer.EZKeys.Add(key);
            }

            Logger.DebugOutputParam(nameof(layer), layer);

            return layer;
        }

        private EZKey PrepareKeyLabels(ErgodoxKey ergodoxKey)
        {
            Logger.TraceMethod();
            Logger.DebugInputParam(nameof(ergodoxKey), ergodoxKey);

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
                                      Color = GetColor(ergodoxKey.GlowColor),
                                      DisplayType = KeyDisplayType.SimpleLabel
                                  };

            switch (keyDefinition.KeyCategory)
            {
                case KeyCategory.DualFunction:

                    if (AddCommandLabel(ergodoxKey, key))
                    {
                        key.DisplayType = KeyDisplayType.ModifierUnder;
                    }
                    else
                    {
                        key.KeyCategory = KeyCategory.Modifier;
                    }

                    break;
                case KeyCategory.Layer:
                case KeyCategory.LayerShortcuts:
                    key.Label.Content = string.Format(key.Label.Content, ergodoxKey.Layer.ToString());

                    if (AddCommandLabel(ergodoxKey, key))
                    {
                        key.DisplayType = KeyDisplayType.ModifierUnder;
                    }

                    break;
                case KeyCategory.Modifier:

                    if (ergodoxKey.Code == KeyCodeOsm && !IsCommandEmpty(ergodoxKey.Command))
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

                    if (!IsCommandEmpty(ergodoxKey.Command))
                    {
                        var commandDefinition = GetKeyDefinition(ergodoxKey.Command);
                        key.Label.Content = $"{key.Label.Content} + {commandDefinition.Label}";
                    }

                    break;
                case KeyCategory.French:
                    key.InternationalHint = "fr";

                    break;
                case KeyCategory.German:
                    key.InternationalHint = "de";

                    break;
                case KeyCategory.Spanish:
                    key.InternationalHint = "es";

                    break;
            }

            ProcessModifiers(ergodoxKey, key);

            Logger.DebugOutputParam(nameof(key), key);

            return key;
        }

        private static string GetColor(string keyColor)
        {
            var fontColor = string.IsNullOrWhiteSpace(keyColor) ? "#777777" : keyColor;

            return fontColor;
        }

        private KeyDefinition GetKeyDefinition(string ergodoxKeyCode)
        {
            var keyDefinition = _keyDefinitionDictionary.KeyDefinitions.FirstOrDefault(k => k.KeyCode == ergodoxKeyCode);

            if (keyDefinition == null)
            {
                Logger.Warn("Key code '{0}' unknown", ergodoxKeyCode);
                keyDefinition = GetKeyDefinition(TransparentKey);
            }

            return keyDefinition;
        }

        /// <summary>
        /// Apply the command label.
        /// </summary>
        /// <param name="ergodoxKey">The <see cref="ErgodoxKey"/> containing the command to be applied.</param>
        /// <param name="key">The <see cref="EZKey"/> to apply the command to.</param>
        /// <returns><c>True</c> if command has been applied.</returns>
        private bool AddCommandLabel(ErgodoxKey ergodoxKey, EZKey key)
        {
            if (IsCommandEmpty(ergodoxKey.Command)) return false;

            var commandDefinition = GetKeyDefinition(ergodoxKey.Command);
            key.Modifier = key.Label;
            key.Label = new KeyLabel(commandDefinition.Label, commandDefinition.IsGlyph);

            return true;
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
            string subLabel;

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

        private bool IsCommandEmpty(string command)
        {
            var isEmpty = string.IsNullOrWhiteSpace(command) || command == NoCommand || command == KeyCodeOsm;

            return isEmpty;
        }
    }
}