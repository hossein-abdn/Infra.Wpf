using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.ComponentModel;
using System.Windows.Data;
using Infra.Wpf.Converters;
using Infra.Wpf.Common;

namespace Infra.Wpf.Controls
{
    [DefaultEvent("SelectedDateChanged")]
    [DefaultProperty("DisplayDate")]
    public partial class PersianCalendar : UserControl
    {
        #region Properties

        public static readonly DependencyProperty DisplayDateProperty =
            DependencyProperty.Register("DisplayDate", typeof(PersianDate), typeof(PersianCalendar),
                new PropertyMetadata(PersianDate.Today, ModeChanged, CoerceDateToBeInRange));

        [Category("Calendar")]
        public PersianDate DisplayDate
        {
            get
            {
                return (PersianDate) this.GetValue(DisplayDateProperty);
            }
            set
            {
                this.SetValue(DisplayDateProperty, value);
            }
        }

        public static readonly DependencyProperty DisplayModeProperty =
            DependencyProperty.Register("DisplayMode", typeof(CalendarType), typeof(PersianCalendar),
                new PropertyMetadata(ModeChanged));

        [Category("Calendar")]
        public CalendarType DisplayMode
        {
            get
            {
                return (CalendarType) GetValue(DisplayModeProperty);
            }
            set
            {
                SetValue(DisplayModeProperty, value);
            }
        }

        public static readonly DependencyProperty DisplayDateStartProperty =
            DependencyProperty.Register("DisplayDateStart", typeof(PersianDate), typeof(PersianCalendar),
                new PropertyMetadata(PersianDate.MinValue, DisplayDateStartChanged));

        [Category("Calendar")]
        public PersianDate DisplayDateStart
        {
            get
            {
                return (PersianDate) GetValue(DisplayDateStartProperty);
            }
            set
            {
                SetValue(DisplayDateStartProperty, value);
            }
        }

        public static readonly DependencyProperty DisplayDateEndProperty =
            DependencyProperty.Register("DisplayDateEnd", typeof(PersianDate), typeof(PersianCalendar),
                new PropertyMetadata(PersianDate.MaxValue, DisplayDateEndChanged, CoerceDisplayDateEnd));

        [Category("Calendar")]
        public PersianDate DisplayDateEnd
        {
            get
            {
                return (PersianDate) GetValue(DisplayDateEndProperty);
            }
            set
            {
                SetValue(DisplayDateEndProperty, value);
            }
        }

        public static readonly DependencyProperty PersianSelectedDateProperty =
            DependencyProperty.Register("PersianSelectedDate", typeof(PersianDate), typeof(PersianCalendar),
                new FrameworkPropertyMetadata(PersianDate.Today, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedDateChanged, CoerceDateToBeInRange));

        [Category("Calendar")]
        public PersianDate PersianSelectedDate
        {
            get
            {
                return (PersianDate) GetValue(PersianSelectedDateProperty);
            }
            set
            {
                SetValue(PersianSelectedDateProperty, value);
            }
        }

        public static readonly DependencyProperty SelectedDateProperty =
            DependencyProperty.Register("SelectedDate", typeof(DateTime?), typeof(PersianCalendar),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public DateTime? SelectedDate
        {
            get { return (DateTime?) GetValue(SelectedDateProperty); }
            set { SetValue(SelectedDateProperty, value); }
        }

        public static readonly DependencyProperty SelectedDateBackgroundProperty =
            DependencyProperty.Register("SelectedDateBackground", typeof(Brush), typeof(PersianCalendar),
                new PropertyMetadata(Brushes.Gold, SelectedDateBackgroundChanged));

        [Category("Calendar")]
        public Brush SelectedDateBackground
        {
            get
            {
                return (Brush) GetValue(SelectedDateBackgroundProperty);
            }
            set
            {
                SetValue(SelectedDateBackgroundProperty, value);
            }
        }

        public static readonly DependencyProperty TodayBackgroundProperty =
            DependencyProperty.Register("TodayBackground", typeof(Brush), typeof(PersianCalendar),
                new PropertyMetadata(Brushes.AliceBlue, TodayBackgroundChanged));

        [Category("Calendar")]
        public Brush TodayBackground
        {
            get
            {
                return (Brush) GetValue(TodayBackgroundProperty);
            }
            set
            {
                SetValue(TodayBackgroundProperty, value);
            }
        }

        public static readonly RoutedEvent SelectedDateChangedEvent =
            EventManager.RegisterRoutedEvent("SelectedDateChanged",
                RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(PersianCalendar));

        [Category("Calendar")]
        public event RoutedEventHandler SelectedDateChanged
        {
            add
            {
                AddHandler(SelectedDateChangedEvent, value);
            }
            remove
            {
                RemoveHandler(SelectedDateChangedEvent, value);
            }
        }

        internal Button[,] monthModeButtons = new Button[6, 7];

        Button[] yearModeButtons = new Button[12];

        Button[] DecadeModeButtons = new Button[12];

        #endregion

        #region Methods

        public PersianCalendar()
        {
            InitializeComponent();

            InitializeMonth();
            InitializeYear();
            InitializeDecade();

            this.SetCalendarType();
        }

        static void DisplayDateStartChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PersianCalendar pc = d as PersianCalendar;
            pc.CoerceValue(DisplayDateEndProperty);
            pc.CoerceValue(PersianSelectedDateProperty);
            pc.CoerceValue(DisplayDateProperty);
            ModeChanged(d, new DependencyPropertyChangedEventArgs());
        }

        static void DisplayDateEndChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PersianCalendar pc = d as PersianCalendar;
            pc.CoerceValue(PersianSelectedDateProperty);
            pc.CoerceValue(DisplayDateProperty);
            ModeChanged(d, new DependencyPropertyChangedEventArgs());
        }

        static void ModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PersianCalendar pc = d as PersianCalendar;
            pc.SetCalendarType();
        }

        private static void OnSelectedDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PersianCalendar pc = d as PersianCalendar;
            pc.SelectedDateCheck((PersianDate) e.OldValue);

            if (e.NewValue != null)
                pc.SetValue(SelectedDateProperty, (e.NewValue as PersianDate).ToDateTime());
            else
                pc.SetValue(SelectedDateProperty, null);

            pc.RaiseEvent(new RoutedEventArgs(SelectedDateChangedEvent, pc));
        }

        private static void TodayBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PersianCalendar pc = d as PersianCalendar;
            pc.TodayCheck();
        }

        private static void SelectedDateBackgroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PersianCalendar pc = d as PersianCalendar;
            pc.SelectedDateCheck(null);
        }

        static object CoerceDisplayDateEnd(DependencyObject d, object o)
        {
            PersianCalendar pc = d as PersianCalendar;
            PersianDate value = (PersianDate) o;
            if (value < pc.DisplayDateStart)
                return pc.DisplayDateStart;

            return o;
        }

        static object CoerceDateToBeInRange(DependencyObject d, object o)
        {
            PersianCalendar pc = d as PersianCalendar;
            PersianDate value = (PersianDate) o;
            if (value < pc.DisplayDateStart)
            {
                return pc.DisplayDateStart;
            }
            if (value > pc.DisplayDateEnd)
            {
                return pc.DisplayDateEnd;
            }
            return o;
        }

        void InitializeMonth()
        {
            int tabIndex = 10;
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    var element = new Button();
                    element.TabIndex = tabIndex++;
                    element.Click += new RoutedEventHandler(monthModeButton_Click);
                    this.monthUniformGrid.Children.Add(element);
                    this.monthModeButtons[i, j] = element;
                }
            }
        }

        void InitializeYear()
        {
            Style insideButtonStyle = (Style) this.FindResource("InsideButtonsStyle");

            int tabIndex = 10;

            for (int i = 0; i < 12; i++)
            {
                var element = new Button { Style = insideButtonStyle };
                element.Content = ((PersianMonth) i + 1).ToString();
                element.TabIndex = tabIndex++;
                element.Click += new RoutedEventHandler(yearModeButton_Click);
                element.Tag = i + 1;
                this.yearModeButtons[i] = element;
                this.yearUniformGrid.Children.Add(element);
            }
        }

        void InitializeDecade()
        {
            Style insideButtonStyle = (Style) this.FindResource("InsideButtonsStyle");

            int tabIndex = 10;

            this.decadeUniformGrid.Children.Add(new UIElement { IsEnabled = false });
            for (int i = 1; i <= 10; i++)
            {
                var element = new Button { Style = insideButtonStyle };
                element.TabIndex = tabIndex++;
                element.Click += new RoutedEventHandler(decadeModeButton_Click);
                element.Tag = i - 1;
                this.DecadeModeButtons[i] = element;
                this.decadeUniformGrid.Children.Add(element);
            }
        }

        void monthModeButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button) sender;
            var buttonDate = (PersianDate) button.Tag;
            var displayDate = this.DisplayDate;

            if (displayDate.Year != buttonDate.Year || displayDate.Month != buttonDate.Month)
                this.SetCurrentValue(DisplayDateProperty, new PersianDate(buttonDate.Year, buttonDate.Month, 1));

            this.PersianSelectedDate = buttonDate;
        }

        void yearModeButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button) sender;
            int month = (int) button.Tag;
            this.SetCurrentValue(DisplayDateProperty, new PersianDate(this.DisplayDate.Year, month, 1));
            this.DisplayMode = CalendarType.Month;
        }

        void decadeModeButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button) sender;
            this.SetCurrentValue(DisplayDateProperty, new PersianDate((int) button.Tag, 1, 1));
            this.DisplayMode = CalendarType.Year;
        }

        private void SelectedDateCheck(PersianDate oldValue)
        {
            int r, c;
            MonthModeDateToRowColumn(this.PersianSelectedDate, out r, out c);
            SetMonthModeButtonAppearance(this.monthModeButtons[r, c]);

            if (oldValue != null)
            {
                MonthModeDateToRowColumn(oldValue, out r, out c);
                SetMonthModeButtonAppearance(this.monthModeButtons[r, c]);
            }
        }

        private void TodayCheck()
        {
            if (this.DisplayMode == CalendarType.Month)
            {
                int r, c;
                MonthModeDateToRowColumn(PersianDate.Today, out r, out c);
                SetMonthModeButtonAppearance(this.monthModeButtons[r, c]);
            }
        }

        void SetMonthModeButtonAppearance(Button button)
        {
            Brush bg = Brushes.Transparent;
            if (button.Tag != null)
            {
                var bdate = (PersianDate) button.Tag;
                if (bdate == PersianDate.Today)
                    bg = this.TodayBackground;

                if (bdate == this.PersianSelectedDate)
                    bg = this.SelectedDateBackground;
            }
            button.Background = bg;
        }

        private static void MonthModeDateToRowColumn(PersianDate date, out int row, out int column)
        {
            PersianDate firstDay = new PersianDate(date.Year, date.Month, 1);

            int firstCol = (int) firstDay.PersianDayOfWeek + 1;
            row = ((date.Day + firstCol - 1) / 7);
            column = (date.Day + firstCol - 1) % 7;
        }

        private static PersianDate MonthModeRowColumnToDate(int row, int column, PersianDate firstDayInMonth)
        {
            int firstCol = (int) firstDayInMonth.PersianDayOfWeek + 1;
            int dayDifference = (row * 7) + column - firstCol;

            if (firstDayInMonth == PersianDate.MinValue && dayDifference < 0)
                return null;

            if (firstDayInMonth.Year == PersianDate.MaxValue.Year &&
                firstDayInMonth.Month == PersianDate.MaxValue.Month &&
                dayDifference > 9)
            {
                return null;
            }

            return firstDayInMonth.AddDays(dayDifference);
        }

        private void SetCalendarType()
        {
            switch (this.DisplayMode)
            {
                case CalendarType.Month:
                    SetMonthMode();
                    break;
                case CalendarType.Year:
                    SetYearMode();
                    break;
                case CalendarType.Decade:
                    SetDecadeMode();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("DisplayMode", "The DisplayMode value is not valid");
            }
        }

        private void SetDecadeMode()
        {
            this.monthUniformGrid.Visibility = this.yearUniformGrid.Visibility = Visibility.Collapsed;
            this.decadeUniformGrid.Visibility = Visibility.Visible;

            int startDecade = DisplayDate.Year - DisplayDate.Year % 10;
            int endDecade = startDecade + 9;

            for (int i = 0; i < 10; i++)
            {
                int y = i + startDecade;
                if (y >= DisplayDateStart.Year && y <= DisplayDateEnd.Year)
                {
                    DecadeModeButtons[i + 1].Content = startDecade + i;
                    DecadeModeButtons[i + 1].Tag = startDecade + i;
                    DecadeModeButtons[i + 1].IsEnabled = true;
                }
                else
                {
                    DecadeModeButtons[i + 1].Content = "";
                    DecadeModeButtons[i + 1].Tag = null;
                    DecadeModeButtons[i + 1].IsEnabled = false;
                }
            }

            if (startDecade < DisplayDateStart.Year)
                startDecade = DisplayDateStart.Year;

            if (endDecade > DisplayDateEnd.Year)
                endDecade = DisplayDateEnd.Year;

            this.titleButton.Content = string.Format("{0} - {1}", startDecade, endDecade);
        }

        private void SetMonthMode()
        {
            this.decadeUniformGrid.Visibility = this.yearUniformGrid.Visibility = Visibility.Collapsed;
            this.monthUniformGrid.Visibility = Visibility.Visible;

            int year = DisplayDate.Year;
            int month = DisplayDate.Month;
            PersianDate firstDayInMonth = new PersianDate(year, month, 1);

            Style insideButtonStyle = (Style) this.FindResource("InsideButtonsStyle");
            Style GrayButtonStyle = (Style) this.FindResource("GrayButtonsStyle");

            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 7; j++)
                {
                    PersianDate date = null;
                    bool dateInRange;

                    try
                    {
                        date = MonthModeRowColumnToDate(i, j, firstDayInMonth);
                        dateInRange = date >= DisplayDateStart && date <= DisplayDateEnd;
                    }
                    catch (OverflowException)
                    {
                        dateInRange = false;
                    }

                    var button = monthModeButtons[i, j];

                    if (dateInRange && date.Month == firstDayInMonth.Month)
                    {
                        button.Style = insideButtonStyle;
                        button.Content = date.Day.ToString();
                        button.IsEnabled = true;
                        button.Tag = date;
                    }
                    else if (dateInRange)
                    {
                        button.Style = GrayButtonStyle;
                        button.Content = date.Day.ToString();
                        button.IsEnabled = true;
                        button.Tag = date;
                    }
                    else
                    {
                        button.Tag = null;
                        button.Content = "";
                        button.IsEnabled = false;
                        button.Background = Brushes.Transparent;
                    }
                }
            }

            this.titleButton.Content = ((PersianMonth) month).ToString() + " " + year.ToString();

            this.TodayCheck();
            this.SelectedDateCheck(null);
        }

        private void SetYearMode()
        {
            this.monthUniformGrid.Visibility = this.decadeUniformGrid.Visibility = Visibility.Collapsed;
            this.yearUniformGrid.Visibility = Visibility.Visible;

            this.titleButton.Content = this.DisplayDate.Year.ToString();

            for (int i = 0; i < 12; i++)
            {
                int month = i + 1;

                if (PersianDate.IsValid(DisplayDate.Year, month, 1) &&
                    new PersianDate(DisplayDate.Year, month, PersianDate.DaysInMonth(DisplayDate.Year, month)) >= DisplayDateStart &&
                    new PersianDate(DisplayDate.Year, month, 1) <= DisplayDateEnd)
                {
                    yearModeButtons[i].Content = ((PersianMonth) month).ToString();
                    yearModeButtons[i].IsEnabled = true;
                }
                else
                {
                    yearModeButtons[i].Content = "";
                    yearModeButtons[i].IsEnabled = false;
                }
            }
        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            int m = this.DisplayDate.Month;
            int y = this.DisplayDate.Year;
            try
            {
                PersianDate newDisplayDate = DisplayDate;
                if (this.DisplayMode == CalendarType.Month)
                {
                    if (m == 12)
                    {
                        if (PersianDate.IsValid(y + 1, 1, 1))
                            newDisplayDate = new PersianDate(y + 1, 1, 1);
                    }
                    else
                    {
                        if (PersianDate.IsValid(y, m + 1, 1))
                            newDisplayDate = new PersianDate(y, m + 1, 1);
                    }
                }
                else if (this.DisplayMode == CalendarType.Year)
                {
                    if (PersianDate.IsValid(y + 1, 1, 1))
                        newDisplayDate = new PersianDate(y + 1, 1, 1);
                }
                else if (this.DisplayMode == CalendarType.Decade)
                {
                    if (PersianDate.IsValid(y - y % 10 + 10, 1, 1))
                        newDisplayDate = new PersianDate(y - y % 10 + 10, 1, 1);
                }

                if (newDisplayDate >= DisplayDateStart && newDisplayDate <= DisplayDateEnd)
                    this.SetCurrentValue(DisplayDateProperty, newDisplayDate);
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }

        private void previousButton_Click(object sender, RoutedEventArgs e)
        {
            int m = this.DisplayDate.Month;
            int y = this.DisplayDate.Year;
            try
            {
                PersianDate newDisplayDate = DisplayDate;

                if (this.DisplayMode == CalendarType.Month)
                {
                    if (m == 1)
                    {
                        if (y != 1)
                            newDisplayDate = new PersianDate(y - 1, 12, PersianDate.DaysInMonth(y - 1, 12));
                    }
                    else
                        newDisplayDate = new PersianDate(y, m - 1, PersianDate.DaysInMonth(y, m - 1));
                }
                else if (this.DisplayMode == CalendarType.Year)
                {
                    if (y != 1)
                        newDisplayDate = new PersianDate(y - 1, 12, PersianDate.DaysInMonth(y - 1, 12));
                }
                else if (this.DisplayMode == CalendarType.Decade)
                {
                    if ((y - y % 10 - 1) >= 1)
                        newDisplayDate = new PersianDate(y - y % 10 - 1, 12, PersianDate.DaysInMonth(y - y % 10 - 1, 12));
                }

                if (newDisplayDate >= DisplayDateStart && newDisplayDate <= DisplayDateEnd)
                    this.SetCurrentValue(DisplayDateProperty, newDisplayDate);
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }

        private void titleButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.DisplayMode == CalendarType.Month)
                this.DisplayMode = CalendarType.Year;
            else if (this.DisplayMode == CalendarType.Year)
                this.DisplayMode = CalendarType.Decade;
        }

        private void btnToDay_Click(object sender, RoutedEventArgs e)
        {
            this.PersianSelectedDate = PersianDate.Today;
            this.DisplayDate = PersianDate.Today;
        }

        #endregion
    }
}