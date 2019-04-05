using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace InvvardDev.EZLayoutDisplay.Desktop.View.Converter
{
    [ ValueConversion(typeof(bool), typeof(Visibility)) ]
    public sealed class BoolToVisibilityConverter : IValueConverter
    {
        public Visibility TrueValue { private get; set; }
        public Visibility FalseValue { private get; set; }

        public BoolToVisibilityConverter()
        {
            // set defaults
            TrueValue = Visibility.Visible;
            FalseValue = Visibility.Collapsed;
        }

        public object Convert(object      value,
                              Type        targetType,
                              object      parameter,
                              CultureInfo culture)
        {
            if (!(value is bool)) return null;

            return (bool) value ? TrueValue : FalseValue;
        }

        public object ConvertBack(object      value,
                                  Type        targetType,
                                  object      parameter,
                                  CultureInfo culture)
        {
            if (Equals(value, TrueValue)) return true;
            if (Equals(value, FalseValue)) return false;

            return null;
        }
    }
}