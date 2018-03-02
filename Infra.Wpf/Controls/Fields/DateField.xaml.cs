using Infra.Wpf.Common;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Infra.Wpf.Common.Helpers;

namespace Infra.Wpf.Controls
{
    public partial class DateField : UserControl, INotifyPropertyChanged, IField
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

        private bool _SuggestionVisible;
        public bool SuggestionVisible
        {
            get { return _SuggestionVisible; }
            set
            {
                _SuggestionVisible = value;
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

        public DateTime? SelectedDate
        {
            get { return (DateTime?) GetValue(SelectedDateProperty); }
            set { SetValue(SelectedDateProperty, value); }
        }

        public static readonly DependencyProperty SelectedDateProperty =
            DependencyProperty.Register("SelectedDate", typeof(DateTime?), typeof(DateField), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,SelectedDateChanged));

        private static void SelectedDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as DateField).SearchPhraseChanged?.Invoke();
        }

        private bool _IsDropDown;
        public bool IsDropDown
        {
            get { return _IsDropDown; }
            set
            {
                _IsDropDown = value;
                OnPropertyChanged();
            }
        }

        public DateFormat DateFormat { get; set; }

        public DateField Partner { get; set; }

        private NumericOperator defaultOperator;

        public string Title { get; set; }

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

        public event SearchPhraseChangedEventHandler SearchPhraseChanged;

        #endregion

        #region Methods

        private void searchfield_Loaded(object sender, RoutedEventArgs e)
        {
            defaultOperator = Operator;

            if (DateFormat == DateFormat.RangeFrom)
                OperatorVisible = false;

            if (FlowDirection == FlowDirection.LeftToRight)
                suggestbtn.HorizontalAlignment = HorizontalAlignment.Right;
            else
                suggestbtn.HorizontalAlignment = HorizontalAlignment.Left;

            DisplayName = GetDisplayName();

            if (IsFocused == true)
                this.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
        }

        public DateField()
        {
            InitializeComponent();
            OperatorVisible = true;
            SuggestionVisible = true;
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

        private void suggestbtn_Checked(object sender, RoutedEventArgs e)
        {
            suggestbtn.ContextMenu.IsOpen = true;
            suggestbtn.IsChecked = false;
        }

        private void ItemClick(object sender, RoutedEventArgs e)
        {
            var currentItem = (Button) sender;

            switch (currentItem.Name)
            {
                case "today":
                    pd.PersianSelectedDate = PersianDate.Today;
                    if (DateFormat != DateFormat.DateTime && Partner != null)
                        Partner.pd.PersianSelectedDate = PersianDate.Today;
                    break;
                case "thisweek":
                    var from = (int) PersianDate.Today.PersianDayOfWeek;
                    var to = from;

                    if (from != 6)
                        from++;
                    else
                        from = 0;

                    if (to != 6)
                        to = 5 - to;

                    if (DateFormat == DateFormat.RangeFrom || DateFormat == DateFormat.DateTime)
                    {
                        pd.PersianSelectedDate = PersianDate.Today.AddDays(from * (-1));
                        if (Partner != null)
                            Partner.pd.PersianSelectedDate = PersianDate.Today.AddDays(to);
                    }
                    else
                    {
                        pd.PersianSelectedDate = PersianDate.Today.AddDays(to);
                        if (Partner != null)
                            Partner.pd.PersianSelectedDate = PersianDate.Today.AddDays(from * (-1));
                    }
                    break;
                case "thismonth":
                    var lastday = 30;
                    if (PersianDate.Today.Month < 7)
                        lastday = 31;
                    else if (PersianDate.Today.Month == 12 && PersianDate.Today.IsLeapYear() == false)
                        lastday = 29;

                    if (DateFormat == DateFormat.RangeFrom || DateFormat == DateFormat.DateTime)
                    {
                        pd.PersianSelectedDate = new PersianDate(PersianDate.Today.Year, PersianDate.Today.Month, 1);
                        if (Partner != null)
                            Partner.pd.PersianSelectedDate = new PersianDate(PersianDate.Today.Year, PersianDate.Today.Month, lastday);
                    }
                    else
                    {
                        pd.PersianSelectedDate = new PersianDate(PersianDate.Today.Year, PersianDate.Today.Month, lastday);
                        if (Partner != null)
                            Partner.pd.PersianSelectedDate = new PersianDate(PersianDate.Today.Year, PersianDate.Today.Month, 1);
                    }
                    break;
                case "thisyear":
                    if (DateFormat == DateFormat.RangeFrom || DateFormat == DateFormat.DateTime)
                    {
                        pd.PersianSelectedDate = new PersianDate(PersianDate.Today.Year, 1, 1);
                        if (Partner != null)
                            Partner.pd.PersianSelectedDate = new PersianDate(PersianDate.Today.Year, 12, PersianDate.DaysInMonth(PersianDate.Today.Year, 12));
                    }
                    else
                    {
                        pd.PersianSelectedDate = new PersianDate(PersianDate.Today.Year, 12, PersianDate.DaysInMonth(PersianDate.Today.Year, 12));
                        if (Partner != null)
                            Partner.pd.PersianSelectedDate = new PersianDate(PersianDate.Today.Year, 1, 1);
                    }

                    break;
            }
            suggestbtn.ContextMenu.IsOpen = false;
        }

        private void SetSeasonClick(object sender, RoutedEventArgs e)
        {
            var currentItem = (MenuItem) sender;

            switch (currentItem.Name)
            {
                case "s1":
                    if (DateFormat == DateFormat.RangeFrom || DateFormat == DateFormat.DateTime)
                    {
                        pd.PersianSelectedDate = new PersianDate(PersianDate.Today.Year, 1, 1);
                        if (Partner != null)
                            Partner.pd.PersianSelectedDate = new PersianDate(PersianDate.Today.Year, 3, 31);
                    }
                    else
                    {
                        pd.PersianSelectedDate = new PersianDate(PersianDate.Today.Year, 3, 31);
                        if (Partner != null)
                            Partner.pd.PersianSelectedDate = new PersianDate(PersianDate.Today.Year, 1, 1);
                    }
                    break;
                case "s2":
                    if (DateFormat == DateFormat.RangeFrom || DateFormat == DateFormat.DateTime)
                    {
                        pd.PersianSelectedDate = new PersianDate(PersianDate.Today.Year, 4, 1);
                        if (Partner != null)
                            Partner.pd.PersianSelectedDate = new PersianDate(PersianDate.Today.Year, 6, 31);
                    }
                    else
                    {
                        pd.PersianSelectedDate = new PersianDate(PersianDate.Today.Year, 6, 31);
                        if (Partner != null)
                            Partner.pd.PersianSelectedDate = new PersianDate(PersianDate.Today.Year, 4, 1);
                    }
                    break;
                case "s3":
                    if (DateFormat == DateFormat.RangeFrom || DateFormat == DateFormat.DateTime)
                    {
                        pd.PersianSelectedDate = new PersianDate(PersianDate.Today.Year, 7, 1);
                        if (Partner != null)
                            Partner.pd.PersianSelectedDate = new PersianDate(PersianDate.Today.Year, 9, 30);
                    }
                    else
                    {
                        pd.PersianSelectedDate = new PersianDate(PersianDate.Today.Year, 9, 30);
                        if (Partner != null)
                            Partner.pd.PersianSelectedDate = new PersianDate(PersianDate.Today.Year, 7, 1);
                    }
                    break;
                case "s4":
                    if (DateFormat == DateFormat.RangeFrom || DateFormat == DateFormat.DateTime)
                    {
                        pd.PersianSelectedDate = new PersianDate(PersianDate.Today.Year, 10, 1);
                        if (Partner != null)
                            Partner.pd.PersianSelectedDate = new PersianDate(PersianDate.Today.Year, 12, PersianDate.DaysInMonth(PersianDate.Today.Year, 12));
                    }
                    else
                    {
                        new PersianDate(PersianDate.Today.Year, 12, PersianDate.DaysInMonth(PersianDate.Today.Year, 12));
                        if (Partner != null)
                            Partner.pd.PersianSelectedDate = new PersianDate(PersianDate.Today.Year, 10, 1);
                    }
                    break;
            }
        }

        private void SetMonthClick(object sender, RoutedEventArgs e)
        {
            int index = Int32.Parse(((MenuItem) sender).Name.Substring(1));

            if (DateFormat == DateFormat.RangeFrom || DateFormat == DateFormat.DateTime)
            {
                pd.PersianSelectedDate = new PersianDate(PersianDate.Today.Year, index, 1);
                if (Partner != null)
                    Partner.pd.PersianSelectedDate = new PersianDate(PersianDate.Today.Year, index, PersianDate.DaysInMonth(PersianDate.Today.Year, index));
            }
            else
            {
                pd.PersianSelectedDate = new PersianDate(PersianDate.Today.Year, index, PersianDate.DaysInMonth(PersianDate.Today.Year, index));
                if (Partner != null)
                    Partner.pd.PersianSelectedDate = new PersianDate(PersianDate.Today.Year, index, 1);
            }
        }

        private void SetHalfYearClick(object sender, RoutedEventArgs e)
        {
            var currentItem = (MenuItem) sender;

            switch (currentItem.Name)
            {
                case "hy1":
                    if (DateFormat == DateFormat.RangeFrom || DateFormat == DateFormat.DateTime)
                    {
                        pd.PersianSelectedDate = new PersianDate(PersianDate.Today.Year, 1, 1);
                        if (Partner != null)
                            Partner.pd.PersianSelectedDate = new PersianDate(PersianDate.Today.Year, 6, 31);
                    }
                    else
                    {
                        pd.PersianSelectedDate = new PersianDate(PersianDate.Today.Year, 6, 31);
                        if (Partner != null)
                            Partner.pd.PersianSelectedDate = new PersianDate(PersianDate.Today.Year, 1, 1);
                    }
                    break;

                case "hy2":
                    if (DateFormat == DateFormat.RangeFrom || DateFormat == DateFormat.DateTime)
                    {
                        pd.PersianSelectedDate = new PersianDate(PersianDate.Today.Year, 7, 1);
                        if (Partner != null)
                            Partner.pd.PersianSelectedDate = new PersianDate(PersianDate.Today.Year, 12, PersianDate.DaysInMonth(PersianDate.Today.Year, 12));
                    }
                    else
                    {
                        pd.PersianSelectedDate = new PersianDate(PersianDate.Today.Year, 12, PersianDate.DaysInMonth(PersianDate.Today.Year, 12));
                        if (Partner != null)
                            Partner.pd.PersianSelectedDate = new PersianDate(PersianDate.Today.Year, 7, 1);
                    }
                    break;
            }
        }

        private void ChangeDayClick(object sender, RoutedEventArgs e)
        {
            var currentItem = (Button) sender;
            var factor = 1;
            if (currentItem == decreaseday)
                factor = (-1);

            if (pd.PersianSelectedDate == null)
                pd.PersianSelectedDate = PersianDate.Today.AddDays(1 * factor);
            else
                pd.PersianSelectedDate = pd.PersianSelectedDate.AddDays(1 * factor);

            if (Partner != null)
                Partner.pd.PersianSelectedDate = pd.PersianSelectedDate;
        }

        private void ChangeWeekClick(object sender, RoutedEventArgs e)
        {
            var currentItem = (Button) sender;
            var factor = 1;
            if (currentItem == decreaseweek)
                factor = (-1);

            if (pd.PersianSelectedDate == null)
                pd.PersianSelectedDate = PersianDate.Today.AddDays(7 * factor);
            else
                pd.PersianSelectedDate = pd.PersianSelectedDate.AddDays(7 * factor);

            if (DateFormat == DateFormat.RangeFrom || DateFormat == DateFormat.DateTime)
            {
                if (Partner != null)
                    Partner.pd.PersianSelectedDate = pd.PersianSelectedDate.AddDays(6);
            }
            else
            {
                if (Partner != null)
                    Partner.pd.PersianSelectedDate = pd.PersianSelectedDate.AddDays(-6);
            }
        }

        private void ChangeMonthClick(object sender, RoutedEventArgs e)
        {
            var currentItem = (Button) sender;
            var factor = 1;
            if (currentItem == decreasemonth)
                factor = (-1);

            if (pd.PersianSelectedDate == null)
                pd.PersianSelectedDate = AddMonth(PersianDate.Today.Year, PersianDate.Today.Month, PersianDate.Today.Day, factor);
            else
                pd.PersianSelectedDate = AddMonth(pd.PersianSelectedDate.Year, pd.PersianSelectedDate.Month, pd.PersianSelectedDate.Day, factor);

            if (DateFormat == DateFormat.RangeFrom || DateFormat == DateFormat.DateTime)
            {
                if (Partner != null)
                    Partner.pd.PersianSelectedDate = AddMonth(pd.PersianSelectedDate.Year, pd.PersianSelectedDate.Month, pd.PersianSelectedDate.Day, 1);
            }
            else
            {
                if (Partner != null)
                    Partner.pd.PersianSelectedDate = AddMonth(pd.PersianSelectedDate.Year, pd.PersianSelectedDate.Month, pd.PersianSelectedDate.Day, -1);
            }
        }

        private PersianDate AddMonth(int year, int month, int day, int factor)
        {
            month += factor;
            if (!PersianDate.IsValid(year, month, day))
            {
                if (month > 12)
                {
                    year++;
                    month = 1;
                }
                else if (month < 1)
                {
                    year--;
                    month = 12;
                }

                if (PersianDate.DaysInMonth(year, month) < day)
                    day = PersianDate.DaysInMonth(year, month);
            }

            return new PersianDate(year, month, day);
        }

        private void ChangeYearClick(object sender, RoutedEventArgs e)
        {
            var currentItem = (Button) sender;
            var factor = 1;
            if (currentItem == decreaseyear)
                factor = (-1);

            if (pd.PersianSelectedDate == null)
                pd.PersianSelectedDate = AddYear(PersianDate.Today.Year, PersianDate.Today.Month, PersianDate.Today.Day, factor);
            else
                pd.PersianSelectedDate = AddYear(pd.PersianSelectedDate.Year, pd.PersianSelectedDate.Month, pd.PersianSelectedDate.Day, factor);

            if (DateFormat == DateFormat.RangeFrom || DateFormat == DateFormat.DateTime)
            {
                if (Partner != null)
                    Partner.pd.PersianSelectedDate = AddYear(pd.PersianSelectedDate.Year, pd.PersianSelectedDate.Month, pd.PersianSelectedDate.Day, 1);
            }
            else
            {
                if (Partner != null)
                    Partner.pd.PersianSelectedDate = AddYear(pd.PersianSelectedDate.Year, pd.PersianSelectedDate.Month, pd.PersianSelectedDate.Day, -1);
            }
        }

        private PersianDate AddYear(int year, int month, int day, int factor)
        {
            year += factor;
            if (!PersianDate.IsValid(year, month, day))
            {
                if (PersianDate.DaysInMonth(year, month) < day)
                    day = PersianDate.DaysInMonth(year, month);
            }

            return new PersianDate(year, month, day);
        }

        private string GetDisplayName()
        {
            BindingExpression bindEx = BindingOperations.GetBindingExpression(this, SelectedDateProperty);
            if (bindEx != null && !string.IsNullOrEmpty(bindEx.ResolvedSourcePropertyName))
            {
                var type = DataContext?.GetType().GetProperty("Model")?.PropertyType;
                if (type != null)
                {
                    var propInfo = type.GetProperty(bindEx.ResolvedSourcePropertyName);
                    var attrib = propInfo?.GetCustomAttributes(typeof(DisplayAttribute), false);
                    var isRequired = propInfo.IsRequired(bindEx.ResolvedSourcePropertyName);
                    if (attrib != null && attrib.Count() > 0)
                    {
                        var result = ((DisplayAttribute) attrib[0]).Name;
                        if (isRequired)
                            result = "* " + result;

                        return result;
                    }
                }
                else
                {
                    var displayText = bindEx.ResolvedSourcePropertyName;
                    if (!string.IsNullOrEmpty(displayText))
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
