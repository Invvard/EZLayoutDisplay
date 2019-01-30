using System.Collections.Generic;
using System.Text;
using InvvardDev.EZLayoutDisplay.Desktop.Properties;
using Newtonsoft.Json;

namespace InvvardDev.EZLayoutDisplay.Desktop.Model.Dictionary
{
    public class KeyDefinitionDictionary
    {
        public List<KeyDefinition> KeyDefinitions { get; private set; }

        public KeyDefinitionDictionary()
        {
            InitializeKeyDefinitions();
        }

        private void InitializeKeyDefinitions()
        {
            KeyDefinitions = new List<KeyDefinition>();

            if (Resources.keyDefinitions.Length <= 0)
            {
                // TODO : add logging
                return;
            }

            var json = Encoding.Default.GetString(Resources.keyDefinitions);

            KeyDefinitions.AddRange(JsonConvert.DeserializeObject<List<KeyDefinition>>(json));
        }
    }
}