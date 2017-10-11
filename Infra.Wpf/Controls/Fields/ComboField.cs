using C1.WPF;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Data;

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
        
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Methods

        public ComboField()
        {
            VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
            IsNullable = true;
            UseEnumValue = true;

            Binding bind = new Binding("FilterItem")
            {
                Source = this
            };
            SetBinding(SelectedItemProperty, bind);
        }

        public void Clear()
        {
            FilterItem = null;
        }

        public void OnPropertyChanged([CallerMemberName]string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        #endregion
    }
}
