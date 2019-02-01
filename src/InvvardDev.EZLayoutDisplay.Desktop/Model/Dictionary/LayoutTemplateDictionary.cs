using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using InvvardDev.EZLayoutDisplay.Desktop.Properties;
using Newtonsoft.Json;

namespace InvvardDev.EZLayoutDisplay.Desktop.Model.Dictionary
{
    public class LayoutTemplateDictionary
    {
        public ObservableCollection<KeyTemplate> KeyBaseTemplates { get; private set; }

        public LayoutTemplateDictionary()
        {
            InitializeLayoutTemplate();
        }

        private void InitializeLayoutTemplate()
        {
            KeyBaseTemplates = new ObservableCollection<KeyTemplate>();

            if (Resources.layoutTemplate.Length <= 0)
            {
                // TODO : add logging
                return;
            }

            var json = Encoding.Default.GetString(Resources.layoutTemplate);

            KeyBaseTemplates = JsonConvert.DeserializeObject<ObservableCollection<KeyTemplate>>(json);
        }
    }
}