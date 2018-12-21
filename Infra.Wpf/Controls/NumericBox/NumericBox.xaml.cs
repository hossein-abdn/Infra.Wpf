using Infra.Wpf.Common.Helpers;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;

namespace Infra.Wpf.Controls
{
    public partial class NumericBox : UserControl
    {
        #region Properties

        public double Increment
        {
            get
            {
                return (double)GetValue(IncrementProperty);
            }
            set
            {
                SetValue(IncrementProperty, value);
            }
        }

        public static readonly DependencyProperty IncrementProperty =
            DependencyProperty.Register("Increment", typeof(double), typeof(NumericBox), new PropertyMetadata(1d));

        public double MaxValue
        {
            get
            {
                return (double)GetValue(MaxValueProperty);
            }
            set
            {
                SetValue(MaxValueProperty, value);
            }
        }

        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(double), typeof(NumericBox), new PropertyMetadata(double.MaxValue));

        public double MinValue
        {
            get
            {
                return (double)GetValue(MinValueProperty);
            }
            set
            {
                SetValue(MinValueProperty, value);
            }
        }

        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register("MinValue", typeof(double), typeof(NumericBox), new PropertyMetadata(double.MinValue));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(NumericBox), new PropertyMetadata(null, null, TextCoerce));

        public double? Value
        {
            get
            {
                return (double?)GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);
            }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double?), typeof(NumericBox),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, ValueChanged, ValueCoerce));

        public bool AllowNull
        {
            get
            {
                return (bool)GetValue(AllowNullProperty);
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
                return (bool)GetValue(ShowButtonsProperty);
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
                return (int)GetValue(IntervalProperty);
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
                return (int)GetValue(DelayProperty);
            }
            set
            {
                SetValue(DelayProperty, value);
            }
        }

        public static readonly DependencyProperty DelayProperty =
            DependencyProperty.Register("Delay", typeof(int), typeof(NumericBox), new PropertyMetadata(300));

        public string Format
        {
            get { return (string)GetValue(FormatProperty); }
            set { SetValue(FormatProperty, value); }
        }

        public static readonly DependencyProperty FormatProperty =
            DependencyProperty.Register("Format", typeof(string), typeof(NumericBox), new PropertyMetadata("F0", null, FormatCoerce));

        public event EventHandler<NumericBoxValueChangedEventArgs> OnValueChanged;

        private bool changeByTextChanged { get; set; }

        private bool isDeletePressed { get; set; }

        private bool isBacksapcePressed { get; set; }

        #endregion

        #region Methods

        public NumericBox()
        {
            InitializeComponent();

            Func<object, Type, object, CultureInfo, object> convert = (v, t, p, c) =>
            {
                if (v == null)
                    return "";

                return ((double)v).ToString(Format);
            };

            Func<object, Type, object, CultureInfo, object> convertBack = (v, t, p, c) =>
            {
                double number;
                var result = double.TryParse(v.ToString(), out number);
                if (result == true)
                    return number;
                else
                    return Value;
            };


            var converter = new CustomConverter(convert, convertBack);

            this.SetBinding(NumericBox.TextProperty, new Binding("Value")
            {
                Source = this,
                Converter = converter,
                Mode = BindingMode.TwoWay
            });
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.CoerceValue(NumericBox.ValueProperty);

            if (IsFocused == true)
                this.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));

            SetValidationStyle();
        }

        private void SetValidationStyle()
        {
            var style = new Style();

            if (Style != null)
            {
                style.BasedOn = Style.BasedOn;
                style.Resources = Style.Resources;
                style.TargetType = Style.TargetType;

                if (Style.Setters != null)
                {
                    foreach (var item in Style.Setters)
                        style.Setters.Add(item);
                }

                if (Style.Triggers != null)
                {
                    foreach (var item in Style.Triggers)
                        style.Triggers.Add(item);
                }
            }

            var trigger = new Trigger()
            {
                Property = Validation.HasErrorProperty,
                Value = true
            };

            var bind = new Binding("(Validation.Errors)[0].ErrorContent")
            {
                RelativeSource = new RelativeSource(RelativeSourceMode.Self)
            };

            trigger.Setters.Add(new Setter(ToolTipProperty, bind));
            style.Triggers.Add(trigger);
            Style = style;

            Validation.SetErrorTemplate(this, new ControlTemplate());

            var borderBind = new Binding("(Validation.HasError)")
            {
                Source = this,
                Converter = new Converters.VisibilityToBoolConverter(),
                Mode = BindingMode.OneWay
            };

            BindingOperations.SetBinding(validationBorder, Border.VisibilityProperty, borderBind);
        }

        private static object FormatCoerce(DependencyObject d, object baseValue)
        {
            string value = (string)baseValue;

            if (string.IsNullOrEmpty(value))
                return "F0";

            string format = value.Substring(0, 1);
            if (format.ToLower() != "n" && format.ToLower() != "f")
                return "F0";

            string digit = "0";
            if (value.Length > 1)
            {
                digit = value.Substring(1, value.Length - 1);
                try
                {

                    Convert.ToInt32(digit);
                }
                catch
                {
                    digit = "0";
                }
            }

            return format + digit;
        }

        private static object TextCoerce(DependencyObject d, object baseValue)
        {
            double result;
            if (double.TryParse(baseValue?.ToString(), out result) == false)
            {
                if ((d as NumericBox).AllowNull == true)
                    return null;
                else
                    return "0";
            }
            return baseValue;
        }

        private static object ValueCoerce(DependencyObject d, object baseValue)
        {
            var nb = d as NumericBox;
            var value = baseValue as double?;

            if (value.HasValue)
            {
                if (value.Value < nb.MinValue)
                    return (double?)nb.MinValue;
                if (value.Value > nb.MaxValue)
                    return (double?)nb.MaxValue;
            }
            else
            {
                if (nb.AllowNull == false)
                    return 0d;
            }

            return baseValue;
        }

        private static void ValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var nb = d as NumericBox;
            nb.OnValueChanged?.Invoke(nb, new NumericBoxValueChangedEventArgs((double?)e.OldValue, (double?)e.NewValue));
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
            else if ((e.Key == Key.Decimal || (Keyboard.Modifiers == ModifierKeys.None && e.Key == Key.OemPeriod)) &&
                    (Format.Substring(1, Format.Length - 1) == "0" || txtNumericEditor.Text.Contains(".")))
                e.Handled = true;
            else if ((e.Key == Key.Subtract || (Keyboard.Modifiers == ModifierKeys.None && e.Key == Key.OemMinus)) &&
                    (txtNumericEditor.SelectionStart != 0 || txtNumericEditor.Text.Contains("-")))
                e.Handled = true;
            else if (e.Key == Key.Delete && Keyboard.Modifiers == ModifierKeys.None)
            {
                isDeletePressed = true;
                txtNumericEditor_TextChanged(this, new TextChangedEventArgs(TextBox.TextChangedEvent, UndoAction.Create) { Source = txtNumericEditor });
                e.Handled = true;
            }
            else if (e.Key == Key.Back && Keyboard.Modifiers == ModifierKeys.None)
            {
                isBacksapcePressed = true;
                txtNumericEditor_TextChanged(this, new TextChangedEventArgs(TextBox.TextChangedEvent, UndoAction.Create) { Source = txtNumericEditor });
                e.Handled = true;
            }
            else if ((e.Key < Key.D0 || e.Key > Key.D9) && (e.Key < Key.NumPad0 || e.Key > Key.NumPad9) &&
                e.Key != Key.Right && e.Key != Key.Left && e.Key != Key.Enter && e.Key != Key.Tab && e.Key != Key.Home && e.Key != Key.End &&
                e.Key != Key.Decimal && (e.Key != Key.OemPeriod || (e.Key == Key.OemPeriod && Keyboard.Modifiers != ModifierKeys.None)) &&
                e.Key != Key.Subtract && (e.Key != Key.OemMinus || (e.Key == Key.OemMinus && Keyboard.Modifiers != ModifierKeys.None)))
                e.Handled = true;
        }

        private void txtNumericEditor_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (changeByTextChanged)
            {
                changeByTextChanged = false;
                return;
            }

            if (isDeletePressed)
            {
                isDeletePressed = false;
                if (txtNumericEditor.SelectionStart != txtNumericEditor.Text.Length)
                {
                    if (txtNumericEditor.SelectionLength > 0)
                        txtNumericEditor.SelectedText = "";
                    else if (txtNumericEditor.SelectionStart < (txtNumericEditor.Text.Length - 1) && txtNumericEditor.Text[txtNumericEditor.SelectionStart] == ',')
                    {
                        txtNumericEditor.SelectionLength = 2;
                        txtNumericEditor.SelectedText = "";
                    }
                    else
                    {
                        txtNumericEditor.SelectionLength = 1;
                        txtNumericEditor.SelectedText = "";
                    }
                }
                return;
            }

            if (isBacksapcePressed)
            {
                isBacksapcePressed = false;
                if (txtNumericEditor.SelectionStart != 0)
                {
                    if (txtNumericEditor.SelectionLength > 0)
                        txtNumericEditor.SelectedText = "";
                    else if (txtNumericEditor.SelectionStart > 1 && txtNumericEditor.Text[txtNumericEditor.SelectionStart - 1] == ',')
                    {
                        txtNumericEditor.SelectionStart -= 2;
                        txtNumericEditor.SelectionLength = 2;
                        txtNumericEditor.SelectedText = "";
                    }
                    else
                    {
                        txtNumericEditor.SelectionStart--;
                        txtNumericEditor.SelectionLength = 1;
                        txtNumericEditor.SelectedText = "";
                    }
                }
                return;
            }

            if (Format[0].ToString().ToLower() == "f")
                return;
            if (string.IsNullOrWhiteSpace(txtNumericEditor.Text))
                return;

            string minus = txtNumericEditor.Text[0] == '-' ? "-" : "";

            string decimalPart = "";
            int decimalPoint = txtNumericEditor.Text.IndexOf(".");
            if (Format.Substring(1, Format.Length - 1) != "0" && decimalPoint != (-1))
                decimalPart = txtNumericEditor.Text.Substring(decimalPoint, txtNumericEditor.Text.Length - decimalPoint);

            int startPoint = 0;
            if (string.IsNullOrEmpty(minus) == false)
                startPoint = 1;

            string intPart = txtNumericEditor.Text.Substring(startPoint, txtNumericEditor.Text.Length - decimalPart.Length - minus.Length);
            intPart = CleanSeparator(intPart);
            string seperatedIntPart = GetSeparatedNumber(intPart);

            var currentSelectionStart = txtNumericEditor.SelectionStart;
            var currentText = txtNumericEditor.Text;
            changeByTextChanged = true;
            txtNumericEditor.Text = minus + seperatedIntPart + decimalPart;
            changeByTextChanged = false;
            txtNumericEditor.SelectionStart = CalculateSelectionStart(currentSelectionStart, intPart.Length, currentText, minus.Length > 0);
        }

        public int CalculateSelectionStart(int currentSelectionStart, int intPartLength, string currentText, bool minus)
        {
            if (currentSelectionStart == 0)
                return 0;

            int commaCount = 0;
            for (int i = 0; i < currentSelectionStart; i++)
            {
                if (currentText[i] == ',')
                    commaCount++;
            }
            currentSelectionStart -= commaCount;

            int separatorCount = (intPartLength - 1) / 3;
            int separatorBeforeSelection;
            if (currentSelectionStart > intPartLength)
                separatorBeforeSelection = separatorCount;
            else
                separatorBeforeSelection = separatorCount - ((intPartLength - currentSelectionStart + Convert.ToInt32(minus)) / 3);

            return currentSelectionStart + separatorBeforeSelection;
        }

        private string CleanSeparator(string digit)
        {
            if (string.IsNullOrWhiteSpace(digit))
                return "";

            string result = "";
            for (int i = digit.Length - 1; i >= 0; i--)
            {
                if (digit[i] != ',')
                    result = result.Insert(0, digit[i].ToString());
            }

            return result;
        }

        private string GetSeparatedNumber(string digit)
        {
            if (string.IsNullOrWhiteSpace(digit))
                return "";

            string result = digit;
            for (int i = 1; i <= (digit.Length - 1) / 3; i++)
            {
                int separatorPos = digit.Length - (i * 3);
                result = result.Insert(separatorPos, ",");
            }

            return result;
        }

        #endregion
    }
}