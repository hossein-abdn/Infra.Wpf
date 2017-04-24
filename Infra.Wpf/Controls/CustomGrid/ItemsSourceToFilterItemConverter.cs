using Infra.Wpf.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows.Data;

namespace Infra.Wpf.Converters
{
    public class ItemsSourceToFilterItemConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || parameter == null)
                return null;

            string[] param = parameter as string[];

            Type valueType = value.GetType();
            if (valueType.IsGenericType)
            {
                Type genricType = valueType.GetGenericArguments()[0];
                PropertyInfo memberPathInfo = genricType.GetProperty(param[0]);
                if (memberPathInfo != null)
                {
                    List<object> listItem = new List<object>();
                    foreach (var item in value as IEnumerable)
                        listItem.Add(memberPathInfo.GetValue(item));

                    List<FilterItem> filterItems = new List<FilterItem>();
                    foreach (var item in listItem.Distinct().ToList())
                        filterItems.Add(new FilterItem() { Value = item, Mask = param[1] });

                    ObservableCollection<FilterItem> result =
                        new ObservableCollection<FilterItem>(filterItems.OrderBy(x => x.Value).ToList());

                    return result;
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}