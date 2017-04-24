using System.Windows.Data;
using C1.WPF.DataGrid;
using System.Windows;
using C1.WPF.DataGrid.Filters;
using System.Collections.Generic;
using System.Linq;

namespace Infra.Wpf.Controls
{
    public class CustomCheckBoxColumn : DataGridCheckBoxColumn
    {
        public bool IsSelectable { get; set; }

        public override void BindCellContent(FrameworkElement cellContent, DataGridRow row)
        {
            System.Windows.Controls.CheckBox checkBox = (System.Windows.Controls.CheckBox) cellContent;
            if (Binding != null)
            {
                if (IsSelectable == false)
                {
                    Binding newBinding = base.CopyBinding(Binding);
                    newBinding.Source = row.DataItem;
                    checkBox.SetBinding(System.Windows.Controls.CheckBox.IsCheckedProperty, newBinding);
                }
                else
                {
                    Binding newBinding = base.CopyBinding(Binding);

                    string key = ((List<CheckBoxViewModel>) (newBinding.Source))?.FirstOrDefault().Key;
                    var keyInfo = row.DataItem.GetType().GetProperty(key);
                    var rowId = keyInfo?.GetValue(row.DataItem);

                    if (keyInfo != null)
                    {
                        newBinding.Source = ((List<CheckBoxViewModel>) (newBinding.Source))?.Where(x => keyInfo.GetValue(x.Item).Equals(rowId)).FirstOrDefault();
                        checkBox.SetBinding(System.Windows.Controls.CheckBox.IsCheckedProperty, newBinding);
                    }
                }
            }
            checkBox.HorizontalAlignment = base.HorizontalAlignment;
            checkBox.VerticalAlignment = base.VerticalAlignment;
            checkBox.Margin = new System.Windows.Thickness(4, 4, 4, 4);
            checkBox.FlowDirection = FlowDirection.LeftToRight;
        }

        public override FrameworkElement GetCellEditingContent(DataGridRow row)
        {
            if (IsSelectable == true)
            {
                var checkBox = (System.Windows.Controls.CheckBox) base.GetCellEditingContent(row);
                checkBox.HorizontalAlignment = base.HorizontalAlignment;
                checkBox.VerticalAlignment = base.VerticalAlignment;
                checkBox.Margin = new System.Windows.Thickness(4, 4, 4, 4);
                checkBox.FlowDirection = FlowDirection.LeftToRight;

                Binding newBinding = base.CopyBinding(Binding);

                string key = ((List<CheckBoxViewModel>) (newBinding.Source))?.FirstOrDefault().Key;
                var keyInfo = row.DataItem.GetType().GetProperty(key);
                var rowId = keyInfo?.GetValue(row.DataItem);

                if (keyInfo != null)
                {
                    newBinding.Source = ((List<CheckBoxViewModel>) (newBinding.Source))?.Where(x => keyInfo.GetValue(x.Item).Equals(rowId)).FirstOrDefault();
                    checkBox.SetBinding(System.Windows.Controls.CheckBox.IsCheckedProperty, newBinding);
                }

                return checkBox;
            }

            return base.GetCellEditingContent(row);
        }

        public override IDataGridFilter GetFilter()
        {
            DataGridContentFilter filter = new DataGridContentFilter();
            filter.Content = new CustomCheckBoxFilter();
            return filter;
        }

        public override string GetCellText(DataGridRow row)
        {
            System.Windows.Controls.CheckBox checkBox = CreateCellContent(row) as System.Windows.Controls.CheckBox;
            BindCellContent(checkBox, row);

            string text = "خیر";
            if (checkBox.IsChecked.Value)
                text = "بله";

            UnbindCellContent(checkBox, row);

            return text;
        }
    }
}