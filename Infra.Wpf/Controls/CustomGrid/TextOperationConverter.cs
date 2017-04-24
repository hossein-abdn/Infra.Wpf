using C1.WPF.DataGrid;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Infra.Wpf.Converters
{
    public class TextOperationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch((DataGridFilterOperation)value)
            {
                case DataGridFilterOperation.Contains:
                    return "شامل";
                case DataGridFilterOperation.StartsWith:
                    return "شروع شود با";
                case DataGridFilterOperation.EndsWith:
                    return "پایان یابد با";
                case DataGridFilterOperation.Equal:
                    return "مساوی";
                case DataGridFilterOperation.NotEqual:
                    return "نامساوی";
                default:
                    return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch(value.ToString())
            {
                case "شامل":
                    return DataGridFilterOperation.Contains;
                case "شروع شود با":
                    return DataGridFilterOperation.StartsWith;
                case "پایان یابد با":
                    return DataGridFilterOperation.EndsWith;
                case "مساوی":
                    return DataGridFilterOperation.Equal;
                case "نامساوی":
                    return DataGridFilterOperation.NotEqual;
                default:
                    return DataGridFilterOperation.None;
            }
        }
    }
}
