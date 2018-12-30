using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Infra.Wpf.Common.Helpers
{
    public class ButtonCommand
    {
        public static DependencyProperty CommandProperty = DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(ButtonCommand), new PropertyMetadata(CommandChanged));

        public static DependencyProperty CommandParameterProperty = DependencyProperty.RegisterAttached("CommandParameter", typeof(object), typeof(ButtonCommand), new UIPropertyMetadata(null));

        public static void SetCommand(DependencyObject target, ICommand value)
        {
            target.SetValue(CommandProperty, value);
        }

        public static object GetCommand(DependencyObject target, ICommand value)
        {
            return target.GetValue(CommandProperty);
        }

        public static void SetCommandParameter(DependencyObject target, object value)
        {
            target.SetValue(CommandParameterProperty, value);
        }
        public static object GetCommandParameter(DependencyObject target)
        {
            return target.GetValue(CommandParameterProperty);
        }

        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            Button control = target as Button;
            if (control != null)
            {
                if ((e.NewValue != null) && (e.OldValue == null))
                    control.Click += Control_Click;
                else if ((e.NewValue == null) && (e.OldValue != null))
                    control.Click -= Control_Click;
            }
        }

        private static void Control_Click(object sender, RoutedEventArgs e)
        {
            Button control = sender as Button;
            ICommand command = (ICommand)control.GetValue(CommandProperty);
            object commandParameter = control.GetValue(CommandParameterProperty);
            command.Execute(commandParameter);
        }
    }
}
