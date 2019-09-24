using Infra.Wpf.Common;
using Infra.Wpf.Common.Helpers;
using Infra.Wpf.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Infra.Wpf.Controls
{
    public class GridLookup : Lookup
    {
        #region Properties

        public delegate void SelectionChangedDelegate(object sender, SelectionChangedEventArgs e);

        public delegate void PreviewSelectDelegate(object sender, PreviewSelectEventArgs e);

        public event SelectionChangedDelegate SelectionChanged;

        public event PreviewSelectDelegate PreviewSelect;

        private LookupWindow lookupWindow = null;

        public string WindowTitle { get; set; }

        public string IdColumn { get; set; }

        private PropertyInfo idColumnPropInfo;

        public string CodeColumn { get; set; }

        private PropertyInfo codeColumnPropInfo;

        public string ValueColumn { get; set; }

        private PropertyInfo valueColumnPropInfo;

        private bool _IsDropDown;
        public bool IsDropDown
        {
            get { return _IsDropDown; }
            set
            {
                _IsDropDown = value;
                OnPropertyChanged();
            }
        }

        private LookupSelectionMode _SelectionMode;
        public LookupSelectionMode SelectionMode
        {
            get { return _SelectionMode; }
            set
            {
                _SelectionMode = value;
                OnPropertyChanged();
            }
        }

        private string _Value;
        public string Value
        {
            get { return _Value; }
            private set
            {
                _Value = value;
                OnPropertyChanged();
            }
        }

        private string _Code;
        public string Code
        {
            get { return _Code; }
            set
            {
                _Code = value;
                OnPropertyChanged();
            }
        }

        private string _Details;
        public string Details
        {
            get { return _Details; }
            set
            {
                _Details = value;
                OnPropertyChanged();
            }
        }

        public FieldCollection SearchFields { get; set; }

        public RelayCommand ClearCommand { get; set; }

        public RelayCommand<object> SelectCommand { get; set; }

        public RelayCommand OpenWindowCommand { get; set; }

        public bool IsItemSelected
        {
            get
            {
                if (SelectionMode == LookupSelectionMode.Single)
                    return SelectedItem != null;
                else
                    return SelectedItems != null && SelectedItems.Count() > 0;
            }
        }

        private ChangeSourceEnum ChangeSource { get; set; }

        public string Columns { get; set; }

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(GridLookup), new PropertyMetadata(null));

        public ICommand SearchCommand
        {
            get { return (ICommand)GetValue(SearchCommandProperty); }
            set { SetValue(SearchCommandProperty, value); }
        }

        public static readonly DependencyProperty SearchCommandProperty =
            DependencyProperty.Register("SearchCommand", typeof(ICommand), typeof(GridLookup), new PropertyMetadata(null));

        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(GridLookup),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedItemChanged, OnSelectedItemCoerce));

        public IEnumerable SelectedItems
        {
            get { return (IEnumerable)GetValue(SelectedItemsProperty); }
            set { SetValue(SelectedItemsProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.Register("SelectedItems", typeof(IEnumerable), typeof(GridLookup),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedItemsChanged, OnSelectedItemsCoerce));

        public int? SelectedIndex
        {
            get { return (int?)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register("SelectedIndex", typeof(int?), typeof(GridLookup),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedIndexChanged, OnSelectedIndexCoerce));

        public IEnumerable<int> SelectedIndices
        {
            get { return (IEnumerable<int>)GetValue(SelectedIndicesProperty); }
            set { SetValue(SelectedIndicesProperty, value); }
        }

        public static readonly DependencyProperty SelectedIndicesProperty = DependencyProperty.Register("SelectedIndices", typeof(IEnumerable<int>), typeof(GridLookup),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedIndicesChanged, OnSelectedIndicesCoerce));

        public int? SelectedId
        {
            get { return (int?)GetValue(SelectedIdProperty); }
            set { SetValue(SelectedIdProperty, value); }
        }

        public static readonly DependencyProperty SelectedIdProperty = DependencyProperty.Register("SelectedId", typeof(int?), typeof(GridLookup),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedIdChanged, OnSelectedIdCoerce));

        public IEnumerable<int> SelectedIds
        {
            get { return (IEnumerable<int>)GetValue(SelectedIdsProperty); }
            set { SetValue(SelectedIdsProperty, value); }
        }

        public static readonly DependencyProperty SelectedIdsProperty = DependencyProperty.Register("SelectedIds", typeof(IEnumerable<int>), typeof(GridLookup),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedIdsChanged, OnSelectedIdsCoerce));

        public bool IsGetFocus
        {
            get { return (bool)GetValue(IsGetFocusProperty); }
            set { SetValue(IsGetFocusProperty, value); }
        }

        public static readonly DependencyProperty IsGetFocusProperty =
            DependencyProperty.Register("IsGetFocus", typeof(bool), typeof(GridLookup), new PropertyMetadata(false, OnIsGetFocusChanged));

        private IEnumerable actualSource { get; set; }

        private int? tmpSelectedIndex;

        private int? tmpSelectedId;

        private object tmpSelectedItem;

        private IEnumerable<int> tmpSelectedIndices;

        private IEnumerable<int> tmpSelectedIds;

        private IEnumerable tmpSelectedItems;

        #endregion

        #region Methods

        public GridLookup()
        {
            OpenWindowCommand = new RelayCommand(OpenWindowExcecute, CanOpenWindowExecute);
            SelectCommand = new RelayCommand<object>(SelectExecute);
            ClearCommand = new RelayCommand(ClearLookup);
            SearchFields = new FieldCollection();

            Loaded += GridLookup_Loaded;
        }

        private void GridLookup_Loaded(object sender, RoutedEventArgs e)
        {
            Binding binding = new Binding("IsFocused")
            {
                Source = this,
                Mode = BindingMode.OneWay
            };

            SetBinding(IsGetFocusProperty, binding);

            if (SelectionMode == LookupSelectionMode.Single)
            {
                SelectedIndex = tmpSelectedIndex;
                SelectedId = tmpSelectedId;
                SelectedItem = tmpSelectedItem;
            }
            else
            {
                SelectedIndices = tmpSelectedIndices;
                SelectedIds = tmpSelectedIds;
                SelectedItems = tmpSelectedItems;
            }
        }

        private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var @this = (GridLookup)d;

            if (@this.SelectionMode == LookupSelectionMode.Multi)
                return;

            if (e.NewValue != null)
            {
                @this.SelectionChanged?.Invoke(@this, new SelectionChangedEventArgs { Items = new List<object> { e.NewValue } });

                @this.Code = string.Empty;
                @this.Value = string.Empty;
                @this.Details = string.Empty;

                @this.Value = @this.valueColumnPropInfo?.GetValue(e.NewValue)?.ToString();
                if (@this.Value == null)
                    @this.Value = string.Empty;

                @this.Code = @this.codeColumnPropInfo?.GetValue(e.NewValue)?.ToString();
                if (@this.Code == null)
                    @this.Code = string.Empty;

                if (!string.IsNullOrEmpty(@this.Code))
                    @this.Details = @this.Details + $"{@this.Code}-{@this.Value}";

                if (@this.ChangeSource == ChangeSourceEnum.None)
                {
                    @this.ChangeSource = ChangeSourceEnum.FromSelectedItem;
                    @this.SelectedIndex = @this.GetIndex(e.NewValue);
                    @this.SelectedId = @this.idColumnPropInfo?.GetValue(e.NewValue) as int?;
                    @this.ChangeSource = ChangeSourceEnum.None;
                }
            }
            else
            {
                @this.ClearLookup();
                @this.SelectionChanged?.Invoke(@this, new SelectionChangedEventArgs { Items = null });
            }

            @this.OnPropertyChanged("IsItemSelected");
        }

        private static object OnSelectedItemCoerce(DependencyObject d, object baseValue)
        {
            if (baseValue != null)
            {
                var @this = (GridLookup)d;
                if (@this.ChangeSource != ChangeSourceEnum.None)
                    return baseValue;

                if (@this.IsLoaded == false)
                    @this.tmpSelectedItem = baseValue;

                @this.LoadData();

                if (@this.valueColumnPropInfo == null || @this.idColumnPropInfo == null)
                {
                    @this.ClearLookup();
                    return null;
                }

                if (@this.ItemsSource != null)
                {
                    foreach (var item in @this.ItemsSource)
                    {
                        if (item.Equals(baseValue))
                            return baseValue;
                    }
                }
            }

            return null;
        }

        private static object OnSelectedItemsCoerce(DependencyObject d, object baseValue)
        {
            if (baseValue != null && (baseValue as IEnumerable).Count() > 0)
            {
                var @this = (GridLookup)d;
                if (@this.ChangeSource != ChangeSourceEnum.None)
                    return baseValue;

                if (@this.IsLoaded == false)
                    @this.tmpSelectedItems = (IEnumerable)baseValue;

                @this.LoadData();

                if (@this.valueColumnPropInfo == null || @this.idColumnPropInfo == null)
                {
                    @this.ClearLookup();
                    return null;
                }

                if (@this.ItemsSource != null)
                {
                    foreach (var item in baseValue as IEnumerable)
                    {
                        bool isExist = false;
                        foreach (var sourceItem in @this.ItemsSource)
                        {
                            if (item.Equals(sourceItem) == true)
                            {
                                isExist = true;
                                break;
                            }
                        }
                        if (isExist == false)
                            return null;
                    }
                    return baseValue;
                }
            }

            return null;
        }

        private static void OnSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var @this = (GridLookup)d;

            if (@this.SelectionMode == LookupSelectionMode.Single)
                return;

            @this.SelectionChanged?.Invoke(@this, new SelectionChangedEventArgs { Items = e.NewValue as IEnumerable });

            if (e.NewValue != null)
            {
                @this.Code = string.Empty;
                @this.Value = string.Empty;
                @this.Details = string.Empty;

                var indexList = new List<int>();
                var idList = new List<int>();

                foreach (var item in e.NewValue as IEnumerable)
                {
                    string value = null;
                    value = @this.valueColumnPropInfo?.GetValue(item)?.ToString();
                    if (!string.IsNullOrEmpty(value))
                        @this.Value = @this.Value + value + ",";

                    string code = null;
                    if (!string.IsNullOrEmpty(@this.CodeColumn))
                        code = @this.codeColumnPropInfo?.GetValue(item)?.ToString();
                    if (!string.IsNullOrEmpty(code))
                        @this.Code = @this.Code + code + ",";

                    if (!string.IsNullOrEmpty(code))
                        @this.Details = @this.Details + $"{code}-{value}\n";

                    if (@this.ChangeSource == ChangeSourceEnum.None)
                    {
                        var index = @this.GetIndex(item);
                        if (index != null)
                            indexList.Add(index.Value);
                        var id = @this.idColumnPropInfo?.GetValue(item) as int?;
                        if (id != null)
                            idList.Add(id.Value);
                    }
                }

                if (!string.IsNullOrEmpty(@this.Code))
                    @this.Code = @this.Code.Substring(0, @this.Code.Length - 1);

                if (!string.IsNullOrEmpty(@this.Value))
                    @this.Value = @this.Value.Substring(0, @this.Value.Length - 1);

                if (!string.IsNullOrEmpty(@this.Details))
                    @this.Details = @this.Details.Substring(0, @this.Details.Length - 1);

                if (@this.ChangeSource == ChangeSourceEnum.None)
                {
                    @this.ChangeSource = ChangeSourceEnum.FromSelectedItem;
                    @this.SelectedIndices = indexList;
                    @this.SelectedIds = idList;
                    @this.ChangeSource = ChangeSourceEnum.None;
                }
            }
            else
                @this.ClearLookup();

            @this.OnPropertyChanged("IsItemSelected");
        }

        private static object OnSelectedIndexCoerce(DependencyObject d, object baseValue)
        {
            if (baseValue != null)
            {
                var @this = (GridLookup)d;
                if (@this.ChangeSource != ChangeSourceEnum.None)
                    return baseValue;

                if (@this.IsLoaded == false)
                    @this.tmpSelectedIndex = (int?)baseValue;

                @this.LoadData();

                if (@this.valueColumnPropInfo == null || @this.idColumnPropInfo == null)
                {
                    @this.ClearLookup();
                    return null;
                }

                if (@this.ItemsSource != null)
                {
                    if (@this.ItemsSource.Count() <= (int)baseValue)
                        return null;

                    return baseValue;
                }
            }

            return null;
        }

        private static void OnSelectedIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var @this = (GridLookup)d;

            if (@this.SelectionMode == LookupSelectionMode.Multi)
                return;

            if (@this.ChangeSource == ChangeSourceEnum.None)
            {
                @this.ChangeSource = ChangeSourceEnum.FromSelectedIndex;
                if (e.NewValue != null)
                {
                    int counter = -1;
                    int index = (int)e.NewValue;
                    foreach (var item in @this.ItemsSource)
                    {
                        counter++;
                        if (counter == index)
                        {
                            @this.SelectedId = @this.idColumnPropInfo?.GetValue(item) as int?;
                            @this.SelectedItem = item;
                            break;
                        }
                    }
                }
                else
                {
                    @this.SelectedId = null;
                    @this.SelectedItem = null;
                }
                @this.ChangeSource = ChangeSourceEnum.None;
            }
        }

        private static object OnSelectedIndicesCoerce(DependencyObject d, object baseValue)
        {
            if (baseValue != null)
            {
                var @this = (GridLookup)d;
                if (@this.ChangeSource != ChangeSourceEnum.None)
                    return baseValue;

                if (@this.IsLoaded == false)
                    @this.tmpSelectedIndices = (IEnumerable<int>)baseValue;

                @this.LoadData();

                if (@this.valueColumnPropInfo == null || @this.idColumnPropInfo == null)
                {
                    @this.ClearLookup();
                    return null;
                }

                var count = @this.ItemsSource?.Count();
                if (count != null)
                {
                    foreach (var item in baseValue as IEnumerable<int>)
                    {
                        if (item >= count.Value)
                            return null;
                    }

                    return baseValue;
                }
            }

            return null;
        }

        private static void OnSelectedIndicesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var @this = (GridLookup)d;

            if (@this.SelectionMode == LookupSelectionMode.Single)
                return;

            if (@this.ChangeSource == ChangeSourceEnum.None)
            {
                @this.ChangeSource = ChangeSourceEnum.FromSelectedIndex;
                if (e.NewValue != null)
                {
                    var itemList = new List<object>();
                    var idList = new List<int>();
                    var indexList = e.NewValue as IEnumerable<int>;
                    var max = indexList.Max();
                    int counter = -1;

                    foreach (var item in @this.ItemsSource)
                    {
                        counter++;
                        if (counter > max)
                            break;

                        foreach (var indexItem in indexList)
                        {
                            if (counter == indexItem)
                            {
                                itemList.Add(item);
                                var id = @this.idColumnPropInfo?.GetValue(item) as int?;
                                if (id != null)
                                    idList.Add(id.Value);
                                break;
                            }
                        }
                    }

                    @this.SelectedIds = idList;
                    @this.SelectedItems = itemList;
                }
                else
                {
                    @this.SelectedIds = null;
                    @this.SelectedItems = null;
                }
                @this.ChangeSource = ChangeSourceEnum.None;
            }
        }

        private static object OnSelectedIdCoerce(DependencyObject d, object baseValue)
        {
            if (baseValue != null)
            {
                var @this = (GridLookup)d;
                int value = (int)baseValue;
                if (@this.ChangeSource != ChangeSourceEnum.None)
                    return baseValue;

                if (@this.IsLoaded == false)
                    @this.tmpSelectedId = (int?)baseValue;

                @this.LoadData();

                if (@this.valueColumnPropInfo == null || @this.idColumnPropInfo == null)
                {
                    @this.ClearLookup();
                    return null;
                }

                if (@this.ItemsSource != null)
                {
                    foreach (var item in @this.ItemsSource)
                    {
                        var id = @this.idColumnPropInfo.GetValue(item) as int?;
                        if (id != null && id.Value == value)
                            return baseValue;
                    }
                }
            }

            return null;
        }

        private static void OnSelectedIdChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var @this = (GridLookup)d;

            if (@this.SelectionMode == LookupSelectionMode.Multi)
                return;

            if (@this.ChangeSource == ChangeSourceEnum.None)
            {
                @this.ChangeSource = ChangeSourceEnum.FromSelectedId;
                if (e.NewValue != null)
                {
                    int counter = -1;
                    foreach (var item in @this.ItemsSource)
                    {
                        counter++;
                        var id = @this.idColumnPropInfo?.GetValue(item) as int?;
                        if (id != null && id.Value == (int)e.NewValue)
                        {
                            @this.SelectedIndex = counter;
                            @this.SelectedItem = item;
                        }
                    }
                }
                else
                {
                    @this.SelectedIndex = null;
                    @this.SelectedItem = null;
                }
                @this.ChangeSource = ChangeSourceEnum.None;
            }
        }

        private static object OnSelectedIdsCoerce(DependencyObject d, object baseValue)
        {
            var values = baseValue as IEnumerable<int>;
            if (values != null && values.Count() > 0)
            {
                var @this = (GridLookup)d;
                if (@this.ChangeSource != ChangeSourceEnum.None)
                    return baseValue;

                if (@this.IsLoaded == false)
                    @this.tmpSelectedIds = (IEnumerable<int>)baseValue;

                @this.LoadData();

                if (@this.valueColumnPropInfo == null || @this.idColumnPropInfo == null)
                {
                    @this.ClearLookup();
                    return null;
                }

                if (@this.ItemsSource != null)
                {
                    foreach (var idItem in values)
                    {
                        bool isExist = false;
                        foreach (var item in @this.ItemsSource)
                        {
                            var id = @this.idColumnPropInfo.GetValue(item) as int?;
                            if (id == idItem)
                            {
                                isExist = true;
                                break;
                            }
                        }
                        if (isExist == false)
                            return null;
                    }
                }
                return baseValue;
            }

            return null;
        }

        private static void OnSelectedIdsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var @this = (GridLookup)d;

            if (@this.SelectionMode == LookupSelectionMode.Single)
                return;

            if (@this.ChangeSource == ChangeSourceEnum.None)
            {
                @this.ChangeSource = ChangeSourceEnum.FromSelectedId;
                if (e.NewValue != null)
                {
                    int counter = -1;
                    var indexList = new List<int>();
                    var itemList = new List<object>();

                    foreach (var item in @this.ItemsSource)
                    {
                        counter++;
                        foreach (var idItem in e.NewValue as IEnumerable<int>)
                        {
                            if (idItem == (int)@this.idColumnPropInfo.GetValue(item))
                            {
                                indexList.Add(counter);
                                itemList.Add(item);
                                break;
                            }
                        }
                    }

                    @this.SelectedItems = itemList;
                    @this.SelectedIndices = indexList;
                }
                else
                {
                    if (@this.idColumnPropInfo != null)
                    {
                        @this.SelectedIndices = null;
                        @this.SelectedItems = null;
                    }
                }
                @this.ChangeSource = ChangeSourceEnum.None;
            }
        }

        private static void OnIsGetFocusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (((bool)e.NewValue) == true)
                ((GridLookup)d).MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
        }

        private int? GetIndex(object obj)
        {
            int? index = -1;
            if (actualSource != null)
            {
                foreach (var item in actualSource)
                {
                    index++;
                    if (item.Equals(obj))
                        return index;
                }
            }

            return null;
        }

        private void LoadData()
        {
            SearchCommand?.Execute(null);
            actualSource = ItemsSource?.Where("true=true", null);

            if (ItemsSource != null)
            {
                foreach (var item in ItemsSource)
                {
                    SetPropertyInfo(item);
                    break;
                }
            }
        }

        private void SetPropertyInfo(object obj)
        {
            if (string.IsNullOrEmpty(CodeColumn))
                CodeColumn = IdColumn;

            Type objType = obj?.GetType();

            if (!string.IsNullOrEmpty(ValueColumn) && valueColumnPropInfo == null)
                valueColumnPropInfo = objType?.GetProperty(ValueColumn);

            if (!string.IsNullOrEmpty(IdColumn) && idColumnPropInfo == null)
                idColumnPropInfo = objType?.GetProperty(IdColumn);

            if (!string.IsNullOrEmpty(CodeColumn) && codeColumnPropInfo == null)
                codeColumnPropInfo = objType?.GetProperty(CodeColumn);
        }

        public void ClearLookup()
        {
            Code = string.Empty;
            Value = string.Empty;
            Details = string.Empty;
            SelectedItems = null;
            SelectedItem = null;
            SelectedIndices = null;
            SelectedIndex = null;
            SelectedId = null;
            SelectedIds = null;
        }

        private bool CanOpenWindowExecute()
        {
            if (string.IsNullOrEmpty(ValueColumn) || string.IsNullOrEmpty(IdColumn) || (SearchCommand == null && ItemsSource == null))
                return false;

            return true;
        }

        private void OpenWindowExcecute()
        {
            if (PreviewSelect != null)
            {
                var arg = new PreviewSelectEventArgs();
                PreviewSelect(this, arg);

                if (arg.Cancel == true)
                {
                    ClearLookup();
                    return;
                }
            }

            LoadData();

            if (valueColumnPropInfo == null || idColumnPropInfo == null)
            {
                ClearLookup();
                return;
            }

            PrepareLookupWindow();

            if (SelectionMode == LookupSelectionMode.Single)
                lookupWindow.ShowDialog();
            else
            {
                if (ItemsSource == null || ItemsSource.Count() == 0)
                {
                    ClearLookup();
                    return;
                }
                else
                {
                    foreach (var item in ItemsSource)
                    {
                        if (!(item is ISelectable))
                            return;
                        break;
                    }
                }

                if (SelectedIndices != null)
                {
                    foreach (var item in SelectedItems)
                        (item as ISelectable).IsSelected = true;
                }

                if (lookupWindow.ShowDialog() == true)
                {
                    var itemList = new List<object>();
                    var indexList = new List<int>();
                    var idList = new List<int>();
                    int counter = -1;
                    foreach (var item in ItemsSource)
                    {
                        counter++;
                        if ((item as ISelectable).IsSelected)
                        {
                            indexList.Add(counter);
                            itemList.Add(item);
                            var id = idColumnPropInfo?.GetValue(item) as int?;
                            if (id != null)
                                idList.Add(id.Value);
                        }
                    }

                    if (itemList.Count > 0)
                    {
                        ChangeSource = ChangeSourceEnum.FromComponent;
                        SelectedItems = itemList;
                        SelectedIndices = indexList;
                        SelectedIds = idList;
                        ChangeSource = ChangeSourceEnum.None;
                    }
                    else
                        ClearLookup();
                }
            }
        }

        private void PrepareLookupWindow()
        {
            lookupWindow = new LookupWindow();
            lookupWindow.maingrid.FlowDirection = FlowDirection;
            lookupWindow.title.Text = WindowTitle;
            lookupWindow.searchpanel.SearchCommand = SearchCommand;
            lookupWindow.searchpanel.RowMargin = 5;
            lookupWindow.searchpanel.ColumnMargin = 10;
            lookupWindow.LookupMode = LookupWindow.LookupModeEnum.Grid;

            var bind = new Binding("ItemsSource") { Source = this };
            BindingOperations.SetBinding(lookupWindow, DataContextProperty, bind);
            lookupWindow.SelectionMode = SelectionMode;
            lookupWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            var sourceType = ItemsSource?.GetType();
            if (sourceType != null)
            {
                if (sourceType.IsGenericType)
                    lookupWindow.searchpanel.ModelType = sourceType.GenericTypeArguments[0];
            }

            if (SearchFields != null)
            {
                foreach (var item in SearchFields)
                {
                    item.Clear();
                    lookupWindow.searchpanel.SearchFields.Add(item);
                }
            }

            lookupWindow.datagrid.ColumnsToAdd = Columns;

            if (SelectionMode == LookupSelectionMode.Single)
            {
                var buttonColumn = new CustomButtonColumn();
                buttonColumn.Image = new BitmapImage(new Uri("/Infra.Wpf;component/Controls/Resources/Select-24.png", UriKind.RelativeOrAbsolute));
                buttonColumn.MouseOverImage = new BitmapImage(new Uri("/Infra.Wpf;component/Controls/Resources/SelectOver-24.png", UriKind.RelativeOrAbsolute));
                buttonColumn.Width = new C1.WPF.DataGrid.DataGridLength(32);
                buttonColumn.Order = 0;
                buttonColumn.Command = SelectCommand;
                lookupWindow.datagrid.ButtonColumns.Add(buttonColumn);
            }
            else
            {
                var checkColumn = new CustomCheckBoxColumn();
                checkColumn.IsSelectable = true;
                checkColumn.Binding = new Binding("IsSelected");
                checkColumn.Width = new C1.WPF.DataGrid.DataGridLength(32);
                lookupWindow.datagrid.Columns.Add(checkColumn);
            }
        }

        private void SelectExecute(object obj)
        {
            ChangeSource = ChangeSourceEnum.FromComponent;
            SelectedItem = obj;
            SelectedIndex = GetIndex(obj);
            SelectedId = idColumnPropInfo?.GetValue(obj) as int?;
            ChangeSource = ChangeSourceEnum.None;

            lookupWindow?.Close();
        }

        public override void CustomTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F2 && OpenWindowCommand.CanExecute(null))
                OpenWindowCommand.Execute(null);
            else if (e.Key == Key.Enter)
            {
                if (string.IsNullOrEmpty(ValueColumn) || string.IsNullOrEmpty(IdColumn) || string.IsNullOrEmpty((sender as CustomTextBox).Text) || (SearchCommand == null && ItemsSource == null))
                {
                    ClearLookup();
                    return;
                }

                if (PreviewSelect != null)
                {
                    var arg = new PreviewSelectEventArgs();
                    PreviewSelect(this, arg);

                    if (arg.Cancel == true)
                    {
                        ClearLookup();
                        return;
                    }
                }

                LoadData();

                if (valueColumnPropInfo == null || idColumnPropInfo == null)
                {
                    ClearLookup();
                    return;
                }

                if (SelectionMode == LookupSelectionMode.Single)
                {
                    IEnumerable result = null;
                    if (SearchCommand == null)
                    {
                        try
                        {
                            result = ItemsSource.Where($@"{CodeColumn}=={(sender as CustomTextBox).Text}", null);
                        }
                        catch
                        {
                            ClearLookup();
                            return;
                        }
                    }
                    else
                    {
                        try
                        {
                            SearchCommand?.Execute(new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>(CodeColumn, $@"{CodeColumn}=={(sender as CustomTextBox).Text}") });
                            result = ItemsSource;
                        }
                        catch
                        {
                            ClearLookup();
                            return;
                        }
                    }

                    if (result != null && result.Count() > 0)
                    {
                        ChangeSource = ChangeSourceEnum.FromComponent;
                        foreach (var item in result)
                        {
                            SelectedItem = item;
                            SelectedIndex = GetIndex(item);
                            SelectedId = idColumnPropInfo?.GetValue(item) as int?;
                            break;
                        }
                        ChangeSource = ChangeSourceEnum.None;
                    }
                    else
                        ClearLookup();
                }
                else
                {
                    var strCode = (sender as CustomTextBox).Text.Split(',');
                    for (int i = 0; i < strCode.Length; i++)
                        strCode[i] = strCode[i].Trim();
                    strCode = strCode.Distinct().ToArray();

                    int counter = 0;
                    var itemList = new List<object>();
                    var indexList = new List<int>();
                    var idList = new List<int>();
                    if (ItemsSource != null)
                    {
                        foreach (var item in ItemsSource)
                        {
                            var code = codeColumnPropInfo?.GetValue(item)?.ToString();
                            if (!string.IsNullOrEmpty(code))
                            {
                                if (strCode.Any(x => x == code))
                                {
                                    itemList.Add(item);
                                    indexList.Add(counter);
                                    var id = idColumnPropInfo?.GetValue(item) as int?;
                                }
                            }
                            counter++;
                        }
                    }

                    if (itemList.Any())
                    {
                        ChangeSource = ChangeSourceEnum.FromComponent;
                        SelectedItems = itemList;
                        SelectedIndices = indexList;
                        SelectedIds = idList;
                        ChangeSource = ChangeSourceEnum.None;
                    }
                    else
                        ClearLookup();
                }
            }
        }

        #endregion
    }
}
