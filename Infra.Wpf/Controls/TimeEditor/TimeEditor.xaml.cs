using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using C1.WPF;
using Infra.Wpf.Common;
using Infra.Wpf.Common.Helpers;

namespace Infra.Wpf.Controls
{
    public partial class TimeEditor : UserControl, INotifyPropertyChanged
    {
        #region Properties

        public TimeEditorFormat Format
        {
            get
            {
                return (TimeEditorFormat) GetValue(FormatProperty);
            }
            set
            {
                SetValue(FormatProperty, value);
            }
        }

        public static readonly DependencyProperty FormatProperty =
            DependencyProperty.Register("Format", typeof(TimeEditorFormat), typeof(TimeEditor),
                new PropertyMetadata(TimeEditorFormat.Short, TimeEditorFormatChanged));

        public TimeSpan Increment
        {
            get
            {
                return (TimeSpan) GetValue(IncrementProperty);
            }
            set
            {
                SetValue(IncrementProperty, value);
            }
        }

        public static readonly DependencyProperty IncrementProperty =
            DependencyProperty.Register("Increment", typeof(TimeSpan), typeof(TimeEditor),
                new PropertyMetadata(new TimeSpan(0, 1, 0)));

        public TimeSpan MaxValue
        {
            get
            {
                return (TimeSpan) GetValue(MaxValueProperty);
            }
            set
            {
                SetValue(MaxValueProperty, value);
            }
        }

        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(TimeSpan), typeof(TimeEditor),
                new PropertyMetadata(new TimeSpan(0, 23, 59, 59)));

        public TimeSpan MinValue
        {
            get
            {
                return (TimeSpan) GetValue(MinValueProperty);
            }
            set
            {
                SetValue(MinValueProperty, value);
            }
        }

        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register("MinValue", typeof(TimeSpan), typeof(TimeEditor),
                new PropertyMetadata(new TimeSpan(0, 0, 0)));

        public TimeSpan? Value
        {
            get
            {
                return (TimeSpan?) GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);
            }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(TimeSpan?), typeof(TimeEditor),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    ValueChanged, CoerceValue));

        public string DisplayValue
        {
            get
            {
                if (Value.HasValue)
                {
                    switch (Format)
                    {
                        case TimeEditorFormat.Short:
                            return new PersianDate(Value.Value).ToString("hh:mm tt");
                        case TimeEditorFormat.Long:
                            return new PersianDate(Value.Value).ToString("hh:mm:ss tt");
                        case TimeEditorFormat.TimeSpan:
                            return Value.Value.ToString("hh\\:mm\\:ss");
                        case TimeEditorFormat.TimeSpanShort:
                            return Value.Value.ToString("hh\\:mm");
                    }
                }

                if (Format == TimeEditorFormat.Short || Format == TimeEditorFormat.TimeSpanShort)
                    return "__:__";
                else
                    return "__:__:__";
            }
        }

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

        public static readonly DependencyProperty AllowNullProperty =
            DependencyProperty.Register("AllowNull", typeof(bool), typeof(TimeEditor), new PropertyMetadata(true));

        public bool ShowButtons
        {
            get
            {
                return (bool) GetValue(ShowButtonsProperty);
            }
            set
            {
                SetValue(ShowButtonsProperty, value);
            }
        }

        public static readonly DependencyProperty ShowButtonsProperty =
            DependencyProperty.Register("ShowButtons", typeof(bool), typeof(TimeEditor), new PropertyMetadata(true));

        public int Interval
        {
            get
            {
                return (int) GetValue(IntervalProperty);
            }
            set
            {
                SetValue(IntervalProperty, value);
            }
        }

        public static readonly DependencyProperty IntervalProperty =
            DependencyProperty.Register("Interval", typeof(int), typeof(TimeEditor), new PropertyMetadata(50));

        public int Delay
        {
            get
            {
                return (int) GetValue(DelayProperty);
            }
            set
            {
                SetValue(DelayProperty, value);
            }
        }

        public static readonly DependencyProperty DelayProperty =
            DependencyProperty.Register("Delay", typeof(int), typeof(TimeEditor), new PropertyMetadata(300));

        private string _Mask;

        public string Mask
        {
            get
            {
                return _Mask;
            }
            private set
            {
                _Mask = value;
                OnPropertyChanged();
            }
        }

        Common.Helpers.CustomConverter TimeSpanToStringConverter = null;

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler<TimeEditorValueChangedEventArgs> OnValueChanged;

        #endregion

        #region Methods

        public TimeEditor()
        {
            InitializeComponent();
            Mask = "00:00";

            TimeSpanToStringConverter = new Common.Helpers.CustomConverter(
                (v, t, p, c) =>
                {
                    TimeSpan? source = v as TimeSpan?;

                    if (source != null)
                    {
                        if (Format == TimeEditorFormat.Long || Format == TimeEditorFormat.TimeSpan)
                            return source.Value.ToString("hh\\:mm\\:ss");
                        else
                            return source.Value.ToString("hh\\:mm");
                    }
                    else
                        return null;
                },
                (v, t, p, c) =>
                {
                    string source = v.ToString();
                    source = source.Replace('_', ' ');

                    string[] parts = source.Split(':');

                    var style = NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite;
                    int h = 0;
                    int m = 0;
                    int s = 0;

                    if (string.IsNullOrWhiteSpace(parts[0]) || string.IsNullOrWhiteSpace(parts[1]))
                        return null;

                    if (parts.Length > 2)
                    {
                        if (string.IsNullOrWhiteSpace(parts[2]))
                            return null;
                        else
                            s = int.Parse(parts[2], style);
                    }

                    h = int.Parse(parts[0], style);
                    m = int.Parse(parts[1], style);

                    return new TimeSpan(h, m, s);
                });

            txtTimeEditor.SetBinding(C1MaskedTextBox.TextProperty, new Binding("Value")
            {
                Source = this,
                Converter = TimeSpanToStringConverter
            });
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.CoerceValue(TimeEditor.ValueProperty);
        }

        private static object CoerceValue(DependencyObject d, object baseValue)
        {
            var te = d as TimeEditor;
            var value = baseValue as TimeSpan?;

            if (value.HasValue)
            {
                if (value.Value < te.MinValue)
                    return (TimeSpan?) te.MaxValue;
                if (value.Value > te.MaxValue)
                    return (TimeSpan?) te.MinValue;
            }
            else
            {
                if (te.AllowNull == false)
                    return new TimeSpan(0, 0, 0);
            }

            return baseValue;
        }

        private static void TimeEditorFormatChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var te = d as TimeEditor;
            var value = (TimeEditorFormat) e.NewValue;

            switch (value)
            {
                case TimeEditorFormat.Short:
                case TimeEditorFormat.TimeSpanShort:
                    te.Mask = "00:00";
                    break;
                case TimeEditorFormat.Long:
                case TimeEditorFormat.TimeSpan:
                    te.Mask = "00:00:00";
                    break;
            }
        }

        private static void ValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var te = d as TimeEditor;
            te.OnPropertyChanged("DisplayValue");
            if (te.OnValueChanged != null)
                te.OnValueChanged(te, new TimeEditorValueChangedEventArgs((TimeSpan?) e.OldValue, (TimeSpan?) e.NewValue));
        }

        private void OnPropertyChanged([CallerMemberName] string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private void btnIncrease_Click(object sender, RoutedEventArgs e)
        {
            if (Value.HasValue)
                Value = ((TimeSpan) Value).Add(Increment);
            else
                Value = Increment;
        }

        private void btnDecrease_Click(object sender, RoutedEventArgs e)
        {
            if (Value.HasValue)
                Value = ((TimeSpan) Value).Subtract(Increment);
            else
                Value = new TimeSpan(24, 0, 0).Subtract(Increment);
        }

        private void txtTimeEditor_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
                btnIncrease_Click(sender, new RoutedEventArgs());
            else if (e.Delta < 0)
                btnDecrease_Click(sender, new RoutedEventArgs());
        }

        private void txtTimeEditor_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (txtTimeEditor != null)
                txtTimeEditor.SelectAll();
        }

        private void txtTimeEditor_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (txtTimeEditor != null)
            {
                if (!txtTimeEditor.IsKeyboardFocusWithin)
                {
                    if (e.OriginalSource.GetType().Name == "TextBoxView")
                    {
                        e.Handled = true;
                        txtTimeEditor.Focus();
                    }
                }
            }
        }

        private void txtTimeEditor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txtTimeEditor.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }

        private void txtTimeEditor_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up || e.Key == Key.Down)
            {
                ChangeDateByKey(e.Key == Key.Up ? 1 : -1);
            }
        }

        private void ChangeDateByKey(int operation)
        {
            byte status = 0;
            if (txtTimeEditor.SelectionStart.Between(0, 2))
                status = 1;
            else if (txtTimeEditor.SelectionStart.Between(3, 5))
                status = 2;
            else if (txtTimeEditor.SelectionStart.Between(6, 8))
                status = 3;

            int hour;
            int minute;
            int second = 0;
            int selectionStart = 0;

            int.TryParse(txtTimeEditor.Text.Substring(0, 2).Replace("_", ""), out hour);
            int.TryParse(txtTimeEditor.Text.Substring(3, 2).Replace("_", ""), out minute);
            if (Format == TimeEditorFormat.Long || Format == TimeEditorFormat.TimeSpan)
                int.TryParse(txtTimeEditor.Text.Substring(6, 2).Replace("_", ""), out second);

            switch (status)
            {
                case 1:
                    hour = hour + operation;
                    if (hour > MaxValue.Hours)
                        hour = MinValue.Hours;
                    if (hour < MinValue.Hours)
                        hour = MaxValue.Hours;
                    selectionStart = 2;
                    break;
                case 2:
                    minute = minute + operation;
                    if (minute > 59)
                        minute = 0;
                    if (minute < 0)
                        minute = 59;
                    selectionStart = 5;
                    break;
                case 3:
                    second = second + operation;
                    if (second > 59)
                        second = 0;
                    if (second < 0)
                        second = 59;
                    selectionStart = 8;
                    break;
            }

            if (Format == TimeEditorFormat.Long || Format == TimeEditorFormat.TimeSpan)
                txtTimeEditor.Text = string.Format("{0:D2}:{1:D2}:{2:D2}", hour, minute, second);
            else
                txtTimeEditor.Text = string.Format("{0:D2}:{1:D2}", hour, minute);
            txtTimeEditor.SelectionStart = selectionStart;
        }

        #endregion
    }
}