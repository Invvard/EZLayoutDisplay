using System.Collections.Generic;
using System.Linq;
using InvvardDev.EZLayoutDisplay.Desktop.Model;
using InvvardDev.EZLayoutDisplay.Desktop.Model.Dictionary;
using InvvardDev.EZLayoutDisplay.Desktop.Model.Enum;
using InvvardDev.EZLayoutDisplay.Desktop.Model.Ez;
using InvvardDev.EZLayoutDisplay.Desktop.Model.Ez.Content;
using InvvardDev.EZLayoutDisplay.Desktop.Model.ZsaModels;
using NLog;

namespace InvvardDev.EZLayoutDisplay.Desktop.Helper
{
    public class EZLayoutMaker
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private const string TransparentKey = "KC_TRANSPARENT";
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
                var key = PrepareKeyContent(ergodoxKey, keyDisplayMode);
                key.GlowColor = GetColor(ergodoxKey.GlowColor, layer.Color);

                layer.Keys.Add(key);
            }

            Logger.DebugOutputParam(nameof(layer), layer);

            return layer;
        }

        private KeyDisplayMode SelectKeyDisplayMode(ErgodoxKey ergodoxKey)
        {
            var features = new List<ErgodoxKeyFeature> {
                ergodoxKey.Tap,
                ergodoxKey.Hold,
                ergodoxKey.DoubleTap,
                ergodoxKey.TapHold
            }.Where(f => f != null)
            .ToList();

            var featureCount = features.Count;
            KeyDefinition firstKeyDefinition = null;
            if (features.Any())
            {
                firstKeyDefinition = GetKeyDefinition(features.First().Code);
            }

            var category = featureCount switch
            {
                0 => KeyDisplayMode.Empty,
                1 when ergodoxKey.Tap?.Macro != null => KeyDisplayMode.Macro,
                1 when firstKeyDefinition.Category == KeyCategory.Modifier => KeyDisplayMode.Modifier,
                1 when firstKeyDefinition.Category == KeyCategory.Shine => KeyDisplayMode.ColorControl,
                1 when !string.IsNullOrWhiteSpace(ergodoxKey.CustomLabel) => KeyDisplayMode.CustomLabel,
                >= 2 => KeyDisplayMode.DualFunction,
                _ => KeyDisplayMode.Base
            };

            return category;
        }

        private Key PrepareKeyContent(ErgodoxKey ergodoxKey, KeyDisplayMode displayMode)
        {
            Logger.TraceMethod();
            Logger.DebugInputParam(nameof(ergodoxKey), ergodoxKey);
            Logger.DebugInputParam(nameof(displayMode), displayMode);

            var features = new List<ErgodoxKeyFeature> {
                ergodoxKey.Tap,
                ergodoxKey.Hold,
                ergodoxKey.DoubleTap,
                ergodoxKey.TapHold }
            .Where(f => f != null)
            .ToList();

            var key = new Key() { DisplayMode = displayMode };

            switch (displayMode)
            {
                case KeyDisplayMode.CustomLabel:
                    key.Primary = new BaseContent { Label = ergodoxKey.CustomLabel };
                    break;
                case KeyDisplayMode.Macro:
                    key.Primary = new BaseContent { Label = "Macro" };
                    break;
                case KeyDisplayMode.Base:
                case KeyDisplayMode.Modifier:
                case KeyDisplayMode.ColorControl:
                    key.Primary = GetDisplayedFeature(features[0]);
                    break;
                case KeyDisplayMode.DualFunction:
                    key.Primary = GetDisplayedFeature(features[0]);
                    key.Secondary = GetDisplayedFeature(features[1]);

                    if (key.Primary.Label.Length > 1) key.Primary.Tag = "";
                    if (key.Secondary.Label.Length > 1) key.Secondary.Tag = "";
                    break;
                default:
                    key.Primary = new BaseContent { Label = string.Empty };
                    break;
            }

            Logger.DebugOutputParam(nameof(key), key);

            return key;
        }

        private BaseContent GetDisplayedFeature(ErgodoxKeyFeature feature)
        {
            var mods = ProcessModifiers(feature.Modifiers);
            var keyDefinition = GetKeyDefinition(feature.Code);

            var keyFeature = keyDefinition switch
            {
                // ColorControl Key - Color picker 
                { KeyCode: "RGB" } => new ColorPicker { Label = keyDefinition.Label, ColorCode = feature.Color },
                // Layer toggle key
                { Category: KeyCategory.Layer } => new Layer { Label = keyDefinition.Label, Id = feature.Layer.Value },
                { IsGlyph: true } => new Glyph { Label = keyDefinition.Label, Modifier = mods },
                // Modded label
                _ when !string.IsNullOrWhiteSpace(mods) => new BaseContent { Label = $"{mods}+{keyDefinition.Label}", Tag = keyDefinition.Tag },
                _ => new BaseContent { Label = keyDefinition.Label, Tag = keyDefinition.Tag }
            };

            return keyFeature;
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