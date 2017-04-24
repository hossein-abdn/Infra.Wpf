using Infra.Wpf.Common;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Infra.Wpf.Converters
{
    public class PersianToDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
                return ((PersianDate) value).ToDateTime();

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
                return PersianDate.MinValue.ToDateTime();

            return null;
        }
    }
}
