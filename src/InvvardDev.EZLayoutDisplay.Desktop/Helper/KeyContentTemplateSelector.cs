using System;
using System.Windows;
using System.Windows.Controls;
using InvvardDev.EZLayoutDisplay.Desktop.Model;
using InvvardDev.EZLayoutDisplay.Desktop.Model.Enum;

namespace InvvardDev.EZLayoutDisplay.Desktop.Helper
{
    public class KeyContentTemplateSelector : DataTemplateSelector
    {
        public DataTemplate SingleFeatureDataTemplate { private get; set; }
        public DataTemplate DoubleFeatureDataTemplate { private get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
            => item switch
            {
                KeyTemplate { Key.DisplayMode: KeyDisplayMode.DualFunction } => DoubleFeatureDataTemplate,
                _ => SingleFeatureDataTemplate
            };
    }
}