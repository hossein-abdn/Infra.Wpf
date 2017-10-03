using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace Infra.Wpf.Controls
{
    public partial class TextSearchField : UserControl, INotifyPropertyChanged, ISearchField
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

        private string _FilterText;
        public string FilterText
        {
            get { return _FilterText; }
            set
            {
                _FilterText = value;
                OnPropertyChanged();
            }
        }

        private StringOperator defaultOperator;

        public string Title { get; set; }

        public string FilterField { get; set; }

        public string SearchPhrase
        {
            get
            {
                if (string.IsNullOrWhiteSpace(FilterText) || string.IsNullOrWhiteSpace(FilterField))
                    return "";

                switch (Operator)
                {
                    case StringOperator.Equals:
                        return $@"{FilterField}.Equals(""{FilterText.Trim()}"")";
                    case StringOperator.NotEquals:
                        return $@"!{FilterField}.Equals(""{FilterText.Trim()}"")";
                        break;
                    case StringOperator.Contains:
                        return $@"{FilterField}.Contains(""{FilterText.Trim()}"")";
                        break;
                    case StringOperator.StartsWith:
                        return $@"{FilterField}.StartsWith(""{FilterText.Trim()}"")";
                        break;
                    case StringOperator.EndsWith:
                        return $@"{FilterField}.EndsWith(""{FilterText.Trim()}"")";
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

        public static readonly DependencyProperty SearchFieldFormatProperty = CustomTextBox.TextBoxFormatProperty.AddOwner(typeof(TextSearchField),
            new FrameworkPropertyMetadata(CustomTextBoxFormat.String, FrameworkPropertyMetadataOptions.Inherits));

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Methods

        private void searchfield_Loaded(object sender, RoutedEventArgs e)
        {
            defaultOperator = Operator;
        }

        public TextSearchField()
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
            FilterText = string.Empty;
            Operator = defaultOperator;
        }

        #endregion
    }
}
