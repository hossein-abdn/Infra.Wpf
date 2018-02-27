using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Infra.Wpf.Common.Helpers;

namespace Infra.Wpf.Controls
{
    public partial class BoolField : UserControl, INotifyPropertyChanged, IField
    {
        #region Properties

        public string Title { get; set; }

        public string FilterField { get; set; }

        public string FilterText { get; set; }

        public string SearchPhrase
        {
            get
            {
                if (string.IsNullOrWhiteSpace(FilterText))
                    return "";

                return $@"{FilterField}=={FilterText.Trim()}";
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

        public bool IsChecked
        {
            get { return (bool) GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }

        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.Register("IsChecked", typeof(bool), typeof(BoolField), 
                new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIsCheckedChange));

        public event PropertyChangedEventHandler PropertyChanged;

        public event SearchPhraseChangedEventHandler SearchPhraseChanged;

        #endregion

        #region Methods

        public BoolField()
        {
            InitializeComponent();
            Loaded += BoolField_Loaded;
        }

        private void BoolField_Loaded(object sender, RoutedEventArgs e)
        {
            DisplayName = GetDisplayName();

            if (IsFocused == true)
                this.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
        }

        public void OnPropertyChanged([CallerMemberName]string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public void Clear()
        {
            IsChecked = false;
            FilterText = "";
        }

        private static void OnIsCheckedChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var @this = (BoolField) d;
            if (@this != null)
            {
                @this.FilterText = ((bool) e.NewValue).ToString();
                @this.SearchPhraseChanged?.Invoke();
            }
        }

        private string GetDisplayName()
        {
            BindingExpression bindEx = BindingOperations.GetBindingExpression(this, IsCheckedProperty);
            if (bindEx != null && !string.IsNullOrEmpty(bindEx.ResolvedSourcePropertyName))
            {
                var type = DataContext?.GetType().GetProperty("Model")?.PropertyType;
                if (type != null)
                {
                    var propInfo = type.GetProperty(bindEx.ResolvedSourcePropertyName);
                    var attrib = propInfo?.GetCustomAttributes(typeof(DisplayAttribute), false);
                    var isRequired = propInfo.IsRequired(bindEx.ResolvedSourcePropertyName);
                    if (attrib != null && attrib.Count() > 0)
                    {
                        var result = ((DisplayAttribute) attrib[0]).Name;
                        if (isRequired)
                            result = "* " + result;

                        return result;
                    }
                }
                else
                {
                    var displayText = bindEx.ResolvedSourcePropertyName;
                    if (!string.IsNullOrEmpty(displayText))
                        return displayText;
                }
            }

            if (!string.IsNullOrWhiteSpace(FilterField))
            {
                var type = DataContext?.GetType().GetProperty("ItemsSource")?.PropertyType;

                if (type != null && type.IsGenericType)
                {
                    var propInfo = type.GenericTypeArguments[0].GetProperty(FilterField);
                    if (propInfo != null)
                    {
                        var attrib = propInfo.GetCustomAttributes(typeof(DisplayAttribute), false);
                        if (attrib != null && attrib.Count() > 0)
                            return ((DisplayAttribute) attrib[0]).Name;
                    }
                }

                return FilterField;
            }

            return string.Empty;
        }
        #endregion
    }
}
