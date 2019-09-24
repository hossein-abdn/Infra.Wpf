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
                    case NumericOperator.NotEquals:
                        return $@"{FilterField}!=TimeSpan({h},{m},{s})";
                    case NumericOperator.GreaterThan:
                        return $@"{FilterField}>TimeSpan({h},{ m},{ s})";
                    case NumericOperator.GreaterThanEqual:
                        return $@"{FilterField}>=TimeSpan({h},{m},{s})";
                    case NumericOperator.LessThan:
                        return $@"{FilterField}<TimeSpan({h},{m},{s})";
                    case NumericOperator.LessThanEqual:
                        return $@"{FilterField}<=TimeSpan({h},{m},{s})";
                    default:
                        return "";
                }
            }
        }

        public Type ModelType { get; set; }

        public bool IsGetFocus
        {
            get { return (bool)GetValue(IsGetFocusProperty); }
            set { SetValue(IsGetFocusProperty, value); }
        }

        public static readonly DependencyProperty IsGetFocusProperty =
            DependencyProperty.Register("IsGetFocus", typeof(bool), typeof(TimeField), new PropertyMetadata(false, OnIsGetFocusChanged));

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

            Binding binding = new Binding("IsFocused")
            {
                Source = this,
                Mode = BindingMode.OneWay
            };

            SetBinding(IsGetFocusProperty, binding);
        }

        public override void OnApplyTemplate()
        {
            SetValidationStyle();
            base.OnApplyTemplate();
        }

        private void SetValidationStyle()
        {
            var style = new Style();

            if (Style != null)
            {
                style.BasedOn = Style.BasedOn;
                style.Resources = Style.Resources;
                style.TargetType = Style.TargetType;

                foreach (var item in Style.Setters)
                    style.Setters.Add(item);

                foreach (var item in Style.Triggers)
                    style.Triggers.Add(item);
            }

            var trigger = new Trigger()
            {
                Property = Validation.HasErrorProperty,
                Value = true
            };

            var bind = new Binding("(Validation.Errors)[0].ErrorContent")
            {
                RelativeSource = new RelativeSource(RelativeSourceMode.Self)
            };

            trigger.Setters.Add(new Setter(ToolTipProperty, bind));
            style.Triggers.Add(trigger);
            Style = style;

            Validation.SetErrorTemplate(this, new ControlTemplate());

            var borderBind = new Binding("(Validation.HasError)")
            {
                Source = this,
                Converter = new Converters.VisibilityToBoolConverter(),
                Mode = BindingMode.OneWay
            };

            BindingOperations.SetBinding(validationBorder, Border.VisibilityProperty, borderBind);
        }

        public void OnPropertyChanged([CallerMemberName]string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private static void OnIsGetFocusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (((bool)e.NewValue) == true)
                ((TimeField)d).MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
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
