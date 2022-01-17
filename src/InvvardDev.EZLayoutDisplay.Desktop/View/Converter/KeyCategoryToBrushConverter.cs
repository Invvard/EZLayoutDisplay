using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using InvvardDev.EZLayoutDisplay.Desktop.Model.Enum;

namespace InvvardDev.EZLayoutDisplay.Desktop.View.Converter
{
    [ValueConversion(typeof(SolidColorBrush), typeof(KeyCategory))]
    public class KeyCategoryToBrushConverter : IValueConverter
    {
        public SolidColorBrush DefaultBackgroundBrush { private get; set; }
        public SolidColorBrush ColorControlBackgroundBrush { private get; set; }
        public SolidColorBrush CustomLabelBackgroundBrush { private get; set; }
        public SolidColorBrush DualFunctionBackgroundBrush { private get; set; }
        public SolidColorBrush MacroBackgroundBrush { private get; set; }
        public SolidColorBrush ModifierBackgroundBrush { private get; set; }

        public KeyCategoryToBrushConverter()
        {
            DefaultBackgroundBrush = Application.Current.Resources["DefaultBackgroundBrush"] as SolidColorBrush;
            ColorControlBackgroundBrush = Application.Current.Resources["ColorControlBackgroundBrush"] as SolidColorBrush;
            CustomLabelBackgroundBrush = Application.Current.Resources["CustomLabelBackgroundBrush"] as SolidColorBrush;
            DualFunctionBackgroundBrush = Application.Current.Resources["DualFunctionBackgroundBrush"] as SolidColorBrush;
            MacroBackgroundBrush = Application.Current.Resources["MacroBackgroundBrush"] as SolidColorBrush;
            ModifierBackgroundBrush = Application.Current.Resources["ModifierBackgroundBrush"] as SolidColorBrush;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SolidColorBrush brush = null;
            if (value is KeyDisplayMode displayMode)
            {
                brush = displayMode switch
                {
                    KeyDisplayMode.ColorControl => ColorControlBackgroundBrush,
                    KeyDisplayMode.CustomLabel => CustomLabelBackgroundBrush,
                    KeyDisplayMode.DualFunction => DualFunctionBackgroundBrush,
                    KeyDisplayMode.Macro => MacroBackgroundBrush,
                    KeyDisplayMode.Modifier => ModifierBackgroundBrush,
                    _ => DefaultBackgroundBrush
                };
            }

            return brush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}