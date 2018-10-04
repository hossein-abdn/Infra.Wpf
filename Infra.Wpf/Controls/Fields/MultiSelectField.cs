using C1.WPF;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using Infra.Wpf.Common.Helpers;

namespace Infra.Wpf.Controls
{
    public class MultiSelectField : MultiSelect, INotifyPropertyChanged, IField
    {
        #region Properties

        public string Title { get; set; }

        public string FilterField { get; set; }

        public string TargetColumn { get; set; }

        private string _DisplayName;
        public string DisplayName
        {
            get { return _DisplayName; }
            set
            {
                _DisplayName = value;
                OnPropertyChanged();
            }
        }

        public Type ModelType { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public event SearchPhraseChangedEventHandler SearchPhraseChanged;

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

        #endregion

        #region Methods

        public MultiSelectField()
        {
            Loaded += MultiSelectField_Loaded;
            SelectionChanged += MultiSelectField_SelectionChanged;
            Focusable = true;
        }

        private void MultiSelectField_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            SearchPhraseChanged?.Invoke();
        }

        private void MultiSelectField_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            DisplayName = GetDisplayName();
        }

        public void Clear()
        {
            SelectedItems.Clear();
        }

        public void OnPropertyChanged([CallerMemberName]string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private string GetDisplayName()
        {
            BindingExpression bindEx = BindingOperations.GetBindingExpression(this, SelectedItemsProperty);
            if (bindEx != null && !string.IsNullOrEmpty(bindEx.ResolvedSourcePropertyName))
            {
                if (ModelType != null)
                {
                    var propInfo = ModelType.GetProperty(bindEx.ResolvedSourcePropertyName);
                    var attrib = propInfo?.GetCustomAttributes(typeof(DisplayAttribute), false);
                    var isRequired = propInfo.IsRequired();
                    var result = string.Empty;

                    if (attrib != null && attrib.Count() > 0)
                        result = ((DisplayAttribute) attrib[0]).Name;
                    else
                        result = bindEx.ResolvedSourcePropertyName;

                    if (!string.IsNullOrEmpty(result) && isRequired)
                        result = "* " + result;

                    return result;
                }
                else
                {
                    var result = bindEx.ResolvedSourcePropertyName;
                    if (!string.IsNullOrEmpty(result))
                        return result;
                }
            }

            if (!string.IsNullOrWhiteSpace(FilterField))
            {
                var propInfo = ModelType?.GetProperty(FilterField);
                if (propInfo != null)
                {
                    var attrib = propInfo.GetCustomAttributes(typeof(DisplayAttribute), false);
                    if (attrib != null && attrib.Count() > 0)
                        return ((DisplayAttribute) attrib[0]).Name;
                }

                return FilterField;
            }

            return string.Empty;
        }

        #endregion
    }
}
