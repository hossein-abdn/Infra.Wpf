using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

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

        private StringOperator defaultOperator;

        public string Title { get; set; }

        public string FilterField { get; set; }

        public string Text
        {
            get { return (string) GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(TextField), new FrameworkPropertyMetadata(null,FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

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

        #endregion

        #region Methods

        private void searchfield_Loaded(object sender, RoutedEventArgs e)
        {
            defaultOperator = Operator;
        }

        public TextField()
        {
            InitializeComponent();

            OpertatorVisible = true;
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

        #endregion
    }
}
