using System;
using System.Collections.Generic;
using System.Text;
using InvvardDev.EZLayoutDisplay.Desktop.Properties;
using Newtonsoft.Json;
using NLog;

namespace InvvardDev.EZLayoutDisplay.Desktop.Model.Dictionary
{
    public class KeyDefinitionDictionary
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

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
                _logger.Warn("KeyDefinitioins are missing from Resources");
                return;
            }

            try
            {
                var json = Encoding.Default.GetString(Resources.keyDefinitions);
                var keyDefinitions = JsonConvert.DeserializeObject<List<KeyDefinition>>(json);

                KeyDefinitions.AddRange(keyDefinitions);
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex);
            }
        }
    }
}