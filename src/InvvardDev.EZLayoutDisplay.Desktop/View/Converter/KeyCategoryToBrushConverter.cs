using System;
using System.Globalization;
using System.Windows.Data;

namespace InvvardDev.EZLayoutDisplay.Desktop.View.Converter
{
    public class KeyCategoryToBrushConverter : IValueConverter
    {
        public TYPE Type { get; set; }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}