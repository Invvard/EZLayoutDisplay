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

            var ezLayout = new EZLayout { HashId = ergodoxLayout.HashId, Name = ergodoxLayout.Title, Geometry = ergodoxLayout.Geometry };

            var ergodoxLayers = ergodoxLayout.Revision.Layers ?? ergodoxLayout.Revision.Layers;

            if (ergodoxLayers?.Any() != null)
            {
                foreach (var ergodoxLayer in ergodoxLayers)
                {
                    var ezLayer = PrepareEZLayer(ergodoxLayer);
                    ezLayout.EZLayers.Add(ezLayer);
                }
            }

            Logger.DebugOutputParam(nameof(ezLayout), ezLayout);

            return ezLayout;
        }

        private EZLayer PrepareEZLayer(ErgodoxLayer ergodoxLayer)
        {
            Logger.TraceMethod();
            Logger.DebugInputParam(nameof(ergodoxLayer), ergodoxLayer);

            var layer = new EZLayer { Index = ergodoxLayer.Position, Name = ergodoxLayer.Title, Color = GetColor(ergodoxLayer.Color) };

            foreach (var ergodoxKey in ergodoxLayer.Keys)
            {
                var key = PrepareKeyLabels(ergodoxKey, layer.Color);

                layer.EZKeys.Add(key);
            }

            Logger.DebugOutputParam(nameof(layer), layer);

            return layer;
        }

        // TODO : refactor
        private EZKey PrepareKeyLabels(ErgodoxKey ergodoxKey, string layerColor)
        {
            Logger.TraceMethod();
            Logger.DebugInputParam(nameof(ergodoxKey), ergodoxKey);

            var keyDefinition = GetKeyDefinition(ergodoxKey.Tap.Code);

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
            var key = new EZKey
            {
                Primary = new KeyFeature
                {
                    Label = new KeyLabel(ergodoxKey.CustomLabel != null ? ergodoxKey.CustomLabel : keyDefinition.Label, keyDefinition.IsGlyph)
                },
                Color = GetColor(ergodoxKey.GlowColor, layerColor),
                DisplayType = KeyDisplayType.CustomLabel
            };

            ProcessModifiers(ergodoxKey, key);

            Logger.DebugOutputParam(nameof(key), key);

            return key;
        }

        private static string GetColor(string keyColor, string defaultColor = "#777")
        {
            var fontColor = string.IsNullOrWhiteSpace(keyColor) ? defaultColor : keyColor;

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

        private void ProcessModifiers(ErgodoxKey ergodoxKey, EZKey key)
        {
            if (ergodoxKey.Tap.Modifiers == null && ergodoxKey.Hold.Modifiers == null) return;

            var mods = GetModifiersApplied(ergodoxKey.Tap.Modifiers);

            if (!mods.Any()) return;

            key.Primary.Modifier = new KeyLabel(AggregateModifierLabels(mods));
            key.DisplayType = KeyDisplayType.TapMod;
        }

        private List<EZModifier> GetModifiersApplied(ErgodoxModifiers ergodoxModifiers)
        {
            var keyModifiers = new KeyModifierDictionary();
            var mods = new List<EZModifier>();

            if (ergodoxModifiers.LeftAlt) mods.Add(keyModifiers.EZModifiers.First(m => m.KeyModifier == KeyModifier.LeftAlt));

            if (ergodoxModifiers.LeftCtrl) mods.Add(keyModifiers.EZModifiers.First(m => m.KeyModifier == KeyModifier.LeftCtrl));

            if (ergodoxModifiers.LeftShift) mods.Add(keyModifiers.EZModifiers.First(m => m.KeyModifier == KeyModifier.LeftShift));

            if (ergodoxModifiers.LeftWin) mods.Add(keyModifiers.EZModifiers.First(m => m.KeyModifier == KeyModifier.LeftWin));

            if (ergodoxModifiers.RightAlt) mods.Add(keyModifiers.EZModifiers.First(m => m.KeyModifier == KeyModifier.RightAlt));

            if (ergodoxModifiers.RightCtrl) mods.Add(keyModifiers.EZModifiers.First(m => m.KeyModifier == KeyModifier.RightCtrl));

            if (ergodoxModifiers.RightShift) mods.Add(keyModifiers.EZModifiers.First(m => m.KeyModifier == KeyModifier.RightShift));

            if (ergodoxModifiers.RightWin) mods.Add(keyModifiers.EZModifiers.First(m => m.KeyModifier == KeyModifier.RightWin));

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
    }
}