using System;
using System.ComponentModel;
using System.Globalization;
using Infra.Wpf.Common;

namespace Infra.Wpf.Converters
{
    public class PersianDateConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;

            if (sourceType == typeof(DateTime))
                return true;

            return base.CanConvertFrom(context, sourceType);
        }
        
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
                return PersianDate.Parse(value as string);

            if (value is DateTime)
                return new PersianDate((DateTime)value);

            return base.ConvertFrom(context, culture, value);
        }
        
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                PersianDate pd = (PersianDate)value;

                if (pd == null)
                    return string.Empty;

                return pd.ToString();
            }
            
            if (destinationType == typeof(DateTime))
            {
                PersianDate pd = (PersianDate)value;
                return pd.ToDateTime();
            }
            
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
