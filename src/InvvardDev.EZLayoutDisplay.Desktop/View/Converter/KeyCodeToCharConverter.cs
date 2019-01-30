using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Forms;

namespace InvvardDev.EZLayoutDisplay.Desktop.View.Converter
{
    public class KeyCodeToCharConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) { return ""; }

            var kc = new KeysConverter();
            var keyValue = kc.ConvertToString(value);

            return keyValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) { return ""; }

            var kc = new KeysConverter();
            var keyCode = kc.ConvertFrom(value);

            return keyCode;
        }
    }
}