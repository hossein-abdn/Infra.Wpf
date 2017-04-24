using System.Runtime.CompilerServices;
using C1.WPF.DataGrid;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using System.Windows.Data;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Reflection;
using Infra.Wpf.Common.Helpers;

namespace Infra.Wpf.Controls
{
    public partial class CustomMultiValueFilter : UserControl, IDataGridFilterUnity
    {
        #region Methods
        
        public CustomMultiValueFilter()
        {
            InitializeComponent();
            
            IsSetFilter = true;
            IsFilterItemChangedFire = true;               
            
            CustomConverter converter = new CustomConverter(
                (v, t, p, c) =>
                {
                    if (v != null)
                    {
                        if ((v as ObservableCollection<FilterItem>).All(x => x.IsChecked))
                            return true;
                        else if ((v as ObservableCollection<FilterItem>).All(x => x.IsChecked == false))
                            return false;
                        else
                            return null;
                    }
                    return null;
                }, 
                (v, t, p, c) =>
                {
                    if (v != null)
                    {
                        bool newValue = (bool)v;
                        foreach (var item in ItemsSource)
                        {
                            IsFilterItemChangedFire = false;
                            item.IsChecked = newValue;
                            IsFilterItemChangedFire = true;
                        }
                    }
                    
                    return ItemsSource;
                });
            
            this.SetBinding(CustomMultiValueFilter.SelectAllProperty, new Binding("ItemsSource")
            {
                Source = this,
                Converter = converter,
                Mode = BindingMode.TwoWay
            });
        }
        
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            FrameworkElementFactory checkBox = new FrameworkElementFactory(typeof(CheckBox));
            checkBox.SetValue(CheckBox.FlowDirectionProperty, FlowDirection.LeftToRight);
            checkBox.SetBinding(CheckBox.IsCheckedProperty, new Binding("IsChecked"));
            
            FrameworkElementFactory textBlock = new FrameworkElementFactory(typeof(TextBlock));
            textBlock.SetValue(TextBlock.MarginProperty, new Thickness(5, 0, 5, 0));
            textBlock.SetBinding(TextBlock.TextProperty, new Binding("Text"));
            if (Direction != null)
                textBlock.SetBinding(TextBlock.FlowDirectionProperty, new Binding("Direction") { Source = this });
            
            FrameworkElementFactory stackPanel = new FrameworkElementFactory(typeof(StackPanel));
            stackPanel.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);
            stackPanel.AppendChild(checkBox);
            stackPanel.AppendChild(textBlock);
            
            DataTemplate dTemplate = new DataTemplate { VisualTree = stackPanel };
            list.ItemTemplate = dTemplate;
        }
        
        private void CurrentGrid_FilterOpened(object sender, DataGridColumnValueEventArgs<IDataGridFilter> e)
        {
            if (e.Column.Name == DisplayMemberPath)
                GetFilter();
        }
        
        private void ItemsSource_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != null)
                foreach (var item in e.OldValue as ObservableCollection<FilterItem>)
                    item.FilterItemChanged -= new EventHandler(FilterItem_Changed);
            
            if (e.NewValue != null)
                foreach (var item in e.NewValue as ObservableCollection<FilterItem>)
                    item.FilterItemChanged += new EventHandler(FilterItem_Changed);
        }
        
        private void FilterItem_Changed(object sender, EventArgs e)
        {
            if (ItemsSource != null && IsFilterItemChangedFire == true)
            {
                if (ItemsSource.All(x => x.IsChecked) && SelectAll != true)
                    SelectAll = true;
                else if (ItemsSource.All(x => x.IsChecked == false) && SelectAll != false)
                    SelectAll = false;
                else if (SelectAll != null)
                    SelectAll = null;
                
                if (IsSetFilter == true)
                    Filter = SetFilter();
            }
        }
        
        private DataGridFilterState SetFilter()
        {
            DataGridFilterInfo filterInfo = new DataGridFilterInfo();                        
            filterInfo.FilterType = DataGridFilterType.MultiValue;
            filterInfo.FilterCombination = DataGridFilterCombination.And;
            
            if (ItemsSource.All(t => t.IsChecked))
                filterInfo.FilterOperation = DataGridFilterOperation.All;
            else
            {
                filterInfo.FilterOperation = DataGridFilterOperation.IsOneOf;
                filterInfo.Value = ItemsSource.Where(x => x.IsChecked).Select(x => x.Value).ToList();
            }
            
            List<DataGridFilterInfo> filterInfoList = new List<DataGridFilterInfo>();
            if (Filter != null && Filter.FilterInfo != null)
            {
                foreach (var item in Filter.FilterInfo)
                {
                    if (item.FilterType != DataGridFilterType.MultiValue)
                        filterInfoList.Add(item);                   
                }
            }
            
            filterInfoList.Add(filterInfo);
            
            return new DataGridFilterState { FilterInfo = filterInfoList };
        }
        
        private void GetFilter()
        {
            DataGridFilterInfo filterInfo = null;
            if (Filter != null && Filter.FilterInfo != null)
            {
                foreach (var item in Filter.FilterInfo)
                {
                    if (item.FilterType == DataGridFilterType.MultiValue)
                    {
                        filterInfo = item;
                        break;
                    }
                }
            }
            
            if (filterInfo != null && filterInfo.Value != null)
            {
                SelectAll = false;
                
                var filterList = filterInfo.Value as IEnumerable<object>;
                
                foreach (var item in ItemsSource)
                {
                    if (filterList.Contains(item.Value))
                    {
                        IsSetFilter = false;
                        item.IsChecked = true;
                        IsSetFilter = true;
                    }
                }
            }
            else
                SelectAll = true;
            
            var currentRows = GetCurrentRows();
            if (currentRows != null)
            {
                foreach (var item in ItemsSource)
                {
                    if (currentRows.Contains(item.Value))
                    {
                        IsSetFilter = false;
                        item.IsChecked = true;
                        IsSetFilter = true;
                    }
                    else
                    {
                        IsSetFilter = false;
                        item.IsChecked = false;
                        IsSetFilter = true;
                    }
                }
            }
        }
        
        private List<object> GetCurrentRows()
        {
            if (rows == null)
                return null;
            
            if (rows.Count == 0)
                return new List<object>();
            
            var itemRows = rows.Where(x => x.Type == DataGridRowType.Item).ToList();
            
            Type valueType = itemRows.First().DataItem.GetType();
            PropertyInfo MemberPathInfo = valueType.GetProperty(DisplayMemberPath);
            
            if (MemberPathInfo != null)
            {
                List<object> listItem = new List<object>();
                foreach (C1.WPF.DataGrid.DataGridRow item in itemRows)
                    listItem.Add(MemberPathInfo.GetValue(item.DataItem));
                
                return listItem.Distinct().ToList();
            }
            
            return new List<object>();
        }
        
        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            FilterItem_Changed(sender, e);
        }
        
        private void OnPropertyChanged([CallerMemberName] string prop = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        
        #endregion
        
        #region Properties
        
        public event PropertyChangedEventHandler PropertyChanged;
        
        public ObservableCollection<FilterItem> ItemsSource
        {
            get
            {
                return (ObservableCollection<FilterItem>)GetValue(ItemsSourceProperty);
            }
            set
            {
                SetValue(ItemsSourceProperty, value);
            }
        }
        
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(ObservableCollection<FilterItem>),
                typeof(CustomMultiValueFilter), new PropertyMetadata(null,
                    (d, a) => { ((CustomMultiValueFilter)d).ItemsSource_Changed(d, a); }));
        
        public bool? SelectAll
        {
            get
            {
                return (bool?)GetValue(SelectAllProperty);
            }
            set
            {
                SetValue(SelectAllProperty, value);
            }
        }
        
        public static readonly DependencyProperty SelectAllProperty =
            DependencyProperty.Register("SelectAll", typeof(bool?), typeof(CustomMultiValueFilter),
                new PropertyMetadata(false));
        
        private DataGridFilterState _Filter;
        
        public DataGridFilterState Filter
        {
            get
            {
                return _Filter;
            }
            set
            {
                if (_Filter != value)
                {
                    _Filter = value;
                    OnPropertyChanged();
                }
            }
        }
        
        public string DisplayMemberPath { get; set; }
        
        public bool IsSetFilter { get; set; }
        
        public bool IsFilterItemChangedFire { get; set; }
        
        private FlowDirection? _Direction;
        
        public FlowDirection? Direction 
        {
            get
            {
                return _Direction;
            }
            set
            {
                if (_Direction != value)
                {
                    _Direction = value;
                    OnPropertyChanged();
                }
            }
        }
        
        private DataGridRowCollection rows 
        { 
            get
            {
                if (CurrentGrid != null)
                    return CurrentGrid.Rows;
                return null;
            }
        }
        
        private C1DataGrid _CurrentGrid;
        
        public C1DataGrid CurrentGrid
        { 
            get
            {
                return _CurrentGrid;
            }
            set 
            {
                _CurrentGrid = value;
                _CurrentGrid.FilterOpened += CurrentGrid_FilterOpened;
            }
        }
    
        #endregion
    }
}