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
    public class ComboField : CustomComboBox, INotifyPropertyChanged, IField
    {
        #region Properties

        public string Title { get; set; }

        public string FilterField { get; set; }

        private object _FilterItem;
        public object FilterItem
        {
            get { return _FilterItem; }
            set
            {
                _FilterItem = value;
                OnPropertyChanged();
                SearchPhraseChanged?.Invoke();
            }
        }

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

        public string SearchPhrase
        {
            get
            {
                if (FilterItem == null || string.IsNullOrWhiteSpace(FilterField))
                    return "";

                if (EnumType != null && UseEnumValue == true)
                {
                    var value = (int) FilterItem;
                    return $@"{FilterField}=={value}";
                }
                else
                {
                    string filterValue = string.Empty;
                    PropertyInfo targetInfo = null;
                    Type targetType = null;

                    if (!string.IsNullOrEmpty(TargetColumn))
                    {
                        targetInfo = FilterItem?.GetType()?.GetProperty(TargetColumn);
                        filterValue = targetInfo?.GetValue(FilterItem)?.ToString();
                        targetType = targetInfo?.PropertyType;
                    }
                    else if (!string.IsNullOrEmpty(DisplayMemberPath))
                    {
                        targetInfo = FilterItem?.GetType()?.GetProperty(DisplayMemberPath);
                        filterValue = targetInfo?.GetValue(FilterItem)?.ToString();
                        targetType = targetInfo?.PropertyType;
                    }

                    else
                    {
                        filterValue = FilterItem.ToString();
                        targetType = FilterItem.GetType();
                    }

                    if (string.IsNullOrEmpty(filterValue))
                        return null;

                    if (targetType.IsNumeric())
                        return $@"{FilterField}.Equals({filterValue.Trim()})";
                    else
                        return $@"{FilterField}.Equals(""{filterValue.Trim()}"")";
                }
            }
        }

        public string TargetColumn { get; set; }

        public bool UseEnumValue { get; set; }

        public Type ModelType { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public event SearchPhraseChangedEventHandler SearchPhraseChanged;

        #endregion

        #region Methods

        public ComboField()
        {
            VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
            IsNullable = true;
            UseEnumValue = true;
            Focusable = true;

            Binding bind = new Binding("FilterItem")
            {
                Source = this
            };
            SetBinding(SelectedItemProperty, bind);

            Loaded += ComboField_Loaded;
        }

        private void ComboField_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            DisplayName = GetDisplayName();

            if (IsFocused == true)
                MoveFocus(new System.Windows.Input.TraversalRequest(System.Windows.Input.FocusNavigationDirection.First));
        }

        public void Clear()
        {
            FilterItem = null;
        }

        public void OnPropertyChanged([CallerMemberName]string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private string GetDisplayName()
        {
            BindingExpression bindEx = BindingOperations.GetBindingExpression(this, SelectedItemProperty);
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
