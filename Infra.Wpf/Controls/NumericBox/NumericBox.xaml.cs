using Infra.Wpf.Common.Helpers;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Globalization;

namespace Infra.Wpf.Controls
{
    public partial class NumericBox : UserControl
    {
        #region Properties

        public long Increment
        {
            get
            {
                return (long) GetValue(IncrementProperty);
            }
            set
            {
                SetValue(IncrementProperty, value);
            }
        }

        public static readonly DependencyProperty IncrementProperty =
            DependencyProperty.Register("Increment", typeof(long), typeof(NumericBox), new PropertyMetadata(1l));

        public long MaxValue
        {
            get
            {
                return (long) GetValue(MaxValueProperty);
            }
            set
            {
                SetValue(MaxValueProperty, value);
            }
        }

        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(long), typeof(NumericBox), new PropertyMetadata(long.MaxValue));

        public long MinValue
        {
            get
            {
                return (long) GetValue(MinValueProperty);
            }
            set
            {
                SetValue(MinValueProperty, value);
            }
        }

        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register("MinValue", typeof(long), typeof(NumericBox), new PropertyMetadata(long.MinValue));

        public long? Value
        {
            get
            {
                return (long?) GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);
            }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(long?), typeof(NumericBox),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, ValueChanged, CoerceValue));

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
            DependencyProperty.Register("AllowNull", typeof(bool), typeof(NumericBox), new PropertyMetadata(true));

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
            DependencyProperty.Register("ShowButtons", typeof(bool), typeof(NumericBox), new PropertyMetadata(true));

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
            DependencyProperty.Register("Interval", typeof(int), typeof(NumericBox), new PropertyMetadata(50));

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
            DependencyProperty.Register("Delay", typeof(int), typeof(NumericBox), new PropertyMetadata(300));

        public event EventHandler<NumericBoxValueChangedEventArgs> OnValueChanged;

        #endregion

        #region Methods

        public NumericBox()
        {
            InitializeComponent();

            var converter = new CustomConverter((v, t, p, c) => v?.ToString(), (v, t, p, c) =>
            {
                long number;
                var result = long.TryParse(v.ToString(), out number);
                if (result == true)
                    return number;

                return null;
            });

            txtNumericEditor.SetBinding(TextBox.TextProperty, new Binding("Value")
            {
                Source = this,
                Converter = converter
            });
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.CoerceValue(NumericBox.ValueProperty);
        }

        private static object CoerceValue(DependencyObject d, object baseValue)
        {
            var nb = d as NumericBox;
            var value = baseValue as long?;

            if (value.HasValue)
            {
                if (value.Value < nb.MinValue)
                    return (long?) nb.MinValue;
                if (value.Value > nb.MaxValue)
                    return (long?) nb.MaxValue;
            }
            else
            {
                if (nb.AllowNull == false)
                    return 0l;
            }

            return baseValue;
        }

        private static void ValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var nb = d as NumericBox;
            nb.OnValueChanged?.Invoke(nb, new NumericBoxValueChangedEventArgs((long?) e.OldValue, (long?) e.NewValue));
        }

        private void btnIncrease_Click(object sender, RoutedEventArgs e)
        {
            if (Value.HasValue)
                Value += Increment;
            else
                Value = Increment;
        }

        private void btnDecrease_Click(object sender, RoutedEventArgs e)
        {
            if (Value.HasValue)
                Value -= Increment;
            else
                Value = 0;
        }

        private void txtNumericEditor_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
                btnIncrease_Click(sender, new RoutedEventArgs());
            else if (e.Delta < 0)
                btnDecrease_Click(sender, new RoutedEventArgs());
        }

        private void txtNumericEditor_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (txtNumericEditor != null)
                txtNumericEditor.SelectAll();
        }

        private void txtNumericEditor_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (txtNumericEditor != null)
            {
                if (!txtNumericEditor.IsKeyboardFocusWithin)
                {
                    if (e.OriginalSource.GetType().Name == "TextBoxView")
                    {
                        e.Handled = true;
                        txtNumericEditor.Focus();
                    }
                }
            }
        }

        private void txtNumericEditor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txtNumericEditor.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }

        private void txtNumericEditor_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                btnIncrease_Click(sender, new RoutedEventArgs());
                return;
            }
            else if (e.Key == Key.Down)
            {
                btnDecrease_Click(sender, new RoutedEventArgs());
                return;
            }

            if ((e.Key < Key.D0 || e.Key > Key.D9) && (e.Key < Key.NumPad0 || e.Key > Key.NumPad9) && e.Key != Key.Back && e.Key != Key.Delete &&
                e.Key != Key.Right && e.Key != Key.Left && e.Key != Key.Enter && e.Key != Key.Tab)
                e.Handled = true;
        }

        #endregion
    }
}