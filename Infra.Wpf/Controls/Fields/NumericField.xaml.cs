using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Infra.Wpf.Common.Helpers;
using System.Windows.Media;
using System;

namespace Infra.Wpf.Controls
{
    public partial class NumericField : UserControl, INotifyPropertyChanged, IField
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

        public long? Value
        {
            get { return (long?) GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(long?), typeof(NumericField), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChanged));

        public long MaxValue
        {
            get
            {
                return (long) GetValue(MaxValueProperty);
            }
            set
            {
                SetValue(MaxValueProperty, value);
            }
        }

        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(long), typeof(NumericField), new PropertyMetadata(long.MaxValue));

        public long MinValue
        {
            get
            {
                return (long) GetValue(MinValueProperty);
            }
            set
            {
                SetValue(MinValueProperty, value);
            }
        }

        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register("MinValue", typeof(long), typeof(NumericField), new PropertyMetadata(long.MinValue));

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

                double field;
                if (double.TryParse(filterText, out field) == false)
                    return "";

                filterText = filterText.Trim();
                switch (Operator)
                {
                    case NumericOperator.Equals:
                        return $@"{FilterField}=={filterText}";
                    case NumericOperator.NotEquals:
                        return $@"{FilterField}!={filterText}";
                    case NumericOperator.GreaterThan:
                        return $@"{FilterField}>{filterText}";
                    case NumericOperator.GreaterThanEqual:
                        return $@"{FilterField}>={filterText}";
                    case NumericOperator.LessThan:
                        return $@"{FilterField}<{filterText}";
                    case NumericOperator.LessThanEqual:
                        return $@"{FilterField}<={filterText}";
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
            DependencyProperty.Register("IsGetFocus", typeof(bool), typeof(NumericField), new PropertyMetadata(false, OnIsGetFocusChanged));

        public event PropertyChangedEventHandler PropertyChanged;

        public event SearchPhraseChangedEventHandler SearchPhraseChanged;

        #endregion

        #region Methods

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

            BindingExpression bindingExpression = this.GetBindingExpression(ValueProperty);

            Binding valueBinding = new Binding("Value")
            {
                Source = this,
                Mode = BindingMode.TwoWay,
                UpdateSourceTrigger = bindingExpression?.ParentBinding?.UpdateSourceTrigger ?? UpdateSourceTrigger.Default
            };
            numericbox.SetBinding(NumericBox.ValueProperty, valueBinding);
        }

        public override void OnApplyTemplate()
        {
            SetValidationStyle();
            base.OnApplyTemplate();
        }

        public NumericField()
        {
            InitializeComponent();

            OperatorVisible = true;
            ShowButtons = false;
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
                ((NumericField)d).MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
        }

        public void Clear()
        {
            Value = null;
            if (isSetDefaultOperator)
                Operator = defaultOperator;
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as NumericField).SearchPhraseChanged?.Invoke();
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
