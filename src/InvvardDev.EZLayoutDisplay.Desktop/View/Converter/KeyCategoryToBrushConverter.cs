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
        public SolidColorBrush DefaultBackgroundBrush { get; set; }
        public SolidColorBrush DualFunctionBackgroundBrush { get; set; }
        public SolidColorBrush ModifierBackgroundBrush { get; set; }
        public SolidColorBrush ShineBackgroundBrush { get; set; }

        public KeyCategoryToBrushConverter()
        {
            DefaultBackgroundBrush = Application.Current.Resources["BaseDefaultBackgroundBrush"] as SolidColorBrush;
            DualFunctionBackgroundBrush = Application.Current.Resources["BaseDualFunctionBackgroundBrush"] as SolidColorBrush;
            ModifierBackgroundBrush = Application.Current.Resources["BaseModifierBackgroundBrush"] as SolidColorBrush;
            ShineBackgroundBrush = Application.Current.Resources["BaseShineBackgroundBrush"] as SolidColorBrush;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is KeyCategory)) return null;

            SolidColorBrush brush;
            switch ((KeyCategory)value)
            {
                case KeyCategory.LayerShortcuts:
                case KeyCategory.DualFunction:
                    brush = DualFunctionBackgroundBrush;
                    break;
                case KeyCategory.Modifier:
                    brush = ModifierBackgroundBrush;
                    break;
                case KeyCategory.Shine:
                    brush = ShineBackgroundBrush;
                    break;
                default:
                    brush = DefaultBackgroundBrush;
                    break;
            }

            return brush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}