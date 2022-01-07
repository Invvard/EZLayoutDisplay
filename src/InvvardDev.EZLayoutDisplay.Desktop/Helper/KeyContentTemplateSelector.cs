using System;
using System.Windows;
using System.Windows.Controls;
using InvvardDev.EZLayoutDisplay.Desktop.Model;
using InvvardDev.EZLayoutDisplay.Desktop.Model.Enum;

namespace InvvardDev.EZLayoutDisplay.Desktop.Helper
{
    public class KeyContentTemplateSelector : DataTemplateSelector
    {
        public DataTemplate LabelDataTemplate { private get; set; }
        public DataTemplate LabelModDataTemplate { private get; set; }
        public DataTemplate TapHoldDataTemplate { private get; set; }
        public DataTemplate TapModHoldDataTemplate { private get; set; }
        public DataTemplate TapHoldModDataTemplate { private get; set; }
        public DataTemplate TapModHoldModDataTemplate { private get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            DataTemplate template = LabelDataTemplate;

            if (!(item is KeyTemplate key)) return template;

            switch (key.EZKey.DisplayType)
            {
                case KeyDisplayType.None:
                case KeyDisplayType.Tap:
                case KeyDisplayType.Hold:
                case KeyDisplayType.CustomLabel:
                    template = LabelDataTemplate;
                    break;
                case KeyDisplayType.TapMod:
                case KeyDisplayType.HoldMod:
                    template = LabelModDataTemplate;
                    break;
                case KeyDisplayType.TapHold:
                    template = TapHoldDataTemplate;
                    break;
                case KeyDisplayType.TapModHold:
                    template = TapModHoldDataTemplate;
                    break;
                case KeyDisplayType.TapHoldMod:
                    template = TapHoldModDataTemplate;
                    break;
                case KeyDisplayType.TapModHoldMod:
                    template = TapModHoldModDataTemplate;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return template;
        }
    }
}