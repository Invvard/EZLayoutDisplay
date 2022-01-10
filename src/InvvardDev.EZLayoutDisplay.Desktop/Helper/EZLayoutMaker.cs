using System;
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
                var keyDisplayMode = SelectKeyDisplayMode(ergodoxKey);
                var key = PrepareKeyLabels(ergodoxKey, keyDisplayMode);
                key.Color = GetColor(ergodoxKey.GlowColor, layer.Color);

                layer.EZKeys.Add(key);
            }

            Logger.DebugOutputParam(nameof(layer), layer);

            return layer;
        }

        private KeyDisplayMode SelectKeyDisplayMode(ErgodoxKey ergodoxKey)
        {
            var featureCount = new List<ErgodoxKeyFeature> {
                ergodoxKey.Tap,
                ergodoxKey.Hold,
                ergodoxKey.DoubleTap,
                ergodoxKey.TapHold
            }.Where(f => f != null)
            .Count();

            var displayMode = featureCount switch
            {
                _ when !string.IsNullOrWhiteSpace(ergodoxKey.CustomLabel) => KeyDisplayMode.CustomLabel,
                1 => KeyDisplayMode.SingleFeature,
                > 1 => KeyDisplayMode.DoubleFeature,
                _ => throw new NotImplementedException(),
            };

            return displayMode;
        }

        private EZKey PrepareKeyLabels(ErgodoxKey ergodoxKey, KeyDisplayMode displayMode)
        {
            Logger.TraceMethod();
            Logger.DebugInputParam(nameof(ergodoxKey), ergodoxKey);

            var key = new EZKey
            {
                DisplayMode = displayMode
            };

            switch (key.DisplayMode)
            {
                case KeyDisplayMode.CustomLabel:
                    key.Primary = new KeyFeature(ergodoxKey.CustomLabel);
                    break;
                case KeyDisplayMode.SingleFeature:
                    (key.Primary, _) = GetDisplayedFeature(ergodoxKey);
                    break;
                case KeyDisplayMode.DoubleFeature:
                    (key.Primary, key.Secondary) = GetDisplayedFeature(ergodoxKey, 2);
                    break;
            }

            Logger.DebugOutputParam(nameof(key), key);

            return key;
        }

        private (KeyFeature, KeyFeature) GetDisplayedFeature(ErgodoxKey ergodoxKey, int count = 1)
        {
            var features = new List<ErgodoxKeyFeature> {
                ergodoxKey.Tap,
                ergodoxKey.Hold,
                ergodoxKey.DoubleTap,
                ergodoxKey.TapHold }.Where(f => f != null).ToList();

            KeyFeature[] keyFeatures = new KeyFeature[2];

            for (int i = 0; i < count; i++)
            {
                var feature = features.Count >= i + 1 ? features[i] : null;
                if (feature != null)
                {
                    var mods = ProcessModifiers(feature.Modifiers);
                    var keyDefinition = GetKeyDefinition(feature.Code);

                    keyFeatures[i] = new KeyFeature(keyDefinition.Label, keyDefinition.IsGlyph, keyDefinition.Tag, mods);
                }
            }

            return (keyFeatures[0], keyFeatures[1]);
        }

        private static string GetColor(string keyColor, string defaultColor = "#777")
        {
            return string.IsNullOrWhiteSpace(keyColor) ? defaultColor : keyColor;
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

        private string ProcessModifiers(ErgodoxModifiers modifiers)
        {
            if (modifiers == null) return "";

            var mods = GetModifiersApplied(modifiers);

            return AggregateModifierLabels(mods);
        }

        private List<EZModifier> GetModifiersApplied(ErgodoxModifiers ergodoxModifiers)
        {
            var keyModifiers = new KeyModifierDictionary();
            var mods = new List<EZModifier>();

            if (ergodoxModifiers.LeftAlt) mods.Add(keyModifiers.EZModifiers[KeyModifier.LeftAlt]);
            if (ergodoxModifiers.LeftCtrl) mods.Add(keyModifiers.EZModifiers[KeyModifier.LeftCtrl]);
            if (ergodoxModifiers.LeftShift) mods.Add(keyModifiers.EZModifiers[KeyModifier.LeftShift]);
            if (ergodoxModifiers.LeftWin) mods.Add(keyModifiers.EZModifiers[KeyModifier.LeftWin]);
            if (ergodoxModifiers.RightAlt) mods.Add(keyModifiers.EZModifiers[KeyModifier.RightAlt]);
            if (ergodoxModifiers.RightCtrl) mods.Add(keyModifiers.EZModifiers[KeyModifier.RightCtrl]);
            if (ergodoxModifiers.RightShift) mods.Add(keyModifiers.EZModifiers[KeyModifier.RightShift]);
            if (ergodoxModifiers.RightWin) mods.Add(keyModifiers.EZModifiers[KeyModifier.RightWin]);

            return mods.OrderBy(m => m.Index).ToList();
        }

        private string AggregateModifierLabels(List<EZModifier> mods)
        {
            return mods.Count switch
            {
                1 => mods.First().Labels[EZModifier.LabelSize.Large],
                2 => mods.Select(m => m.Labels[EZModifier.LabelSize.Medium]).Aggregate((seed, inc) => $"{seed}+{inc}"),
                > 2 => mods.Select(m => m.Labels[EZModifier.LabelSize.Small]).Aggregate((seed, inc) => $"{seed}+{inc}"),
                _ => ""
            };
        }
    }
}