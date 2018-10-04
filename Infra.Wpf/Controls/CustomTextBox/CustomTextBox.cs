using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Infra.Wpf.Controls
{
    public class CustomTextBox : TextBox
    {
        public CustomTextBoxFormat TextBoxFormat
        {
            get { return (CustomTextBoxFormat) GetValue(TextBoxFormatProperty); }
            set { SetValue(TextBoxFormatProperty, value); }
        }

        public static readonly DependencyProperty TextBoxFormatProperty = DependencyProperty.Register("TextBoxFormat", typeof(CustomTextBoxFormat), typeof(CustomTextBox),
            new FrameworkPropertyMetadata(CustomTextBoxFormat.String, FrameworkPropertyMetadataOptions.Inherits));

        public LanguageEnum Lang { get; set; }

        private CultureInfo inputLanguage;

        static CustomTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomTextBox), new FrameworkPropertyMetadata(typeof(CustomTextBox)));
        }

        public CustomTextBox()
        {
            GotKeyboardFocus += CustomTextBox_GotKeyboardFocus;
            PreviewMouseLeftButtonDown += CustomTextBox_PreviewMouseLeftButtonDown;
            KeyDown += CustomTextBox_KeyDown;
            PreviewKeyDown += CustomTextBox_PreviewKeyDown;
            Loaded += CustomTextBox_Loaded;
        }

        private void CustomTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            switch (Lang)
            {
                case LanguageEnum.Default:
                    if (FlowDirection == FlowDirection.RightToLeft)
                        ChangeLanguage("fa-IR");
                    break;
                case LanguageEnum.English:
                    ChangeLanguage("en-US");
                    break;
                case LanguageEnum.Farsi:
                    ChangeLanguage("fa-IR");
                    break;
            }

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

                if(Style.Setters!=null)
                {
                    foreach (var item in Style.Setters)
                        style.Setters.Add(item);
                }

                if(Style.Triggers !=null)
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

            var validationBorder = (Border) GetTemplateChild("validationBorder");

            var borderBind = new Binding("(Validation.HasError)")
            {
                Source = this,
                Converter = new Converters.VisibilityToBoolConverter(),
                Mode = BindingMode.OneWay
            };

            BindingOperations.SetBinding(validationBorder, Border.VisibilityProperty, borderBind);
        }

        private void ChangeLanguage(string cultureName)
        {
            if (System.Windows.Forms.InputLanguage.InstalledInputLanguages.Contains(System.Windows.Forms.InputLanguage.FromCulture(new CultureInfo(cultureName))))
            {
                inputLanguage = new CultureInfo(cultureName);
                GotFocus += CustomTextBox_GotFocus;
                LostFocus += CustomTextBox_LostFocus;
            }
        }

        private void CustomTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            InputLanguageManager.Current.CurrentInputLanguage = new CultureInfo("en-US");
        }

        private void CustomTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            InputLanguageManager.Current.CurrentInputLanguage = inputLanguage;
        }

        private void CustomTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (TextBoxFormat == CustomTextBoxFormat.Numeric)
            {
                if ((e.Key < Key.D0 || e.Key > Key.D9) && (e.Key < Key.NumPad0 || e.Key > Key.NumPad9) && e.Key != Key.Back && e.Key != Key.Delete &&
                    e.Key != Key.Right && e.Key != Key.Left && e.Key != Key.Enter && e.Key != Key.Subtract && e.Key != Key.Decimal && e.Key != Key.Space && e.Key != Key.Tab)
                    e.Handled = true;
            }
        }

        private void CustomTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                this.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }

        private void CustomTextBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!this.IsKeyboardFocusWithin)
            {
                if (e.OriginalSource.GetType().Name == "TextBoxView")
                {
                    e.Handled = true;
                    this.Focus();
                }
            }
        }

        private void CustomTextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            this.SelectAll();
        }
    }
}
