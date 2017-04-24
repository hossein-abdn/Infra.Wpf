using System.Windows;
using System.Windows.Data;
using C1.WPF.DataGrid;
using C1.WPF.DataGrid.Filters;
using Infra.Wpf.Converters;

namespace Infra.Wpf.Controls
{
    public class CustomTextColumn : DataGridTextColumn
    {
        public override void BindCellContent(FrameworkElement cellContent, DataGridRow row)
        {
            System.Windows.Controls.TextBlock textBlock = (System.Windows.Controls.TextBlock)cellContent;

            if (Binding == null)
            {
                textBlock.ClearValue(System.Windows.Controls.TextBlock.TextProperty);
                return;
            }

            textBlock.HorizontalAlignment = base.HorizontalAlignment;
            textBlock.VerticalAlignment = base.VerticalAlignment;
            textBlock.TextWrapping = TextWrapping;
            textBlock.TextTrimming = TextTrimming;
            textBlock.Margin = new Thickness(4, 4, 4, 4);
            if (FlowDirection != null)
                textBlock.FlowDirection = (FlowDirection)FlowDirection;
            if (Binding != null)
            {
                Binding newBinding = CopyBinding(Binding);
                newBinding.Source = row.DataItem;
                newBinding.Mode = BindingMode.OneWay;
                textBlock.SetBinding(System.Windows.Controls.TextBlock.TextProperty, newBinding);
            }
        }

        public override IDataGridFilter GetFilter()
        {
            CustomMultiValueFilter multiValueFilter = new CustomMultiValueFilter();
            multiValueFilter.CurrentGrid = this.DataGrid;
            multiValueFilter.DisplayMemberPath = this.Name;
            multiValueFilter.Direction = this.FlowDirection;

            Binding bind = new Binding("ItemsSource")
            {
                Source = this.DataGrid,
                Converter = new ItemsSourceToFilterItemConverter(),
                ConverterParameter = new string[2] { multiValueFilter.DisplayMemberPath, string.Empty },
                Mode = BindingMode.OneWay
            };
            multiValueFilter.SetBinding(CustomMultiValueFilter.ItemsSourceProperty, bind);
            DataGridFilterList filterList = new DataGridFilterList();
            filterList.Items.Add(new CustomTextFilter());
            filterList.Items.Add(multiValueFilter);
            
            DataGridContentFilter filter = new DataGridContentFilter();
            filter.Content = filterList;
            return filter;
        }

        public FlowDirection? FlowDirection
        {
            get
            {
                return (FlowDirection?)GetValue(FlowDirectionProperty);
            }
            set
            {
                SetValue(FlowDirectionProperty, value);
            }
        }

        public static readonly DependencyProperty FlowDirectionProperty =
            DependencyProperty.Register("FlowDirection", typeof(FlowDirection?), typeof(CustomTextColumn), new PropertyMetadata(null));
    }
}