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
        private const string KeyDefinitionJsFilename = "keyDefinitions.json";

        public KeyDefinitionProcessor() { }

        public async void RunProcess()
        {
            if (!CheckKeyDefinitionJsExists())
            {
                return;
            }

            var keyCodesOrigin = ParseJson();

            if (!keyCodesOrigin.Any())
            {
                return;
            }

            List<KeyDefinition> keyDefinitions = ToEZKeyDefinitions(keyCodesOrigin);
        }

        private bool CheckKeyDefinitionJsExists()
        {
            var fileExist = File.Exists(KeyDefinitionJsFilename);

            return fileExist;
        }

        private List<ConfiguratorKeyDefinition> ParseJson()
        {
            List<ConfiguratorKeyDefinition> keyCodes;

            using (StreamReader sr = new StreamReader(KeyDefinitionJsFilename))
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
                var keyDefinition = new KeyDefinition {
                                                          Label = ChooseLabel(configKeyDef),
                                                          Description = configKeyDef.Description,
                                                          IsGlyph = string.IsNullOrWhiteSpace(configKeyDef.Glyph),
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

        private string ChooseLabel(ConfiguratorKeyDefinition configKeyDef)
        {
            if (string.IsNullOrWhiteSpace(configKeyDef.Label))
            {
                
            }
        }
    }
}