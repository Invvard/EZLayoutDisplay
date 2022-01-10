using System;
using System.Windows;
using System.Windows.Controls;
using InvvardDev.EZLayoutDisplay.Desktop.Model;
using InvvardDev.EZLayoutDisplay.Desktop.Model.Enum;

namespace InvvardDev.EZLayoutDisplay.Desktop.Helper
{
    public class KeyContentTemplateSelector : DataTemplateSelector
    {
        public DataTemplate CustomLabelDataTemplate { private get; set; }
        public DataTemplate SingleFeatureDataTemplate { private get; set; }
        public DataTemplate DoubleFeatureDataTemplate { private get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (!(item is KeyTemplate key)) return CustomLabelDataTemplate;

            return key.EZKey.DisplayMode switch
            {
                KeyDisplayMode.SingleFeature => SingleFeatureDataTemplate,
                KeyDisplayMode.DoubleFeature => DoubleFeatureDataTemplate,
                _ => CustomLabelDataTemplate
            };
        }
    }
}