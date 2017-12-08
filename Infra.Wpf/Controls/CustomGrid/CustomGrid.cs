using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using C1.WPF;
using C1.WPF.DataGrid;
using System.Windows.Media;
using System.Windows.Data;
using System.ComponentModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Infra.Wpf.Controls
{
    public class CustomGrid : C1DataGrid
    {
        #region Methods

        public CustomGrid()
        {
            ButtonColumns = new ObservableCollection<CustomButtonColumn>();
            this.Loaded += CustomGrid_Loaded;
            this.AutoGeneratingColumn += CustomGrid_AutoGeneratingColumn;
            C1RowIndexHeaderBehavior indexBehavior = new C1RowIndexHeaderBehavior() { StretchHeader = true };
            C1RowIndexHeaderBehavior.SetRowIndexHeaderBehavior(this, indexBehavior);

            var descriptor = DependencyPropertyDescriptor.FromProperty(CustomGrid.ItemsSourceProperty, typeof(CustomGrid));
            if (descriptor != null)
                descriptor.AddValueChanged(this, OnItemsSourceChanged);
        }

        private void OnItemsSourceChanged(object sender, EventArgs e)
        {
            ItemsSourceChanged?.Invoke(this, e);

            if (loadedFlag)
                OrderButtonColumns();
        }

        private void CustomGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (!string.IsNullOrEmpty(ColumnsToAdd) || !string.IsNullOrEmpty(ColumnsToRemove))
            {
                string[] removeColumnsList = null;
                if (!string.IsNullOrEmpty(ColumnsToRemove))
                {
                    removeColumnsList = ColumnsToRemove.Split(',');
                    if (removeColumnsList.Contains(e.Property.Name))
                        e.Cancel = true;
                }

                if (string.IsNullOrEmpty(ColumnsToRemove))
                {
                    string[] addColumnsList = null;
                    if (!string.IsNullOrEmpty(ColumnsToAdd))
                    {
                        addColumnsList = ColumnsToAdd.Split(',');
                        if (!addColumnsList.Contains(e.Property.Name))
                            e.Cancel = true;
                    }
                }
            }

            string dispalyName = GetDisplayName(e.Property.Name);
            if (e.Property.PropertyType == typeof(string) || e.Property.PropertyType == typeof(char))
            {
                var customColumn = new CustomTextColumn();
                if (e.Property.PropertyType == typeof(char))
                    customColumn.MaxLength = 1;

                if (!string.IsNullOrEmpty(dispalyName))
                    customColumn.Header = dispalyName;
                else
                    customColumn.Header = e.Property.Name;
                customColumn.IsReadOnly = true;
                customColumn.Binding = new Binding(e.Property.Name);
                e.Column = customColumn;
            }

            if (e.Property.PropertyType.IsNumeric())
            {
                var customColumn = new CustomNumericColumn();

                if (!string.IsNullOrEmpty(dispalyName))
                    customColumn.Header = dispalyName;
                else
                    customColumn.Header = e.Property.Name;
                customColumn.IsReadOnly = true;
                customColumn.Binding = new Binding(e.Property.Name);
                customColumn.Format = "0,0.##";
                if (this.FlowDirection == FlowDirection.RightToLeft)
                    customColumn.HorizontalAlignment = HorizontalAlignment.Left;
                e.Column = customColumn;
            }

            if (e.Property.PropertyType == typeof(bool) || e.Property.PropertyType == typeof(bool?))
            {
                var customColumn = new CustomCheckBoxColumn();

                if (!string.IsNullOrEmpty(dispalyName))
                    customColumn.Header = dispalyName;
                else
                    customColumn.Header = e.Property.Name;
                customColumn.IsReadOnly = true;
                customColumn.Binding = new Binding(e.Property.Name);
                e.Column = customColumn;
            }

            if (e.Property.PropertyType == typeof(ImageSource))
            {
                var customColumn = new DataGridImageColumn();
                customColumn.IsReadOnly = true;

                if (!string.IsNullOrEmpty(dispalyName))
                    customColumn.Header = dispalyName;
                else
                    customColumn.Header = e.Property.Name;
                customColumn.Binding = new Binding(e.Property.Name);
                e.Column = customColumn;
            }

            if (e.Property.PropertyType == typeof(DateTime) || e.Property.PropertyType == typeof(DateTime?))
            {
                var customColumn = new CustomDateTimeColumn();

                if (!string.IsNullOrEmpty(dispalyName))
                    customColumn.Header = dispalyName;
                else
                    customColumn.Header = e.Property.Name;
                customColumn.IsReadOnly = true;
                customColumn.Binding = new Binding(e.Property.Name);
                customColumn.Format = "d";
                if (this.FlowDirection == FlowDirection.RightToLeft)
                    customColumn.HorizontalAlignment = HorizontalAlignment.Left;
                e.Column = customColumn;
            }
        }

        private string GetDisplayName(string filterField)
        {
            if (ItemsSource != null && !string.IsNullOrWhiteSpace(filterField))
            {
                var type = ItemsSource.GetType();
                if (type.IsGenericType)
                {
                    var propInfo = type.GenericTypeArguments[0].GetProperty(filterField);
                    if (propInfo != null)
                    {
                        var attrib = propInfo.GetCustomAttributes(typeof(DisplayAttribute), false);
                        if (attrib != null && attrib.Count() > 0)
                            return ((DisplayAttribute) attrib[0]).Name;
                    }
                }
            }

            return string.Empty;
        }

        private void CustomGrid_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (loadedFlag)
                return;

            this.BottomRows.Add(new CustomBottomRow() { Height = new DataGridLength(32) });

            foreach (var item in ButtonColumns)
                this.Columns.Add(item);
            OrderButtonColumns();

            this.FrozenBottomRowsCount = 1;
            this.CanUserAddRows = false;
            this.CanUserEditRows = true;
            this.CanUserGroup = true;
            this.CanUserRemoveRows = false;
            this.CanUserReorderColumns = true;
            this.CanUserResizeRows = false;
            this.CanUserSort = true;
            this.IsReadOnly = false;
            this.SelectionMode = DataGridSelectionMode.SingleRow;
            loadedFlag = true;
        }

        private void OrderButtonColumns()
        {
            if (DesignerProperties.GetIsInDesignMode(this))
                return;

            Dictionary<CustomButtonColumn, int> orders = new Dictionary<CustomButtonColumn, int>();
            foreach (var item in ButtonColumns)
            {
                if (item.Order.HasValue)
                    orders.Add(item, item.Order.Value);
            }

            var sortOrders = orders.OrderByDescending(x => x.Value);
            foreach (var item in sortOrders)
                item.Key.DisplayIndex = item.Value;
        }

        #endregion

        #region Properties

        public ObservableCollection<CustomButtonColumn> ButtonColumns { get; set; }

        public string ColumnsToAdd { get; set; }

        public string ColumnsToRemove { get; set; }

        public event EventHandler ItemsSourceChanged;

        public int ItemsSourceCount
        {
            get
            {
                if (ItemsSource == null)
                    return 0;

                int counter = 0;
                foreach (var item in ItemsSource)
                    counter++;
                return counter;
            }
        }

        private bool loadedFlag = false;

        #endregion
    }
}
