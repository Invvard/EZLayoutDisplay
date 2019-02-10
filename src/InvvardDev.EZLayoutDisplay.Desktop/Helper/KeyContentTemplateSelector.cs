using System;
using System.Windows;
using System.Windows.Controls;
using InvvardDev.EZLayoutDisplay.Desktop.Model;
using InvvardDev.EZLayoutDisplay.Desktop.Model.Enum;

namespace InvvardDev.EZLayoutDisplay.Desktop.Helper
{
    public class KeyContentTemplateSelector : DataTemplateSelector
    {
        public DataTemplate SimpleLabelDataTemplate { get; set; }
        public DataTemplate SimpleGlyphDataTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            DataTemplate template = SimpleLabelDataTemplate;

            if (!(item is KeyTemplate key)) return template;

            switch (key.EZKey.DisplayType)
            {
                case KeyDisplayType.None:
                case KeyDisplayType.SimpleLabel:
                    template = SimpleLabelDataTemplate;

                    break;
                case KeyDisplayType.ModifierOnTop:

                    break;
                case KeyDisplayType.ModifierUnder:

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return template;
        }
    }
}