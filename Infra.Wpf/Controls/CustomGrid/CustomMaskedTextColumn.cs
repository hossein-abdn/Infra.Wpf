using System.Windows.Data;
using C1.WPF.DataGrid;
using C1.WPF;
using System.Windows;
using System.Windows.Media;
using C1.WPF.DataGrid.Filters;
using Infra.Wpf.Converters;

namespace Infra.Wpf.Controls
{
    public class CustomMaskedTextColumn : DataGridTextColumn
    {
        #region Methods
        
        public CustomMaskedTextColumn()
        {
            TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
        }
        
        public override object GetCellContentRecyclingKey(C1.WPF.DataGrid.DataGridRow row)
        {
            return typeof(C1MaskedTextBox);
        }
        
        public override System.Windows.FrameworkElement CreateCellContent(C1.WPF.DataGrid.DataGridRow row)
        {
            return new C1MaskedTextBox();
        }

        public override string GetCellText(DataGridRow row)
        {
            C1MaskedTextBox maskedTextBox = CreateCellContent(row) as C1MaskedTextBox;
            BindCellContent(maskedTextBox, row);
            string text = maskedTextBox.Text;
            UnbindCellContent(maskedTextBox, row);

            return text;
        }
        
        public override void BindCellContent(System.Windows.FrameworkElement cellContent, C1.WPF.DataGrid.DataGridRow row)
        {
            var maskedTextBox = (C1MaskedTextBox)cellContent;            
            if (Binding == null)
            {
                maskedTextBox.ClearValue(C1MaskedTextBox.ValueProperty);
                return;
            }
            
            maskedTextBox.Mask = Mask;
            maskedTextBox.TextMaskFormat = TextMaskFormat;
            maskedTextBox.PromptChar = ' ';
            maskedTextBox.TextWrapping = TextWrapping;
            maskedTextBox.HorizontalAlignment = base.HorizontalAlignment;
            maskedTextBox.VerticalAlignment = base.VerticalAlignment;
            maskedTextBox.BorderBrush = null;
            maskedTextBox.Background = Brushes.Transparent;
            if (FlowDirection != null)
                maskedTextBox.FlowDirection = (FlowDirection)FlowDirection;
            
            if (Binding != null)
            {
                Binding newBinding = CopyBinding(Binding);
                newBinding.Source = row.DataItem;
                newBinding.Mode = BindingMode.OneWay;
                maskedTextBox.SetBinding(C1MaskedTextBox.ValueProperty, newBinding);
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
                ConverterParameter = new string[2] { multiValueFilter.DisplayMemberPath, this.Mask },
                Mode = BindingMode.OneWay
            };
            
            multiValueFilter.SetBinding(CustomMultiValueFilter.ItemsSourceProperty, bind);
            
            CustomMaskedTextFilter maskedTextFilter = new CustomMaskedTextFilter
            {
                Mask = this.Mask,
                MaskFlowDirection = this.FlowDirection,
                TextMaskFormat = this.TextMaskFormat
            };
            
            DataGridFilterList filterList = new DataGridFilterList();
            filterList.Items.Add(maskedTextFilter);
            filterList.Items.Add(multiValueFilter);
            
            DataGridContentFilter filter = new DataGridContentFilter();
            filter.Content = filterList;
            return filter;
        }
        
        #endregion
        
        #region Properties
        
        public string Mask { get; set; }
        
        public MaskFormat TextMaskFormat { get; set; }
        
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
            DependencyProperty.Register("FlowDirection", typeof(FlowDirection?), typeof(CustomMaskedTextColumn), new PropertyMetadata(null));
    
        #endregion
    }
}