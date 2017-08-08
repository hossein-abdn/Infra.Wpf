using C1.WPF;
using Infra.Wpf.Common;
using Infra.Wpf.Common.Helpers;
using Infra.Wpf.Converters;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Infra.Wpf.Controls
{
    [DefaultEvent("SelectedDateChanged")]
    [DefaultProperty("PersianSelectedDate")]
    public partial class PersianDatePicker : UserControl
    {
        #region Properties

        public static readonly DependencyProperty PersianSelectedDateProperty =
            DependencyProperty.Register("PersianSelectedDate", typeof(PersianDate), typeof(PersianDatePicker),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnPersianSelectedDateChanged, CoercePersianSelectedDate));

        [Category("Date Picker")]
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
            DependencyProperty.Register("SelectedDate", typeof(DateTime?), typeof(PersianDatePicker),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,OnSelectedDateChanged));

        public DateTime? SelectedDate
        {
            get { return (DateTime?) GetValue(SelectedDateProperty); }
            set { SetValue(SelectedDateProperty, value); }
        }

        public static readonly DependencyProperty DisplayDateProperty =
            DependencyProperty.Register("DisplayDate", typeof(PersianDate), typeof(PersianDatePicker),
                new PropertyMetadata(PersianDate.Today, null, CoerceDisplayDate));

        [Category("Date Picker")]
        public PersianDate DisplayDate
        {
            get
            {
                return (PersianDate) GetValue(DisplayDateProperty);
            }
            set
            {
                SetValue(DisplayDateProperty, value);
            }
        }

        public static readonly DependencyProperty DisplayDateStartProperty =
            DependencyProperty.Register("DisplayDateStart", typeof(PersianDate),
                typeof(PersianDatePicker), new PropertyMetadata(PersianDate.MinValue));

        [Category("Date Picker")]
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
            DependencyProperty.Register("DisplayDateEnd", typeof(PersianDate), typeof(PersianDatePicker),
                new PropertyMetadata(PersianDate.MaxValue, null, CoerceDisplayDateEnd));

        [Category("Date Picker")]
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

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(PersianDatePicker));

        public string Text
        {
            get
            {
                return (string) GetValue(TextProperty);
            }
            set
            {
                SetValue(TextProperty, value);
            }
        }

        public static readonly DependencyProperty AllowNullProperty =
            DependencyProperty.Register("AllowNull", typeof(bool), typeof(PersianDatePicker), new PropertyMetadata(true));

        public bool AllowNull
        {
            get
            {
                return (bool) GetValue(AllowNullProperty);
            }
            set
            {
                SetValue(AllowNullProperty, value);
            }
        }

        public PersianCalendar PersianCalendar
        {
            get
            {
                return persianCalendar;
            }
        }

        public static readonly RoutedEvent SelectedDateChangedEvent =
            EventManager.RegisterRoutedEvent("SelectedDateChanged", RoutingStrategy.Bubble,
                typeof(RoutedEventHandler), typeof(PersianDatePicker));

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

        #endregion

        #region Methods

        public PersianDatePicker()
        {
            InitializeComponent();

            foreach (var monthModeButton in PersianCalendar.monthModeButtons)
                monthModeButton.Click += (s, e) => this.persianCalnedarPopup.IsOpen = false;

            PersianCalendar.btnToDay.Click += (s, e) => this.persianCalnedarPopup.IsOpen = false;
        }

        private void datePicker_Loaded(object sender, RoutedEventArgs e)
        {
            this.CoerceValue(PersianDatePicker.PersianSelectedDateProperty);
        }

        static object CoercePersianSelectedDate(DependencyObject d, object o)
        {
            PersianDatePicker pdp = d as PersianDatePicker;
            PersianDate value = (PersianDate) o;

            if (value != null)
            {
                if (value < pdp.DisplayDateStart)
                    return pdp.DisplayDateStart;

                if (value > pdp.DisplayDateEnd)
                    return pdp.DisplayDateEnd;
            }
            else
            {
                if (pdp.AllowNull == false)
                {
                    if (pdp.PersianSelectedDate == null)
                        return PersianDate.Today;
                    else
                        return pdp.PersianSelectedDate;
                }
            }

            return o;
        }

        static object CoerceDisplayDate(DependencyObject d, object o)
        {
            PersianDatePicker pdp = d as PersianDatePicker;
            PersianDate value = (PersianDate) o;

            if (value != null)
            {
                if (value < pdp.DisplayDateStart)
                    return pdp.DisplayDateStart;

                if (value > pdp.DisplayDateEnd)
                    return pdp.DisplayDateEnd;
            }

            return o;
        }

        static object CoerceDisplayDateEnd(DependencyObject d, object o)
        {
            PersianDatePicker pdp = d as PersianDatePicker;
            PersianDate value = (PersianDate) o;

            if (value < pdp.DisplayDateStart)
                return pdp.DisplayDateStart;

            return o;
        }

        static void OnPersianSelectedDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PersianDatePicker pdp = d as PersianDatePicker;

            if (e.NewValue != null)
            {
                pdp.Text = e.NewValue.ToString();
                d.SetValue(SelectedDateProperty, (e.NewValue as PersianDate).ToDateTime());
            }
            else
            {
                pdp.Text = string.Empty;
                d.SetValue(SelectedDateProperty, null);
            }

            pdp.RaiseEvent(new RoutedEventArgs(SelectedDateChangedEvent, pdp));
        }

        static void OnSelectedDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PersianDatePicker pdp = d as PersianDatePicker;
            var date = e.NewValue as DateTime?;

            if (date != null)
            {
                if (pdp.PersianSelectedDate == null || pdp.PersianSelectedDate.ToDateTime() != date.Value)
                {
                    PersianDate newDate;
                    if (PersianDate.TryParse(date.Value, out newDate))
                        d.SetValue(PersianSelectedDateProperty, newDate);
                    else
                        d.SetValue(PersianSelectedDateProperty, null);
                }
            }
            else
                d.SetValue(PersianSelectedDateProperty, null);
        }

        private void ValidateText()
        {
            PersianDate date;

            string txt = this.Text.Replace('_', ' ');

            if (PersianDate.TryParse(txt, out date))
            {
                this.PersianSelectedDate = date;
                this.DisplayDate = date;
                this.Text = date.ToString();
            }
            else
            {
                this.PersianSelectedDate = null;
                this.DisplayDate = this.PersianSelectedDate ?? PersianDate.Today;
                if (this.AllowNull)
                    this.Text = "____/__/__";
                else
                    this.Text = this.PersianSelectedDate.ToString();
            }
        }

        private void dateTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            ValidateText();
        }

        private void dateTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                ValidateText();
                dateTextBox.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }

            if (e.Key == Key.F2)
                persianCalnedarPopup.IsOpen = true;
        }

        private void dateTextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            var textBox = e.OriginalSource as C1MaskedTextBox;
            if (textBox != null)
                textBox.SelectAll();
        }

        private void dateTextBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var textbox = (sender as C1MaskedTextBox);
            if (textbox != null)
            {
                if (!textbox.IsKeyboardFocusWithin)
                {
                    if (e.OriginalSource.GetType().Name == "TextBoxView")
                    {
                        e.Handled = true;
                        textbox.Focus();
                    }
                }
                else if (textbox.SelectionLength == textbox.GetLineLength(0))
                {
                    e.Handled = true;
                    dateTextBox.SelectionStart = 0;
                    dateTextBox.SelectionLength = 4;
                }
            }
        }

        private void dateTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Divide || e.Key == Key.Oem2 || e.Key == Key.Space)
            {
                e.Handled = true;
                if (dateTextBox.SelectionStart.Between(0, 4))
                {
                    dateTextBox.SelectionStart = 5;
                    dateTextBox.SelectionLength = 2;
                }
                else if (dateTextBox.SelectionStart.Between(5, 7))
                {
                    dateTextBox.SelectionStart = 8;
                    dateTextBox.SelectionLength = 2;
                }
                else if(dateTextBox.SelectionStart.Between(8, 10))
                {
                    dateTextBox.SelectionStart = 0;
                    dateTextBox.SelectionLength = 4;
                }
            }
            else if (e.Key == Key.Up || e.Key == Key.Down)
            {
                ChangeDateByKey(e.Key == Key.Up ? 1 : -1);
            }
        }
        private void ChangeDateByKey(int operation)
        {
            byte status = 0;
            if (dateTextBox.SelectionStart.Between(0, 4))
                status = 1;
            else if (dateTextBox.SelectionStart.Between(5, 7))
                status = 2;
            else
                status = 3;

            int year;
            int month;
            int day;
            int selectionStart = 0;

            int.TryParse(Text.Substring(0, 4).Replace("_", ""), out year);
            int.TryParse(Text.Substring(5, 2).Replace("_", ""), out month);
            int.TryParse(Text.Substring(8, 2).Replace("_", ""), out day);

            switch (status)
            {
                case 1:
                    year = year + operation;
                    if (year > 9999)
                        year = 1;
                    if (year < 1)
                        year = 9999;
                    selectionStart = 4;
                    break;
                case 2:
                    month = month + operation;
                    if (month > 12)
                        month = 1;
                    if (month < 1)
                        month = 12;
                    selectionStart = 7;
                    break;
                case 3:
                    day = day + operation;
                    if (day > 31)
                        day = 1;
                    if (day < 1)
                        day = 31;
                    selectionStart = 10;
                    break;
            }

            if (day == 0)
                day = 1;
            if (month == 0)
                month = 1;
            if (year == 0)
                year = 1;
            Text = string.Format("{0:D4}/{1:D2}/{2:D2}", year, month, day);
            dateTextBox.SelectionStart = selectionStart;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ValidateText();
            persianCalnedarPopup.IsOpen = true;
        }

        private void persianCalnedarPopup_Opened(object sender, EventArgs e)
        {
            this.persianCalendar.Focus();
        }

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                persianCalnedarPopup.IsOpen = false;

                if (this.PersianSelectedDate != null)
                    this.Text = this.PersianSelectedDate.ToString();
                else
                {
                    if (this.AllowNull)
                        this.Text = string.Empty;
                    else
                        this.Text = this.PersianSelectedDate.ToString();
                }
                dateTextBox.Focus();
            }
        }

        #endregion
    }
}