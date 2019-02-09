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
        public DataTemplate GlyphDataTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var key = item as KeyTemplate;

            DataTemplate template = SimpleLabelDataTemplate;

            if (key == null) return template;

            switch (key.EZKey.DisplayType)
            {
                case KeyDisplayType.None:
                case KeyDisplayType.SimpleLabel:
                    template = SimpleLabelDataTemplate;

                    break;
                case KeyDisplayType.LabelInternational:

                    break;
                case KeyDisplayType.LabelWithSubLabelUnder:

                    break;
                case KeyDisplayType.LabelWithSubLabelOnTop:

                    break;
                case KeyDisplayType.Glyph:
                    template = GlyphDataTemplate;

                    break;
            }

            return template;
        }
    }
}