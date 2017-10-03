using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace Infra.Wpf.Controls
{
    public partial class NumericSearchField : UserControl, INotifyPropertyChanged, ISearchField
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

        private NumericOperator defaultOperator;

        public string Title { get; set; }

        public string FilterField { get; set; }

        public string SearchPhrase
        {
            get
            {
                if (string.IsNullOrWhiteSpace(FilterText) || string.IsNullOrWhiteSpace(FilterField))
                    return "";

                double field;
                if (double.TryParse(FilterText, out field) == false)
                    return "";

                switch (Operator)
                {
                    case NumericOperator.Equals:
                        return $@"{FilterField}=={FilterText.Trim()}";
                        break;
                    case NumericOperator.NotEquals:
                        return $@"{FilterField}!={FilterText.Trim()}";
                        break;
                    case NumericOperator.GreaterThan:
                        return $@"{FilterField}>{FilterText.Trim()}";
                        break;
                    case NumericOperator.GreaterThanEqual:
                        return $@"{FilterField}>={FilterText.Trim()}";
                        break;
                    case NumericOperator.LessThan:
                        return $@"{FilterField}<{FilterText.Trim()}";
                        break;
                    case NumericOperator.LessThanEqual:
                        return $@"{FilterField}<={FilterText.Trim()}";
                        break;
                    default:
                        return "";
                        break;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Methods

        private void searchfield_Loaded(object sender, RoutedEventArgs e)
        {
            defaultOperator = Operator;
        }

        public NumericSearchField()
        {
            InitializeComponent();

            OpertatorVisible = true;
            ShowButtons = false;
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
