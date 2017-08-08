using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Infra.Wpf.Controls
{
    public class MultiSelect : ListBox
    {
        #region Properties

        TextBox textbox;
        WrapPanel wrappanel;
        Popup popup;
        bool flag;

        #endregion

        #region Methods

        static MultiSelect()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MultiSelect), new FrameworkPropertyMetadata(typeof(MultiSelect)));
        }

        public MultiSelect()
        {
            Loaded += MultiSelect_Loaded;
            var descriptor = DependencyPropertyDescriptor.FromProperty(ItemsSourceProperty, typeof(MultiSelect));
            if (descriptor != null)
                descriptor.AddValueChanged(this, OnItemsSourceChanged);
        }

        private void MultiSelect_Loaded(object sender, RoutedEventArgs e)
        {
            textbox = (TextBox) Template.FindName("textbox", this);
            textbox.GotFocus += Textbox_GotFocus;
            textbox.LostFocus += Textbox_LostFocus;
            wrappanel = (WrapPanel) textbox.Template.FindName("wrappanel", textbox);
            popup = (Popup) Template.FindName("popup", this);

            
        }

        private void OnItemsSourceChanged(object sender, EventArgs e)
        {
            if (ItemsSource == null || flag==true)
            {
                flag = false;
                return;
            }

            var list = new List<MultiItem>();
            foreach (var item in ItemsSource)
                list.Add(new MultiItem { IsSelected = false, Item = item });

            flag = true;
            ItemsSource = list;
        }

        private void Textbox_LostFocus(object sender, RoutedEventArgs e)
        {
            popup.IsOpen = false;
        }

        private void Textbox_GotFocus(object sender, RoutedEventArgs e)
        {
            popup.IsOpen = true;
        }

        private void PrepareItemsSource()
        {

            //BindingExpression bindExpression = BindingOperations.GetBindingExpression(customComboBox, SelectedItemProperty);
            //if (bindExpression != null && bindExpression.ResolvedSource != null)
            //{ }
            //< DataTemplate >
            //    < StackPanel Orientation = "Horizontal" >

            //         < ctl:CustomCheckBox Margin = "0,0,3,0" VerticalAlignment = "Center" />

            //            < ContentPresenter Content = "{Binding Value}" />

            //         </ StackPanel >

            //     </ DataTemplate >
        }

        #endregion
    }
}
