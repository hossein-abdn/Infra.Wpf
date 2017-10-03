using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Controls;
using Infra.Wpf.Mvvm;
using System.Windows.Controls.Primitives;

namespace Infra.Wpf.Controls
{
    [ContentProperty("Items")]
    public class MultiSelect : Control
    {
        #region Properties

        public bool IsOpen
        {
            get { return (bool) GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register("IsOpen", typeof(bool), typeof(MultiSelect), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public double MaxDropDownHeight
        {
            get { return (double) GetValue(MaxDropDownHeightProperty); }
            set { SetValue(MaxDropDownHeightProperty, value); }
        }

        public static readonly DependencyProperty MaxDropDownHeightProperty =
            DependencyProperty.Register("MaxDropDownHeight", typeof(double), typeof(MultiSelect), new PropertyMetadata(200d));

        public Visibility SearchVisible
        {
            get { return (Visibility) GetValue(SearchVisibleProperty); }
            set { SetValue(SearchVisibleProperty, value); }
        }

        public static readonly DependencyProperty SearchVisibleProperty =
            DependencyProperty.Register("SearchVisible", typeof(Visibility), typeof(MultiSelect), new PropertyMetadata(Visibility.Visible));

        public string SearchText
        {
            get { return (string) GetValue(SearchTextProperty); }
            set { SetValue(SearchTextProperty, value); }
        }

        public static readonly DependencyProperty SearchTextProperty =
            DependencyProperty.Register("SearchText", typeof(string), typeof(MultiSelect), new PropertyMetadata(null, OnSearchTextChanged));

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable) GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(MultiSelect), new PropertyMetadata(null, OnItemsSourceChanged));

        public string DisplayMemberPath
        {
            get { return (string) GetValue(DisplayMemberPathProperty); }
            set { SetValue(DisplayMemberPathProperty, value); }
        }

        public static readonly DependencyProperty DisplayMemberPathProperty =
            DependencyProperty.Register("DisplayMemberPath", typeof(string), typeof(MultiSelect), new PropertyMetadata(null));

        public ObservableCollection<object> SelectedItems
        {
            get { return (ObservableCollection<object>) GetValue(SelectedItemsProperty); }
            set { SetValue(SelectedItemsProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.Register("SelectedItems", typeof(ObservableCollection<object>), typeof(MultiSelect),
                new PropertyMetadata(null, OnSelectedItemsChanged, OnSelectedItemsCoerce));

        public ObservableCollection<int> SelectedIndices
        {
            get { return (ObservableCollection<int>) GetValue(SelectedIndicesProperty); }
            set { SetValue(SelectedIndicesProperty, value); }
        }

        public static readonly DependencyProperty SelectedIndicesProperty =
            DependencyProperty.Register("SelectedIndices", typeof(ObservableCollection<int>), typeof(MultiSelect),
                new PropertyMetadata(null, OnSelectedIndicesChanged, OnSelectedIndicesCoerce));

        public static readonly RoutedEvent SelectionChangedEvent =
            EventManager.RegisterRoutedEvent("SelectionChanged", RoutingStrategy.Bubble, typeof(SelectionChangedEventHandler), typeof(MultiSelect));

        public event SelectionChangedEventHandler SelectionChanged
        {
            add
            {
                AddHandler(SelectionChangedEvent, value);
            }
            remove
            {
                RemoveHandler(SelectionChangedEvent, value);
            }
        }

        public event EventHandler DropDownOpened;

        public event EventHandler DropDownClosed;

        private readonly ObservableCollection<object> items;
        public ObservableCollection<object> Items
        {
            get { return items; }
        }

        private StackPanel itemPresenter { get; set; }

        private WrapPanel contentPresenter { get; set; }

        private TextBox searchBox { get; set; }

        private Popup popup { get; set; }

        private ScrollViewer scroll { get; set; }

        private UIElementCollection itemContainers { get; set; }

        private List<string> FilterContentList { get; set; }

        private ControlTemplate selectedItemTemplate { get; set; }

        public RelayCommand<MultiSelectItem> CloseCommand { get; set; }

        private enum ChangeSourceEnum
        {
            None,
            FromSelectedItems,
            FromSelectedIndices
        }

        private ChangeSourceEnum ChangeSource { get; set; }

        bool scrollFlag { get; set; }

        #endregion

        #region Methods

        static MultiSelect()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MultiSelect), new FrameworkPropertyMetadata(typeof(MultiSelect)));
        }

        public MultiSelect()
        {
            ChangeSource = ChangeSourceEnum.None;
            items = new ObservableCollection<object>();
            FilterContentList = new List<string>();
            Loaded += MultiSelect_Loaded;
        }

        public override void OnApplyTemplate()
        {
            itemPresenter = Template.FindName("itemPresenter", this) as StackPanel;
            contentPresenter = Template.FindName("contentPresenter", this) as WrapPanel;
            selectedItemTemplate = (ControlTemplate) TryFindResource(new ComponentResourceKey(typeof(MultiSelect), "SelectedItemTemplate"));
            itemContainers = itemPresenter.Children;
            popup = (Popup) Template.FindName("popup", this);
            searchBox = (TextBox) Template.FindName("searchBox", this);
            ToggleButton toggleButton = (ToggleButton) Template.FindName("toggleButton", this);
            toggleButton.Click += (sender, e) => searchBox.Focus();
            toggleButton.KeyDown += (sender, e) =>
            {
                if (e.Key == Key.Enter || e.Key == Key.Space)
                    popup.IsOpen = true;
            };

            scroll = (ScrollViewer) Template.FindName("scroll", this);

            base.OnApplyTemplate();
        }

        private void MultiSelect_Loaded(object sender, RoutedEventArgs e)
        {
            Items.CollectionChanged += OnItemsChanged;
            searchBox.PreviewKeyDown += SearchBox_PreviewKeyDown;
            popup.KeyDown += Popup_KeyDown;
            popup.Opened += Popup_Opened;
            popup.Closed += Popup_Closed;
            SelectedItems = new ObservableCollection<object>();
            SelectedIndices = new ObservableCollection<int>();
            CloseCommand = new RelayCommand<MultiSelectItem>(CloseCommandExecute);
            foreach (var item in Items)
                OnItemsChanged(Items, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, itemContainers.Count));
        }

        private void Popup_Closed(object sender, EventArgs e)
        {
            DropDownClosed?.Invoke(sender, new EventArgs());
        }

        private void Popup_Opened(object sender, EventArgs e)
        {
            SearchText = string.Empty;
            scrollFlag = false;
            DropDownOpened?.Invoke(sender, new EventArgs());
        }

        private void Popup_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                popup.IsOpen = false;
                return;
            }
            if (e.Key != Key.Down && e.Key != Key.Up && e.Key != Key.Enter && e.Key != Key.Space)
                searchBox.Focus();
        }

        private void SearchBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down || e.Key == Key.Up)
            {
                if (Items != null && Items.Count > 0)
                    itemContainers[0].Focus();
            }
        }

        private static void OnSearchTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((MultiSelect) d).Filter();
        }

        private void Filter()
        {
            if (Items == null || Items.Count == 0)
                return;

            string filter = SearchText.ToLower();

            for (int i = 0; i < Items.Count; i++)
            {
                if (FilterContentList[i].Contains(filter))
                    itemContainers[i].Visibility = Visibility.Visible;
                else
                    itemContainers[i].Visibility = Visibility.Collapsed;
            }
        }

        private void OnItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (e.NewItems[0].GetType() == typeof(MultiSelectItem))
                    itemContainers.Insert(e.NewStartingIndex, GenerateContainer(Items.IndexOf(e.NewItems[0]), true));
                else
                    itemContainers.Insert(e.NewStartingIndex, GenerateContainer(Items.IndexOf(e.NewItems[0]), false));

                FilterContentList.Add(GetItemContent(e.NewItems[0]));
            }
            else if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                itemContainers.Clear();
                FilterContentList.Clear();
                SelectedItems.Clear();
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                itemContainers.RemoveAt(e.OldStartingIndex);
                if (e.OldStartingIndex < itemContainers.Count)
                {
                    for (int i = e.OldStartingIndex; i < itemContainers.Count; i++)
                    {
                        if (Items[i].GetType() == typeof(MultiSelectItem))
                        {
                            Binding bind = SetElementBinding(i, true);
                            ((MultiSelectItem) itemContainers[i]).SetBinding(ContentControl.ContentProperty, bind);
                        }
                        else
                        {
                            Binding bind = SetElementBinding(i, false);
                            ((MultiSelectItem) itemContainers[i]).SetBinding(ContentControl.ContentProperty, bind);
                        }
                    }
                }

                FilterContentList.RemoveAt(e.OldStartingIndex);
                SelectedItems.Remove(e.OldItems[0]);
            }
            else if (e.Action == NotifyCollectionChangedAction.Move)
            {
                string moveItem = FilterContentList[e.OldStartingIndex];
                FilterContentList.RemoveAt(e.OldStartingIndex);
                FilterContentList.Insert(e.NewStartingIndex, moveItem);
            }
            else if (e.Action == NotifyCollectionChangedAction.Replace)
                FilterContentList[e.OldStartingIndex] = GetItemContent(e.NewItems[0]);
        }

        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var multi = (MultiSelect) d;

            if (e.NewValue == null)
                multi.Items.Clear();
            else
            {
                if (e.NewValue is INotifyCollectionChanged)
                    ((INotifyCollectionChanged) e.NewValue).CollectionChanged += multi.OnItemsSource_CollectionChanged;

                multi.Items.Clear();
                foreach (var item in (IEnumerable) e.NewValue)
                    multi.Items.Add(item);
            }
        }

        private void OnItemsSource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
                Items.Add(e.NewItems[0]);
            else if (e.Action == NotifyCollectionChangedAction.Reset)
                Items.Clear();
            else if (e.Action == NotifyCollectionChangedAction.Remove)
                Items.RemoveAt(e.OldStartingIndex);
            else if (e.Action == NotifyCollectionChangedAction.Move)
                Items.Move(e.OldStartingIndex, e.NewStartingIndex);
            else if (e.Action == NotifyCollectionChangedAction.Replace)
                Items[e.NewStartingIndex] = e.NewItems[0];
        }

        private static object OnSelectedItemsCoerce(DependencyObject d, object baseValue)
        {
            var multiSelect = (MultiSelect) d;
            var selectedList = (IList) baseValue;

            if (selectedList == null)
                selectedList = new ObservableCollection<object>();

            for (int i = 0; i < selectedList.Count; i++)
            {
                if (!multiSelect.Items.Contains(selectedList[i]))
                {
                    selectedList.RemoveAt(i);
                    i--;
                }
            }

            return selectedList;
        }

        private static void OnSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var multiSelect = (MultiSelect) d;
            multiSelect.contentPresenter.Children.Clear();
            multiSelect.ChangeSource = ChangeSourceEnum.FromSelectedItems;
            multiSelect.SelectedIndices?.Clear();

            multiSelect.SelectedItems.CollectionChanged += multiSelect.OnSelectedItems_CollectionChanged;


            foreach (var item in (IList) e.NewValue)
            {
                int index = multiSelect.Items.IndexOf(item);
                multiSelect.contentPresenter.Children.Add(multiSelect.GeneratePanelItem(item));
                ((MultiSelectItem) multiSelect.itemContainers[index]).IsSelected = true;
                multiSelect.ChangeSource = ChangeSourceEnum.FromSelectedItems;
                multiSelect.SelectedIndices.Add(index);
            }

            multiSelect.ChangeSource = ChangeSourceEnum.None;
        }

        private void OnSelectedItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add && ChangeSource != ChangeSourceEnum.FromSelectedIndices)
            {
                int index = Items.IndexOf(e.NewItems[0]);
                if (index == -1)
                {
                    SelectedItems.Remove(e.NewItems[0]);
                    return;
                }

                ((MultiSelectItem) itemContainers[index]).IsSelected = true;
                contentPresenter.Children.Add(GeneratePanelItem(e.NewItems[0]));
                ChangeSource = ChangeSourceEnum.FromSelectedItems;
                SelectedIndices.Add(index);
                RaiseEvent(new System.Windows.Controls.SelectionChangedEventArgs(SelectionChangedEvent, new object[0], e.NewItems));
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove && ChangeSource != ChangeSourceEnum.FromSelectedIndices)
            {
                int index = Items.IndexOf(e.OldItems[0]);
                if (index == -1)
                    return;

                ((MultiSelectItem) itemContainers[index]).IsSelected = false;
                contentPresenter.Children.RemoveAt(e.OldStartingIndex);
                ChangeSource = ChangeSourceEnum.FromSelectedItems;
                SelectedIndices.Remove(index);
                RaiseEvent(new System.Windows.Controls.SelectionChangedEventArgs(SelectionChangedEvent, e.OldItems, new object[0]));
            }
            else if (e.Action == NotifyCollectionChangedAction.Reset && ChangeSource != ChangeSourceEnum.FromSelectedIndices)
            {
                foreach (var item in itemContainers)
                    ((MultiSelectItem) item).IsSelected = false;
                contentPresenter.Children.Clear();
                ChangeSource = ChangeSourceEnum.FromSelectedItems;
                SelectedIndices.Clear();
                RaiseEvent(new System.Windows.Controls.SelectionChangedEventArgs(SelectionChangedEvent, new object[0], new object[0]));
            }

            ChangeSource = ChangeSourceEnum.None;
        }

        private static object OnSelectedIndicesCoerce(DependencyObject d, object baseValue)
        {
            var multiSelect = (MultiSelect) d;
            var selectedList = (IList) baseValue;

            if (selectedList == null)
                selectedList = new ObservableCollection<int>();

            for (int i = 0; i < selectedList.Count; i++)
            {
                if ((int) selectedList[i] < 0 || (int) selectedList[i] >= multiSelect.Items.Count)
                {
                    selectedList.RemoveAt(i);
                    i--;
                }
            }

            return selectedList;
        }

        private static void OnSelectedIndicesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var multiSelect = (MultiSelect) d;
            multiSelect.contentPresenter.Children.Clear();
            multiSelect.ChangeSource = ChangeSourceEnum.FromSelectedIndices;
            multiSelect.SelectedItems?.Clear();

            multiSelect.SelectedIndices.CollectionChanged += multiSelect.OnSelectedIndices_CollectionChanged;

            foreach (int index in (IList) e.NewValue)
            {
                object item = multiSelect.Items[index];
                multiSelect.contentPresenter.Children.Add(multiSelect.GeneratePanelItem(item));
                ((MultiSelectItem) multiSelect.itemContainers[index]).IsSelected = true;
                multiSelect.ChangeSource = ChangeSourceEnum.FromSelectedIndices;
                multiSelect.SelectedItems.Add(item);
            }

            multiSelect.ChangeSource = ChangeSourceEnum.None;
        }

        private void OnSelectedIndices_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add && ChangeSource != ChangeSourceEnum.FromSelectedItems)
            {
                var index = (int) e.NewItems[0];
                if (index >= Items.Count || index < 0)
                {
                    SelectedIndices.Remove(index);
                    return;
                }

                ((MultiSelectItem) itemContainers[index]).IsSelected = true;
                contentPresenter.Children.Add(GeneratePanelItem(Items[index]));
                ChangeSource = ChangeSourceEnum.FromSelectedIndices;
                SelectedItems.Add(Items[index]);
                RaiseEvent(new System.Windows.Controls.SelectionChangedEventArgs(SelectionChangedEvent, new object[0], new object[1] { Items[index] }));
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove && ChangeSource != ChangeSourceEnum.FromSelectedItems)
            {
                int index = (int) e.OldItems[0];
                if (index >= Items.Count || index < 0)
                    return;

                ((MultiSelectItem) itemContainers[index]).IsSelected = false;
                contentPresenter.Children.RemoveAt(e.OldStartingIndex);
                ChangeSource = ChangeSourceEnum.FromSelectedIndices;
                SelectedItems.RemoveAt(index);
                RaiseEvent(new System.Windows.Controls.SelectionChangedEventArgs(SelectionChangedEvent, new object[1] { Items[index] }, new object[0]));
            }
            else if (e.Action == NotifyCollectionChangedAction.Reset && ChangeSource != ChangeSourceEnum.FromSelectedItems)
            {
                foreach (var item in itemContainers)
                    ((MultiSelectItem) item).IsSelected = false;
                contentPresenter.Children.Clear();
                ChangeSource = ChangeSourceEnum.FromSelectedIndices;
                SelectedItems.Clear();
                RaiseEvent(new System.Windows.Controls.SelectionChangedEventArgs(SelectionChangedEvent, new object[0], new object[0]));
            }

            ChangeSource = ChangeSourceEnum.None;
        }

        private ContentControl GeneratePanelItem(object item)
        {
            MultiSelectItem panelItem = new MultiSelectItem();
            panelItem.Template = selectedItemTemplate;
            panelItem.Content = GetItemContent(item);
            panelItem.Item = item;

            return panelItem;
        }

        private MultiSelectItem GenerateContainer(int elementIndex, bool isMultiSelectItem)
        {
            MultiSelectItem newElement = new MultiSelectItem();

            var bind = SetElementBinding(elementIndex, isMultiSelectItem);
            newElement.SetBinding(ContentControl.ContentProperty, bind);
            newElement.Item = Items[elementIndex];

            newElement.MouseDown += (sender, e) =>
            {
                var item = ((MultiSelectItem) sender).Item;
                if (((MultiSelectItem) sender).IsSelected)
                    SelectedItems.Remove(item);
                else
                    SelectedItems.Add(item);
            };

            newElement.MouseEnter += (sender, e) =>
            {
                var index = itemContainers.IndexOf(newElement);
                if (index < scroll.VerticalOffset + scroll.ViewportHeight)
                    newElement.Focus();
                else
                {
                    if (scrollFlag == false)
                    {
                        scroll.ScrollToVerticalOffset(scroll.VerticalOffset + 1);
                        newElement.Focus();
                        scrollFlag = true;
                    }
                    else
                        scrollFlag = false;
                }
            };

            newElement.KeyDown += (sender, e) =>
            {
                if (e.Key == Key.Enter || e.Key == Key.Space)
                {
                    var item = ((MultiSelectItem) sender).Item;
                    if (((MultiSelectItem) sender).IsSelected)
                        SelectedItems.Remove(item);
                    else
                        SelectedItems.Add(item);
                }
            };

            return newElement;
        }

        private Binding SetElementBinding(int elementIndex, bool isMultiSelectItem)
        {
            Binding bind = null;

            if (isMultiSelectItem)
            {
                string memberpath = string.Empty;

                if (!string.IsNullOrEmpty(DisplayMemberPath))
                {
                    memberpath = ((MultiSelectItem) Items[elementIndex]).Content.GetType().GetProperty(DisplayMemberPath)?.Name;
                    if (string.IsNullOrEmpty(memberpath) == false)
                        memberpath = "." + memberpath;
                }

                bind = new Binding("Items[" + elementIndex.ToString() + "].Content" + memberpath);
                bind.Source = this;
            }
            else
            {
                string memberpath = string.Empty;

                if (!string.IsNullOrEmpty(DisplayMemberPath))
                {
                    memberpath = Items[elementIndex].GetType().GetProperty(DisplayMemberPath)?.Name;
                    if (string.IsNullOrEmpty(memberpath) == false)
                        memberpath = "." + memberpath;
                }

                bind = new Binding("Items[" + elementIndex.ToString() + "]" + memberpath);
                bind.Source = this;
            }

            return bind;
        }

        private string GetItemContent(object item)
        {
            if (item == null)
                return null;

            if (!string.IsNullOrEmpty(DisplayMemberPath))
            {
                if (item.GetType() == typeof(MultiSelectItem))
                {
                    if (((MultiSelectItem) item).Content == null)
                        return null;

                    var displayMember = ((MultiSelectItem) item).Content.GetType().GetProperty(DisplayMemberPath);
                    if (displayMember != null)
                        return displayMember.GetValue(((MultiSelectItem) item).Content).ToString().ToLower();
                    else
                        return ((MultiSelectItem) item).Content.ToString().ToLower();
                }
                else
                {
                    var displayMember = item.GetType().GetProperty(DisplayMemberPath);
                    if (displayMember != null)
                        return displayMember.GetValue(item).ToString().ToLower();
                    else
                        return item.ToString().ToLower();
                }
            }
            else
            {
                if (item.GetType() == typeof(MultiSelectItem))
                    return ((MultiSelectItem) item).Content.ToString().ToLower();
                else
                    return item.ToString().ToLower();
            }
        }

        private void CloseCommandExecute(MultiSelectItem obj)
        {
            SelectedItems.Remove(obj.Item);
        }

        #endregion
    }
}
