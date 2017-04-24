using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace Infra.Wpf.Controls
{
    public partial class DateSearchField : UserControl, INotifyPropertyChanged, ISearchField
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

        private DateTime? _SelectedDate;
        public DateTime? SelectedDate
        {
            get { return _SelectedDate; }
            set
            {
                _SelectedDate = value;
                OnPropertyChanged();
            }
        }

        public DateFormat DateFormat { get; set; }

        private NumericOperator defaultOperator;

        public string DisplayName { get; set; }

        public string FilterField { get; set; }

        public string SearchPhrase
        {
            get
            {
                if (SelectedDate == null || string.IsNullOrWhiteSpace(FilterField))
                    return "";

                int y = SelectedDate.Value.Year;
                int m = SelectedDate.Value.Month;
                int d = SelectedDate.Value.Day;

                switch (Operator)
                {
                    case NumericOperator.Equals:
                        return $@"{FilterField}>=DateTime({y},{m},{d}) AND {FilterField}<=DateTime({y},{m},{d},23,59,59)";
                        break;
                    case NumericOperator.NotEquals:
                        return $@"{FilterField}<DateTime({y},{m},{d}) OR {FilterField}>DateTime({y},{m},{d},23,59,59)";
                        break;
                    case NumericOperator.GreaterThan:
                        return $@"{FilterField}>DateTime({y},{m},{d},23,59,59)";
                        break;
                    case NumericOperator.GreaterThanEqual:
                        return $@"{FilterField}>=DateTime({y},{m},{d})";
                        break;
                    case NumericOperator.LessThan:
                        return $@"{FilterField}<DateTime({y},{m},{d})";
                        break;
                    case NumericOperator.LessThanEqual:
                        return $@"{FilterField}<=DateTime({y},{m},{d},23,59,59)";
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

            if (DateFormat == DateFormat.Range)
                OpertatorVisible = false;
        }

        public DateSearchField()
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
            pd.PersianSelectedDate = null;
            Operator = defaultOperator;
        }

        #endregion
    }
}
