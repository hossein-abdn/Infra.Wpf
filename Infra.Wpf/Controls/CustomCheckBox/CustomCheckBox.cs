using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Infra.Wpf.Controls
{
    public class CustomCheckBox : CheckBox
    {
        static CustomCheckBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomCheckBox), new FrameworkPropertyMetadata(typeof(CustomCheckBox)));
        }

        public override void OnApplyTemplate()
        {
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

            var validationBorder = (Border) GetTemplateChild("validationBorder");

            var borderBind = new Binding("(Validation.HasError)")
            {
                Source = this,
                Converter = new Converters.VisibilityToBoolConverter(),
                Mode = BindingMode.OneWay
            };

            BindingOperations.SetBinding(validationBorder, Border.VisibilityProperty, borderBind);
        }
    }
}
