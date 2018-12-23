using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Infra.Wpf.Common.Helpers;
using System;

namespace Infra.Wpf.Controls
{
    public partial class TextField : UserControl, INotifyPropertyChanged, IField
    {
        #region Properties

        private StringOperator _Operator;
        public StringOperator Operator
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

        private StringOperator defaultOperator;

        private bool isSetDefaultOperator;

        public string Title { get; set; }

        public string FilterField { get; set; }

        public string Text
        {
            get { return (string) GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(TextField), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnTextPropertyChanged));

        public string SearchPhrase
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Text) || string.IsNullOrWhiteSpace(FilterField))
                    return "";

                switch (Operator)
                {
                    case StringOperator.Equals:
                        return $@"{FilterField}.Equals(""{Text.Trim()}"")";
                    case StringOperator.NotEquals:
                        return $@"!{FilterField}.Equals(""{Text.Trim()}"")";
                        break;
                    case StringOperator.Contains:
                        return $@"{FilterField}.Contains(""{Text.Trim()}"")";
                        break;
                    case StringOperator.StartsWith:
                        return $@"{FilterField}.StartsWith(""{Text.Trim()}"")";
                        break;
                    case StringOperator.EndsWith:
                        return $@"{FilterField}.EndsWith(""{Text.Trim()}"")";
                        break;
                    default:
                        return "";
                        break;
                }
            }
        }

        public Type ModelType { get; set; }

        public CustomTextBoxFormat SearchFieldFormat
        {
            get { return (CustomTextBoxFormat) GetValue(SearchFieldFormatProperty); }
            set { SetValue(SearchFieldFormatProperty, value); }
        }

        public static readonly DependencyProperty SearchFieldFormatProperty = CustomTextBox.TextBoxFormatProperty.AddOwner(typeof(TextField),
            new FrameworkPropertyMetadata(CustomTextBoxFormat.String, FrameworkPropertyMetadataOptions.Inherits));

        public event PropertyChangedEventHandler PropertyChanged;

        public event SearchPhraseChangedEventHandler SearchPhraseChanged;

        #endregion

        #region Methods

        private void searchfield_Loaded(object sender, RoutedEventArgs e)
        {
            defaultOperator = Operator;
            isSetDefaultOperator = true;
            DisplayName = GetDisplayName();

            if (IsFocused == true)
                this.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
        }

        public override void OnApplyTemplate()
        {
            SetValidationStyle();
            base.OnApplyTemplate();
        }

        public TextField()
        {
            InitializeComponent();

            OperatorVisible = true;
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

        private static void OnTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as TextField).SearchPhraseChanged?.Invoke();
        }

        public void OnPropertyChanged([CallerMemberName]string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public void Clear()
        {
            Text = string.Empty;
            if (isSetDefaultOperator)
                Operator = defaultOperator;
        }

        private string GetDisplayName()
        {
            BindingExpression bindEx = BindingOperations.GetBindingExpression(this, TextProperty);
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
