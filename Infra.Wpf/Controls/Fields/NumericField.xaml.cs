using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

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



        public long? Value
        {
            get { return (long?) GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(long?), typeof(NumericField), new FrameworkPropertyMetadata(null,FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        private NumericOperator defaultOperator;

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
                        break;
                    case NumericOperator.NotEquals:
                        return $@"{FilterField}!={filterText}";
                        break;
                    case NumericOperator.GreaterThan:
                        return $@"{FilterField}>{filterText}";
                        break;
                    case NumericOperator.GreaterThanEqual:
                        return $@"{FilterField}>={filterText}";
                        break;
                    case NumericOperator.LessThan:
                        return $@"{FilterField}<{filterText}";
                        break;
                    case NumericOperator.LessThanEqual:
                        return $@"{FilterField}<={filterText}";
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

        public NumericField()
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
            Value = null;
            Operator = defaultOperator;
        }

        #endregion
    }
}
