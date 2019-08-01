using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Data;
using System.Runtime.CompilerServices;

namespace Infra.Wpf.Controls
{
    public abstract class Lookup : Control, INotifyPropertyChanged
    {
        #region Properties

        public event PropertyChangedEventHandler PropertyChanged;

        private Border validationBorder;

        private TextBox textboxPart;

        #endregion

        #region Methods

        static Lookup()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Lookup), new FrameworkPropertyMetadata(typeof(Lookup)));
        }

        public override void OnApplyTemplate()
        {
            validationBorder = Template.FindName("validationBorder", this) as Border;
            textboxPart = Template.FindName("textbox_PART", this) as TextBox;
            textboxPart.KeyDown += CustomTextBox_KeyDown;
            SetValidationStyle();
            base.OnApplyTemplate();
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

        public void OnPropertyChanged([CallerMemberName]string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public abstract void CustomTextBox_KeyDown(object sender, KeyEventArgs e);
        
        #endregion
    }
}
