using Infra.Wpf.Common.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Infra.Wpf.Controls
{
    public class BoolField : CustomCheckBox, INotifyPropertyChanged, IField
    {
        #region Properties

        public string Title { get; set; }

        public string FilterField { get; set; }

        public string SearchPhrase
        {
            get
            {
                if (string.IsNullOrWhiteSpace(IsChecked.ToString()))
                    return "";

                return $@"{FilterField}=={IsChecked.ToString()}";
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

        public Type ModelType { get; set; }

        public bool IsGetFocus
        {
            get { return (bool)GetValue(IsGetFocusProperty); }
            set { SetValue(IsGetFocusProperty, value); }
        }

        public static readonly DependencyProperty IsGetFocusProperty =
            DependencyProperty.Register("IsGetFocus", typeof(bool), typeof(BoolField), new PropertyMetadata(false, OnIsGetFocusChanged));

        public event PropertyChangedEventHandler PropertyChanged;

        public event SearchPhraseChangedEventHandler SearchPhraseChanged;

        #endregion

        #region Methods

        public BoolField()
        {
            Loaded += BoolField_Loaded;
            Checked += BoolField_Change;
            Unchecked += BoolField_Change;
        }

        private void BoolField_Loaded(object sender, RoutedEventArgs e)
        {
            DisplayName = GetDisplayName();

            Binding binding = new Binding("IsFocused")
            {
                Source = this,
                Mode = BindingMode.OneWay
            };

            SetBinding(IsGetFocusProperty, binding);
        }

        public void OnPropertyChanged([CallerMemberName]string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public void Clear()
        {
            if (IsThreeState == true)
                IsChecked = null;
            else
                IsChecked = false;
        }

        private void BoolField_Change(object sender, RoutedEventArgs e)
        {
            SearchPhraseChanged?.Invoke();
        }

        private static void OnIsGetFocusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (((bool)e.NewValue) == true)
                ((BoolField)d).MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
        }

        private string GetDisplayName()
        {
            BindingExpression bindEx = BindingOperations.GetBindingExpression(this, IsCheckedProperty);
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
