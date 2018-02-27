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

        private bool _OpertatorVisible;
        public bool OpertatorVisible
        {
            get { return _OpertatorVisible; }
            set
            {
                _OpertatorVisible = value;
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
            DisplayName = GetDisplayName();

            if (IsFocused == true)
                this.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
        }

        public TextField()
        {
            InitializeComponent();

            OpertatorVisible = true;
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
            Operator = defaultOperator;
        }

        private string GetDisplayName()
        {
            BindingExpression bindEx = BindingOperations.GetBindingExpression(this, TextProperty);
            if (bindEx != null && !string.IsNullOrEmpty(bindEx.ResolvedSourcePropertyName))
            {
                var type = DataContext?.GetType().GetProperty("Model")?.PropertyType;
                if (type != null)
                {
                    var propInfo = type?.GetProperty(bindEx.ResolvedSourcePropertyName);
                    var attrib = propInfo?.GetCustomAttributes(typeof(DisplayAttribute), false);
                    if (attrib != null && attrib.Count() > 0)
                        return ((DisplayAttribute) attrib[0]).Name;
                }
                else
                {
                    var displayText = bindEx.ResolvedSourcePropertyName;
                    if (string.IsNullOrEmpty(displayText))
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
