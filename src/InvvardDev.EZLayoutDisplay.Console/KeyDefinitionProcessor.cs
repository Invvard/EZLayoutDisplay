using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using InvvardDev.EZLayoutDisplay.Desktop.Model;
using InvvardDev.EZLayoutDisplay.Desktop.Model.Enum;
using Newtonsoft.Json;

namespace InvvardDev.EZLayoutDisplay.Console
{
    public class KeyDefinitionProcessor
    {
        private const string KeyDefinitionInputFilename = "keyDefinitions.json";
        private const string KeyDefinitionOutputFilename = "keyDefinitions.output.json";

        public KeyDefinitionProcessor() { }

        public async void RunProcess()
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
            }

            return (label, isGlyph);
        }
    }
}