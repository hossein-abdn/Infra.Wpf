using System.Windows.Data;
using C1.WPF.DataGrid;
using System.Windows;
using C1.WPF.DataGrid.Filters;

namespace Infra.Wpf.Controls
{
    public class CustomNumericColumn : DataGridNumericColumn
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
            if (this.DataGrid.FlowDirection == FlowDirection.RightToLeft)
                textBlock.FlowDirection = FlowDirection.LeftToRight;

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
            DataGridContentFilter filter = new DataGridContentFilter();
            DataGridMultiLineNumericFilter filterContent = new DataGridMultiLineNumericFilter();
            filterContent.Format = this.GetActualFormat();
            filter.Content = filterContent;
            return filter;
        }
    }
}