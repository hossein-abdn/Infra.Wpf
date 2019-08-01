using Infra.Wpf.Common;
using Infra.Wpf.Common.Helpers;
using Infra.Wpf.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Infra.Wpf.Controls
{
    public class TreeLookup : Lookup
    {
        #region Properties

        public delegate void SelectionChangedDelegate(object sender, SelectionChangedEventArgs e);

        public delegate void PreviewSelectDelegate(object sender, PreviewSelectEventArgs e);

        public event SelectionChangedDelegate SelectionChanged;

        public event PreviewSelectDelegate PreviewSelect;

        public HierarchicalDataCollection HierarchicalData { get; set; }

        public string WindowTitle { get; set; }

        public string IdColumn { get; set; }

        public string CodeColumn { get; set; }

        public string ValueColumn { get; set; }

        public bool FillFlatItemsSource { get; set; }

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

        public Type ResultType { get; set; }

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

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(TreeLookup), new PropertyMetadata(null, OnItemsSourceChanged));

        public IList FlatItemsSource
        {
            get { return (IList)GetValue(FlatItemsSourceProperty); }
            set { SetValue(FlatItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty FlatItemsSourceProperty =
            DependencyProperty.Register("FlatItemsSource", typeof(IList), typeof(TreeLookup), new PropertyMetadata(null));

        public ICommand SearchCommand
        {
            get { return (ICommand)GetValue(SearchCommandProperty); }
            set { SetValue(SearchCommandProperty, value); }
        }

        public static readonly DependencyProperty SearchCommandProperty =
            DependencyProperty.Register("SearchCommand", typeof(ICommand), typeof(TreeLookup), new PropertyMetadata(null));

        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(TreeLookup),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedItemChanged, OnSelectedItemCoerce));

        public IEnumerable SelectedItems
        {
            get { return (IEnumerable)GetValue(SelectedItemsProperty); }
            set { SetValue(SelectedItemsProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.Register("SelectedItems", typeof(IEnumerable), typeof(TreeLookup),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedItemsChanged, OnSelectedItemsCoerce));

        public int? SelectedId
        {
            get { return (int?)GetValue(SelectedIdProperty); }
            set { SetValue(SelectedIdProperty, value); }
        }

        public static readonly DependencyProperty SelectedIdProperty = DependencyProperty.Register("SelectedId", typeof(int?), typeof(TreeLookup),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedIdChanged, OnSelectedIdCoerce));

        public IEnumerable<int> SelectedIds
        {
            get { return (IEnumerable<int>)GetValue(SelectedIdsProperty); }
            set { SetValue(SelectedIdsProperty, value); }
        }

        public static readonly DependencyProperty SelectedIdsProperty = DependencyProperty.Register("SelectedIds", typeof(IEnumerable<int>), typeof(TreeLookup),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedIdsChanged, OnSelectedIdsCoerce));

        private ChangeSourceEnum ChangeSource { get; set; }

        private LookupWindow treeLookupWindow = null;

        private PropertyInfo idColumnPropInfo;

        private PropertyInfo codeColumnPropInfo;

        private PropertyInfo valueColumnPropInfo;

        private int? tmpSelectedId;

        private object tmpSelectedItem;

        private IEnumerable<int> tmpSelectedIds;

        private IEnumerable tmpSelectedItems;

        #endregion

        #region Methods

        public TreeLookup()
        {
            OpenWindowCommand = new RelayCommand(OpenWindowExcecute, CanOpenWindowExecute);
            SelectCommand = new RelayCommand<object>(SelectExecute);
            ClearCommand = new RelayCommand(ClearLookup);
            SearchFields = new FieldCollection();
            HierarchicalData = new HierarchicalDataCollection();

            Loaded += GridLookup_Loaded;
        }

        private void GridLookup_Loaded(object sender, RoutedEventArgs e)
        {
            Type itemsSourceType = typeof(List<>).MakeGenericType(ResultType);
            FlatItemsSource = (IList)Activator.CreateInstance(itemsSourceType);

            if (IsFocused == true)
                this.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));

            if (SelectionMode == LookupSelectionMode.Single)
            {
                SelectedId = tmpSelectedId;
                SelectedItem = tmpSelectedItem;
            }
            else
            {
                SelectedIds = tmpSelectedIds;
                SelectedItems = tmpSelectedItems;
            }

            if (ItemsSource != null && FlatItemsSource.Count == 0)
                OnItemsSourceChanged(this, new DependencyPropertyChangedEventArgs(ItemsSourceProperty, null, ItemsSource));
        }

        private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var @this = (TreeLookup)d;

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
                var @this = (TreeLookup)d;
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

                if (@this.FlatItemsSource != null)
                {
                    foreach (var item in @this.FlatItemsSource)
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
                var @this = (TreeLookup)d;
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

                if (@this.FlatItemsSource != null)
                {
                    foreach (var item in baseValue as IEnumerable)
                    {
                        bool isExist = false;
                        foreach (var sourceItem in @this.FlatItemsSource)
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
            var @this = (TreeLookup)d;

            if (@this.SelectionMode == LookupSelectionMode.Single)
                return;

            @this.SelectionChanged?.Invoke(@this, new SelectionChangedEventArgs { Items = e.NewValue as IEnumerable });

            if (e.NewValue != null)
            {
                @this.Code = string.Empty;
                @this.Value = string.Empty;
                @this.Details = string.Empty;

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
                    @this.SelectedIds = idList;
                    @this.ChangeSource = ChangeSourceEnum.None;
                }
            }
            else
                @this.ClearLookup();

            @this.OnPropertyChanged("IsItemSelected");
        }

        private static object OnSelectedIdCoerce(DependencyObject d, object baseValue)
        {
            if (baseValue != null)
            {
                var @this = (TreeLookup)d;
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

                if (@this.FlatItemsSource != null)
                {
                    foreach (var item in @this.FlatItemsSource)
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
            var @this = (TreeLookup)d;

            if (@this.SelectionMode == LookupSelectionMode.Multi)
                return;

            if (@this.ChangeSource == ChangeSourceEnum.None)
            {
                @this.ChangeSource = ChangeSourceEnum.FromSelectedId;
                if (e.NewValue != null)
                {
                    int counter = -1;
                    foreach (var item in @this.FlatItemsSource)
                    {
                        counter++;
                        var id = @this.idColumnPropInfo?.GetValue(item) as int?;
                        if (id != null && id.Value == (int)e.NewValue)
                            @this.SelectedItem = item;
                    }
                }
                else
                    @this.SelectedItem = null;

                @this.ChangeSource = ChangeSourceEnum.None;
            }
        }

        private static object OnSelectedIdsCoerce(DependencyObject d, object baseValue)
        {
            var values = baseValue as IEnumerable<int>;
            if (values != null && values.Count() > 0)
            {
                var @this = (TreeLookup)d;
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

                if (@this.FlatItemsSource != null)
                {
                    foreach (var idItem in values)
                    {
                        bool isExist = false;
                        foreach (var item in @this.FlatItemsSource)
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
            var @this = (TreeLookup)d;

            if (@this.SelectionMode == LookupSelectionMode.Single)
                return;

            if (@this.ChangeSource == ChangeSourceEnum.None)
            {
                @this.ChangeSource = ChangeSourceEnum.FromSelectedId;
                if (e.NewValue != null)
                {
                    var itemList = new List<object>();

                    foreach (var item in @this.FlatItemsSource)
                    {
                        foreach (var idItem in e.NewValue as IEnumerable<int>)
                        {
                            if (idItem == (int)@this.idColumnPropInfo.GetValue(item))
                            {
                                itemList.Add(item);
                                break;
                            }
                        }
                    }

                    @this.SelectedItems = itemList;
                }
                else
                {
                    if (@this.idColumnPropInfo != null)
                        @this.SelectedItems = null;
                }
                @this.ChangeSource = ChangeSourceEnum.None;
            }
        }

        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var @this = (TreeLookup)d;
            if (@this.ResultType == null)
                return;

            var type = e.NewValue.GetType();
            var items = (IEnumerable)e.NewValue;

            if (@this.FillFlatItemsSource)
            {
                @this.FlatItemsSource?.Clear();

                if (items.GetType().IsGenericType == false)
                {
                    object firstItem = null;
                    foreach (var item in items)
                    {
                        firstItem = item;
                        break;
                    }

                    if (@this.ResultType.IsAssignableFrom(firstItem.GetType()))
                    {
                        foreach (var item in items)
                            @this.FlatItemsSource.Add(item);
                    }

                    return;
                }

                if (@this.HierarchicalData == null || @this.HierarchicalData.Count == 0)
                    return;

                var hierarchicalDataTypes = @this.HierarchicalData.Select(x => x.DataType);
                foreach (var item in items)
                    ExtractFlatItemsSource(@this, item, hierarchicalDataTypes);
            }
        }

        private static void ExtractFlatItemsSource(TreeLookup @this, object item, IEnumerable<Type> types)
        {
            var itemType = item.GetType();
            foreach (var member in itemType.GetProperties())
            {
                var memberType = member.PropertyType;
                if (types.Any(x => x.IsAssignableFrom(memberType)) && member.GetValue(item) != null)
                    ExtractFlatItemsSource(@this, member.GetValue(item), types);

                if (memberType.IsGenericType && typeof(IEnumerable).IsAssignableFrom(memberType) && types.Any(x => x.IsAssignableFrom(memberType.GetGenericArguments()[0])) && member.GetValue(item) != null)
                {
                    foreach (var memberItem in member.GetValue(item) as IEnumerable)
                        ExtractFlatItemsSource(@this, memberItem, types);
                }

                if (@this.ResultType.IsAssignableFrom(memberType))
                    @this.FlatItemsSource.Add(member.GetValue(item));
            }
        }

        private void LoadData()
        {
            SearchCommand?.Execute(null);

            if (FlatItemsSource != null)
            {
                foreach (var item in FlatItemsSource)
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
                treeLookupWindow.ShowDialog();
            else
            {
                if (ItemsSource == null)
                {
                    ClearLookup();
                    return;
                }

                if (SelectedItems != null)
                {
                    foreach (var item in SelectedItems)
                    {
                        if (item is ISelectable)
                            (item as ISelectable).IsSelected = true;
                    }
                }

                if (treeLookupWindow.ShowDialog() == true)
                {
                    if (typeof(ISelectable).IsAssignableFrom(ResultType))
                    {
                        var itemList = new List<object>();
                        var idList = new List<int>();

                        var list = FlatItemsSource.Cast<ISelectable>().Where(x => x.IsSelected);
                        foreach (var item in list)
                        {
                            itemList.Add(item);
                            var id = idColumnPropInfo?.GetValue(item) as int?;
                            if (id != null)
                                idList.Add(id.Value);
                        }

                        if (itemList.Count > 0)
                        {
                            ChangeSource = ChangeSourceEnum.FromComponent;
                            SelectedItems = itemList;
                            SelectedIds = idList;
                            ChangeSource = ChangeSourceEnum.None;
                        }
                        else
                            ClearLookup();
                    }
                }
            }
        }

        private void PrepareLookupWindow()
        {
            treeLookupWindow = new LookupWindow();
            treeLookupWindow.maingrid.FlowDirection = FlowDirection;
            treeLookupWindow.title.Text = WindowTitle;
            treeLookupWindow.searchpanel.SearchCommand = SearchCommand;
            treeLookupWindow.searchpanel.RowMargin = 5;
            treeLookupWindow.searchpanel.ColumnMargin = 10;
            treeLookupWindow.LookupMode = LookupWindow.LookupModeEnum.Tree;

            treeLookupWindow.SelectionMode = SelectionMode;
            treeLookupWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            var sourceType = FlatItemsSource?.GetType();
            if (sourceType != null)
            {
                if (sourceType.IsGenericType)
                    treeLookupWindow.searchpanel.ModelType = sourceType.GenericTypeArguments[0];
            }

            if (SearchFields != null)
            {
                foreach (var item in SearchFields)
                {
                    item.Clear();
                    treeLookupWindow.searchpanel.SearchFields.Add(item);
                }
            }

            if (HierarchicalData != null)
            {
                ResourceDictionary resource = new ResourceDictionary();
                foreach (var item in HierarchicalData)
                    resource.Add(new DataTemplateKey(item.DataType), CreateHierarchicalDataTemplate(item));

                treeLookupWindow.treeview.Resources = resource;
            }

            var bind = new Binding("ItemsSource") { Source = this };
            BindingOperations.SetBinding(treeLookupWindow, DataContextProperty, bind);

            if (SelectionMode == LookupSelectionMode.Multi && typeof(ISelectable).IsAssignableFrom(ResultType))
            {
                foreach (var item in FlatItemsSource.Cast<ISelectable>())
                    item.IsSelected = false;
            }
        }

        private HierarchicalDataTemplate CreateHierarchicalDataTemplate(HierarchicalData item)
        {
            FrameworkElementFactory stackPanel = new FrameworkElementFactory(typeof(StackPanel));
            stackPanel.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);
            stackPanel.SetValue(StackPanel.BackgroundProperty, Brushes.Transparent);
            stackPanel.SetValue(StackPanel.MarginProperty, new Thickness(3));

            FrameworkElementFactory image = new FrameworkElementFactory(typeof(Image));
            image.SetValue(Image.WidthProperty, 12d);
            image.SetValue(Image.HeightProperty, 12d);
            image.SetValue(Image.SourceProperty, item.Image);
            stackPanel.AppendChild(image);

            if (SelectionMode == LookupSelectionMode.Single)
            {
                if (item.DataType.GetProperties().Any(x => ResultType.IsAssignableFrom(x.PropertyType)))
                {
                    FrameworkElementFactory imageButton = new FrameworkElementFactory(typeof(ImageButton));
                    imageButton.SetValue(ImageButton.ImageProperty, new BitmapImage(new Uri("pack://application:,,,/Infra.Wpf;component/Controls/Resources/Select-24.png")));
                    imageButton.SetValue(ImageButton.MouseOverImageProperty, new BitmapImage(new Uri("pack://application:,,,/Infra.Wpf;component/Controls/Resources/SelectOver-24.png")));
                    imageButton.SetValue(ImageButton.ImageSizeProperty, 12d);
                    imageButton.SetValue(ImageButton.BorderThicknessProperty, new Thickness(0));
                    imageButton.SetValue(ImageButton.BackgroundProperty, Brushes.Transparent);
                    imageButton.SetValue(ImageButton.MarginProperty, new Thickness(5, 0, 0, 0));
                    imageButton.SetBinding(ButtonCommand.CommandProperty, new Binding("SelectCommand") { Source = this });
                    imageButton.SetBinding(ButtonCommand.CommandParameterProperty, new Binding("."));
                    stackPanel.AppendChild(imageButton);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(item.SelectableField) == false && item.DataType.GetProperties().Any(x => typeof(ISelectable).IsAssignableFrom(x.PropertyType)))
                {
                    FrameworkElementFactory checkbox = new FrameworkElementFactory(typeof(CustomCheckBox));
                    checkbox.SetValue(TextBlock.MarginProperty, new Thickness(5, 0, 0, 0));
                    checkbox.SetBinding(CustomCheckBox.IsCheckedProperty, new Binding(item.SelectableField));
                    stackPanel.AppendChild(checkbox);
                }
            }

            FrameworkElementFactory textBlock = new FrameworkElementFactory(typeof(TextBlock));
            textBlock.SetValue(TextBlock.MarginProperty, new Thickness(5, 0, 5, 0));
            textBlock.SetBinding(TextBlock.TextProperty, new Binding(item.Title));
            stackPanel.AppendChild(textBlock);

            return new HierarchicalDataTemplate { VisualTree = stackPanel, DataType = item.DataType, ItemsSource = item.ItemsSource };
        }

        private void SelectExecute(object obj)
        {
            var resultTypePropInfo = obj?.GetType().GetProperties().FirstOrDefault(x => ResultType.IsAssignableFrom(x.PropertyType));
            if (resultTypePropInfo != null)
            {
                if (!string.IsNullOrEmpty(IdColumn) && idColumnPropInfo == null)
                    idColumnPropInfo = resultTypePropInfo.PropertyType?.GetProperty(IdColumn);

                var resultTypeValue = resultTypePropInfo.GetValue(obj);
                ChangeSource = ChangeSourceEnum.FromComponent;
                SelectedItem = resultTypeValue;
                SelectedId = idColumnPropInfo.GetValue(resultTypeValue) as int?;
                ChangeSource = ChangeSourceEnum.None;
            }

            treeLookupWindow?.Close();
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
                            result = FlatItemsSource.Where($@"{CodeColumn}=={(sender as CustomTextBox).Text}", null);
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
                            result = FlatItemsSource.Where($@"{CodeColumn}=={(sender as CustomTextBox).Text}", null);
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

                    var itemList = new List<object>();
                    var idList = new List<int>();
                    if (FlatItemsSource != null)
                    {
                        foreach (var item in FlatItemsSource)
                        {
                            var code = codeColumnPropInfo?.GetValue(item)?.ToString();
                            if (!string.IsNullOrEmpty(code))
                            {
                                if (strCode.Any(x => x == code))
                                {
                                    itemList.Add(item);
                                    var id = idColumnPropInfo?.GetValue(item) as int?;
                                }
                            }
                        }
                    }

                    if (itemList.Any())
                    {
                        ChangeSource = ChangeSourceEnum.FromComponent;
                        SelectedItems = itemList;
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
