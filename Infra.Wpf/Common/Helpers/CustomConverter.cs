using System;
using System.Globalization;
using System.Windows.Data;

namespace Infra.Wpf.Common.Helpers
{
    public class CustomConverter : IValueConverter
    {
        private Func<object, Type, object, CultureInfo, object> _converterFunc;
        private Func<object, Type, object, CultureInfo, object> _converterBackFunc;

        public CustomConverter(Func<object, Type, object, CultureInfo, object> converterFunc,
            Func<object, Type, object, CultureInfo, object> converterBackFunc)
        {
            _converterFunc = converterFunc;
            _converterBackFunc = converterBackFunc;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (this._converterFunc == null)
                return null;

            return _converterFunc(value, targetType, parameter, culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (_converterBackFunc == null)
                return null;

            return this._converterBackFunc(value, targetType, parameter, culture);
        }
    }
}