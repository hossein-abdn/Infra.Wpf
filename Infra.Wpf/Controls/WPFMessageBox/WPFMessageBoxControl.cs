using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Infra.Wpf.Controls
{
    internal class WPFMessageBoxControl : Window
    {
        Grid TitlePart;

        static WPFMessageBoxControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WPFMessageBoxControl),
                    new FrameworkPropertyMetadata(typeof(WPFMessageBoxControl)));
        }

        public WPFMessageBoxControl(Window owner = null)
        {
            SetValue(AllowsTransparencyProperty, true);
            SetValue(WindowStyleProperty, WindowStyle.None);

            Owner = owner;
            if (Owner != null)
                WindowStartupLocation = WindowStartupLocation.CenterOwner;
            else
                WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (TitlePart == null)
            {
                TitlePart = GetTemplateChild("TitlePart") as Grid;
                if (TitlePart != null)
                    TitlePart.MouseLeftButtonDown += new MouseButtonEventHandler(TitlePart_MouseLeftButtonDown);
            }
        }

        void TitlePart_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}