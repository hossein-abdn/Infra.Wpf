﻿using Infra.Wpf.Common;
using System;
using System.Windows.Data;

namespace Infra.Wpf.Converters
{
    [ValueConversion(typeof(DateTime), typeof(PersianDate))]
    public class DateTimeToPersianDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) 
                return null;
            
            DateTime date = (DateTime)value;
            return new PersianDate(date).ToString(parameter as string);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) 
                return null;
            
            PersianDate pDate = (PersianDate)value;
            return pDate.ToDateTime();
        }
    }

}
