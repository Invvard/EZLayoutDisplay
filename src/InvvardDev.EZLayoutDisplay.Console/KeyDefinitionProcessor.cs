using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using InvvardDev.EZLayoutDisplay.Desktop.Model;
using InvvardDev.EZLayoutDisplay.Desktop.Model.Enum;
using Newtonsoft.Json;

namespace InvvardDev.EZLayoutDisplay.Console
{
    public class KeyDefinitionProcessor
    {
        private const string KeyDefinitionInputFilename = "keyDefinitions.json";
        private const string KeyDefinitionOutputFilename = "keyDefinitions.output.json";

        public void RunProcess()
        {
            if (!CheckKeyDefinitionJsExists())
            {
                return;
            }

            var keyCodesOrigin = ReadJsonFile();

            if (!keyCodesOrigin.Any())
            {
                return;
            }

            List<KeyDefinition> keyDefinitions = ToEZKeyDefinitions(keyCodesOrigin);

            WriteJsonFile(keyDefinitions);
        }

        private bool CheckKeyDefinitionJsExists()
        {
            var fileExist = File.Exists(KeyDefinitionInputFilename);

            return fileExist;
        }

        private List<ConfiguratorKeyDefinition> ReadJsonFile()
        {
            List<ConfiguratorKeyDefinition> keyCodes;

            using (StreamReader sr = new StreamReader(KeyDefinitionInputFilename))
            {
                var json = sr.ReadToEnd();
                keyCodes = JsonConvert.DeserializeObject<List<ConfiguratorKeyDefinition>>(json);
            }

            return keyCodes;
        }

        private List<KeyDefinition> ToEZKeyDefinitions(List<ConfiguratorKeyDefinition> keyCodesOrigin)
        {
            List<KeyDefinition> keyDefinitions = new List<KeyDefinition>();

            foreach (var configKeyDef in keyCodesOrigin)
            {
                var (label, isGlyph) = ChooseLabel(configKeyDef);
                var keyDefinition = new KeyDefinition {
                                                          Label = label,
                                                          Description = configKeyDef.Description,
                                                          IsGlyph = isGlyph,
                                                          KeyCode = configKeyDef.KeyCode,
                                                          KeyCategory = (KeyCategory) Enum.Parse(typeof(KeyCategory), configKeyDef.Category, true),
                                                          SecondaryCommand = configKeyDef.Command
                                                      };

                if (configKeyDef.PrecedingKey.HasValue)
                {
                    keyDefinition.PrecedingKey = configKeyDef.PrecedingKey.Value;
                }

                keyDefinitions.Add(keyDefinition);
            }

            return keyDefinitions;
        }

        private void WriteJsonFile(List<KeyDefinition> keyDefinitions)
        {
            var json = JsonConvert.SerializeObject(keyDefinitions);
            json = json.Replace(@"\\u", @"\u");

            File.WriteAllText(KeyDefinitionOutputFilename, json);
        }

        private (string, bool) ChooseLabel(ConfiguratorKeyDefinition configKeyDef)
        {
            var label = string.IsNullOrWhiteSpace(configKeyDef.Label)
                            ? configKeyDef.MenuLabel
                            : configKeyDef.Label;

            bool isGlyph;
            (label, isGlyph) = ApplySpecialLabel(configKeyDef.KeyCode, label);

            return (label, isGlyph);
        }

        private (string, bool) ApplySpecialLabel(string keyCode, string label)
        {
            var isGlyph = false;

            switch (keyCode)
            {
                case "KC_TRANSPARENT":
                    label = "";

                    break;
                case "TG":
                    label = "\\u2750 {0}";

                    break;
                case "MO":
                    label = "\\u27F2 {0}";

                    break;
                case "OSL":
                    label = "OSL {0}";

                    break;
                case "TO":
                    label = "TO {0}";

                    break;
                case "TT":
                    label = "TT {0}";

                    break;
                case "LT":
                    label = "LT \\u2192 {0}";

                    break;
                case "KC_AUDIO_MUTE":
                    label = "\\ue913";
                    isGlyph = true;

                    break;
                case "KC_AUDIO_VOL_UP":
                    label = "\\ue914";
                    isGlyph = true;

                    break;
                case "KC_AUDIO_VOL_DOWN":
                    label = "\\ue912";
                    isGlyph = true;

                    break;
                case "KC_MEDIA_NEXT_TRACK":
                    label = "\\ue908";
                    isGlyph = true;

                    break;
                case "KC_MEDIA_PREV_TRACK":
                    label = "\\ue90a";
                    isGlyph = true;

                    break;
                case "KC_MEDIA_STOP":
                    label = "\\ue919";
                    isGlyph = true;

                    break;
                case "KC_MEDIA_PLAY_PAUSE":
                    label = "\\ue91a";
                    isGlyph = true;

                    break;
                case "KC_MEDIA_EJECT":
                    label = "\\ue90c";
                    isGlyph = true;

                    break;
                case "KC_MEDIA_FAST_FORWARD":
                    label = "\\ue906";
                    isGlyph = true;

                    break;
                case "KC_MEDIA_REWIND":
                    label = "\\ue909";
                    isGlyph = true;

                    break;
                case "LSFT_T":
                case "RSFT_T":
                case "KC_LSHIFT":
                case "KC_RSHIFT":
                case "MOD_LSFT":
                case "MOD_RSFT":
                    label = "\\u21e7";

                    break;
                case "KC_LGUI":
                case "KC_RGUI":
                case "MOD_LGUI":
                case "MOD_RGUI":
                    label = "\\ue904";
                    isGlyph = true;

                    break;
                case "KC_MS_UP":
                    label = "\\ue91c";
                    isGlyph = true;

                    break;
                case "KC_MS_DOWN":
                    label = "\\ue91d";
                    isGlyph = true;

                    break;
                case "KC_MS_LEFT":
                    label = "\\ue91e";
                    isGlyph = true;

                    break;
                case "KC_MS_RIGHT":
                    label = "\\ue91f";
                    isGlyph = true;

                    break;
                case "KC_MS_BTN1":
                    label = "\\ue920";
                    isGlyph = true;

                    break;
                case "KC_MS_BTN2":
                    label = "\\ue922";
                    isGlyph = true;

                    break;
                case "KC_MS_BTN3":
                    label = "\\ue921";
                    isGlyph = true;

                    break;
                case "KC_BSPACE":
                    label = "\\ue918";
                    isGlyph = true;

                    break;
                case "KC_ENTER":
                    label = "\\u23ce";

                    break;
                case "KC_SPACE":
                    label = "\\u23b5";

                    break;
                case "KC_APPLICATION":
                    label = "\\ue90f";
                    isGlyph = true;

                    break;
                case "KC_LEFT":
                    label = "\\u25c0";

                    break;
                case "KC_UP":
                    label = "\\u25b2";

                    break;
                case "KC_RIGHT":
                    label = "\\u25b6";

                    break;
                case "KC_DOWN":
                    label = "\\u25bc";

                    break;
                case "RGB_MOD":
                    label = "\\ue916";
                    isGlyph = true;

                    break;
                case "RGB_SLD":
                    label = "\\ue90e";
                    isGlyph = true;

                    break;
                case "RGB_VAI":
                    label = "\\ue911";
                    isGlyph = true;

                    break;
                case "RGB_VAD":
                    label = "\\ue910";
                    isGlyph = true;

                    break;
                case "RGB_HUI":
                    label = "\\ue923";
                    isGlyph = true;

                    break;
                case "RGB_HUD":
                    label = "\\ue924";
                    isGlyph = true;

                    break;
                case "RGB_TOG":
                    label = "\\ue90d";
                    isGlyph = true;

                    break;
                case "RGB":
                    label = "\\ue915";
                    isGlyph = true;

                    break;
                case "FR_SUP2":
                    label = "\\u00B2";

                    break;
                case "DE_SQ2":
                    label = "\\u00B2";

                    break;
                case "FR_EACU":
                    label = "\\u00E9";

                    break;
                case "FR_EGRV":
                    label = "\\u00E8";

                    break;
                case "FR_CCED":
                    label = "\\u00E7";

                    break;
                case "FR_AGRV":
                    label = "\\u00E0";

                    break;
                case "FR_UGRV":
                    label = "\\u00F9";

                    break;
                case "DE_SS":
                    label = "\\u00DF";

                    break;
                case "DE_AE":
                    label = "\\u00C4";

                    break;
                case "DE_UE":
                    label = "\\u00DC";

                    break;
                case "DE_OE":
                    label = "\\u00D6";

                    break;
                case "DE_CIRC":
                    label = "\\u005E\\u00B0";

                    break;
                case "NO_HALF":
                case "ES_EURO":
                    label = "\\u00BD";

                    break;
                case "NO_AM":
                    label = "\\u00E5";

                    break;
                case "NO_AE":
                    label = "\\u00F8";

                    break;
                case "NO_OSLH":
                    label = "\\u00E6";

                    break;
                case "ES_OVRR":
                    label = "\\u00BA";

                    break;
                case "ES_IEXL":
                    label = "\\u00A1";

                    break;
                case "ES_NTIL":
                    label = "\\u00F1";

                    break;
                case "ES_ASML":
                    label = "\\u00AA";

                    break;
                case "FR_OVRR":
                    label = "\\u00B0";

                    break;
                case "DE_RING":
                    label = "\\u00B0";

                    break;
                case "FR_UMLT":
                    label = "\\u00A8";

                    break;
                case "FR_PND":
                    label = "\\u00A3";

                    break;
                case "FR_MU":
                    label = "\\u03BC";

                    break;
                case "FR_SECT":
                    label = "\\u00A7";

                    break;
                case "DE_PARA":
                    label = "\\u00A7";

                    break;
                case "DE_EURO":
                    label = "\\u20AC";

                    break;
                case "FR_EURO":
                case "NO_EURO":
                    label = "\\u20AC";

                    break;
                case "FR_BULT":
                    label = "\\u00A4";

                    break;
                case "DE_ACUT":
                    label = "\\u00B4\\u0060";

                    break;
                case "ES_ACUT":
                    label = "\\u00B4";

                    break;
                case "DE_SQ3":
                    label = "\\u00B3";

                    break;
                case "NO_LCBR":
                    label = "\\u00B6";

                    break;
                case "NO_RCBR":
                    label = "\\u2260";

                    break;
                case "ES_OVDT":
                    label = "\\u00B7";

                    break;
                case "ES_IQUE":
                    label = "\\u00BF";

                    break;
                case "ES_NOT":
                    label = "\\u00AC";

                    break;
                case "HU_UE":
                    label = "\\u00FC";

                    break;
                case "HU_OO":
                    label = "\\u00F3";

                    break;
                case "HU_OE":
                    label = "\\u00F6";

                    break;
                case "HU_OEE":
                    label = "\\u0151";

                    break;
                case "HU_UU":
                    label = "\\u00FA";

                    break;
                case "HU_EE":
                    label = "\\u00E9";

                    break;
                case "HU_BRV":
                    label = "\\u02D8";

                    break;
                case "HU_RING":
                    label = "\\u00B0";

                    break;
                case "HU_EURO":
                    label = "\\u20AC";

                    break;
                case "HU_SS":
                    label = "\\u00DF";

                    break;
                case "HU_AA":
                    label = "\\u00E1";

                    break;
                case "HU_UEE":
                    label = "\\u0171";

                    break;
                case "HU_PARA":
                    label = "\\u00A7";

                    break;
                case "HU_II":
                    label = "\\u00ED";

                    break;
                case "HU_ACUT":
                    label = "\\u00B4";

                    break;
                case "HU_DIV":
                    label = "\\u00F7";

                    break;
                case "HU_CRSS":
                    label = "\\u00D7";

                    break;
            }

            return (label, isGlyph);
        }
    }
}