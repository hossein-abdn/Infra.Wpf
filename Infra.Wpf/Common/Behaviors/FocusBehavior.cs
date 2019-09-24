using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;

namespace Infra.Wpf.Common.Behaviors
{
    public class FocusBehavior:Behavior<UIElement>
    {
        public int Delay
        {
            get { return (int)GetValue(DelayProperty); }
            set { SetValue(DelayProperty, value); }
        }

        public static readonly DependencyProperty DelayProperty =
            DependencyProperty.Register("Delay", typeof(int), typeof(FocusBehavior), new PropertyMetadata(100));

        protected override void OnAttached()
        {
            int delay = Delay;
            Task.Run(() =>
            {
                Thread.Sleep(delay);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    AssociatedObject.Focus();
                });
            });

            base.OnAttached();
        }
    }
}
