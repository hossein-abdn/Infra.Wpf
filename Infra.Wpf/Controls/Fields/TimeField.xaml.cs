using Infra.Wpf.Common.Helpers;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Infra.Wpf.Controls
{
    public partial class TimeField : UserControl, INotifyPropertyChanged, IField
    {
        #region Properties

        private NumericOperator _Operator;
        public NumericOperator Operator
        {
            get { return _Operator; }
            set
            {
                _Operator = value;
                OnPropertyChanged();
                SearchPhraseChanged?.Invoke();
            }
        }

        private bool _OperatorVisible;
        public bool OperatorVisible
        {
            get { return _OperatorVisible; }
            set
            {
                _OperatorVisible = value;
                OnPropertyChanged();
            }
        }

        private bool _ShowButtons;
        public bool ShowButtons
        {
            get { return _ShowButtons; }
            set
            {
                _ShowButtons = value;
                OnPropertyChanged();
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

        public TimeSpan? Value
        {
            get
            {
                return (TimeSpan?) GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);
            }
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(TimeSpan?), typeof(TimeField),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChanged));

        private TimeEditorFormat _Format;
        public TimeEditorFormat Format
        {
            get { return _Format; }
            set
            {
                _Format = value;
                OnPropertyChanged();
            }
        }

        public TimeSpan MaxValue
        {
            get
            {
                return (TimeSpan) GetValue(MaxValueProperty);
            }
            set
            {
                SetValue(MaxValueProperty, value);
            }
        }

        public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register("MaxValue", typeof(TimeSpan), typeof(TimeField),
                new PropertyMetadata(new TimeSpan(0, 23, 59, 59)));

        public TimeSpan MinValue
        {
            get
            {
                return (TimeSpan) GetValue(MinValueProperty);
            }
            set
            {
                SetValue(MinValueProperty, value);
            }
        }

        public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register("MinValue", typeof(TimeSpan), typeof(TimeField),
                new PropertyMetadata(new TimeSpan(0, 0, 0)));

        private NumericOperator defaultOperator;

        private bool isSetDefaultOperator;

        public string Title { get; set; }

        public string FilterField { get; set; }

        public string SearchPhrase
        {
            get
            {
                string filterText = Value?.ToString();
                if (string.IsNullOrWhiteSpace(filterText) || string.IsNullOrWhiteSpace(FilterField))
                    return "";

                TimeSpan field;
                if (TimeSpan.TryParse(filterText, out field) == false)
                    return "";

                int h = field.Hours;
                int m = field.Minutes;
                int s = field.Seconds;

                switch (Operator)
                {
                    case NumericOperator.Equals:
                        return $@"{FilterField}==TimeSpan({h},{m},{s})";
                        break;
                    case NumericOperator.NotEquals:
                        return $@"{FilterField}!=TimeSpan({h},{m},{s})";
                        break;
                    case NumericOperator.GreaterThan:
                        return $@"{FilterField}>TimeSpan({h},{ m},{ s})";
                        break;
                    case NumericOperator.GreaterThanEqual:
                        return $@"{FilterField}>=TimeSpan({h},{m},{s})";
                        break;
                    case NumericOperator.LessThan:
                        return $@"{FilterField}<TimeSpan({h},{m},{s})";
                        break;
                    case NumericOperator.LessThanEqual:
                        return $@"{FilterField}<=TimeSpan({h},{m},{s})";
                        break;
                    default:
                        return "";
                        break;
                }
            }
        }

        public Type ModelType { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public event SearchPhraseChangedEventHandler SearchPhraseChanged;

        #endregion

        #region Meghods

        public TimeField()
        {
            InitializeComponent();

            OperatorVisible = true;
            ShowButtons = false;
        }

        private void searchfield_Loaded(object sender, RoutedEventArgs e)
        {
            defaultOperator = Operator;
            isSetDefaultOperator = true;
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
            Value = null;
            if (isSetDefaultOperator)
                Operator = defaultOperator;
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as TimeField).SearchPhraseChanged?.Invoke();
        }

        private string GetDisplayName()
        {
            BindingExpression bindEx = BindingOperations.GetBindingExpression(this, ValueProperty);
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
