using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Infra.Wpf.Controls
{
    [ContentProperty("Content")]
    public class MultiSelectItem : ContentControl
    {
        static MultiSelectItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MultiSelectItem), new FrameworkPropertyMetadata(typeof(MultiSelectItem)));
        }

        public bool IsSelected
        {
            get { return (bool) GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(MultiSelectItem), new PropertyMetadata(false));

        public bool IsActive
        {
            get { return (bool) GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }

        public static readonly DependencyProperty IsActiveProperty =
            DependencyProperty.Register("IsActive", typeof(bool), typeof(MultiSelectItem), new PropertyMetadata(false));

        public object Item { get; set; }
    }
}
