using C1.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Wpf.Controls
{
    public class MultiSelectField : MultiSelect, IField
    {
        #region Properties

        public string Title { get; set; }

        public string FilterField { get; set; }

        public string TargetColumn { get; set; }

        #endregion

        #region Methods

        public string SearchPhrase
        {
            get
            {
                if (SelectedItems == null || SelectedItems.Count == 0 || string.IsNullOrWhiteSpace(FilterField))
                    return null;

                string query = string.Empty;
                foreach (var item in SelectedItems)
                {
                    string filterValue = string.Empty;
                    PropertyInfo targetInfo = null;
                    Type targetType = null;

                    if (!string.IsNullOrEmpty(TargetColumn))
                    {
                        targetInfo = item?.GetType()?.GetProperty(TargetColumn);
                        filterValue = targetInfo?.GetValue(item)?.ToString();
                        targetType = targetInfo?.PropertyType;
                    }
                    else if (!string.IsNullOrEmpty(DisplayMemberPath))
                    {
                        targetInfo = item?.GetType()?.GetProperty(DisplayMemberPath);
                        filterValue = targetInfo?.GetValue(item)?.ToString();
                        targetType = targetInfo?.PropertyType;
                    }
                    else
                    {
                        filterValue = item.ToString();
                        targetType = item.GetType();
                    }

                    if (string.IsNullOrEmpty(filterValue))
                        continue;
                    
                    if (targetType.IsNumeric())
                        query = query + $@"{FilterField}=={filterValue.Trim()}" + " OR ";
                    else
                        query = query + $@"{FilterField}==""{filterValue.Trim()}""" + " OR ";
                }

                if (!string.IsNullOrEmpty(query))
                {
                    query = query.Substring(0, query.Length - 4);
                    return query;
                }

                return null;
            }
        }

        public void Clear()
        {
            SelectedItems.Clear();
        }

        #endregion
    }
}
