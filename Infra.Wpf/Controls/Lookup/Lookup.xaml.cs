using Infra.Wpf.Common.Helpers;
using Infra.Wpf.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Infra.Wpf.Controls
{
    public partial class Lookup : UserControl, INotifyPropertyChanged
    {
        #region Properties

        public delegate void SelectionChangedDelegate(object sender, SelectionChangedEventArgs e);

        public delegate void PreviewSelectDelegate(object sender, PreviewSelectEventArgs e);

        public event SelectionChangedDelegate SelectionChanged;

        public event PreviewSelectDelegate PreviewSelect;

        private LookupWindow lookupWindow = null;

        public List<int> SelectedIndices { get; set; }

        public List<CheckBoxViewModel> MultiSelectVM { get; set; }

        public string Title { get; set; }

        public string IdColumn { get; set; }

        public string CodeColumn { get; set; }

        public string ValueColumn { get; set; }

        public object SelectedItem
        {
            get
            {
                if (SelectedItems != null)
                    return (SelectedItems.OfType<object>()).FirstOrDefault();

                return null;
            }
        }

        public bool IsSelectedItem
        {
            get { return SelectedItem != null; }
        }

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

        public string Columns { get; set; }

        public List<IField> SearchFields { get; set; }

        public RelayCommand<object> SelectCommand { get; set; }

        public RelayCommand OpenWindowCommand { get; set; }

        public RelayCommand ClearCommand { get; set; }

        public IEnumerable SelectedItems
        {
            get { return (IEnumerable) GetValue(SelectedItemsProperty); }
            set { SetValue(SelectedItemsProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemsProperty = DependencyProperty.Register("SelectedItems", typeof(IEnumerable), typeof(Lookup),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedItemsChanged));

        public object Source
        {
            get { return (object) GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(object), typeof(Lookup), new PropertyMetadata(null));

        public ICommand SearchCommand
        {
            get { return (ICommand) GetValue(SearchCommandProperty); }
            set { SetValue(SearchCommandProperty, value); }
        }

        public static readonly DependencyProperty SearchCommandProperty =
            DependencyProperty.Register("SearchCommand", typeof(ICommand), typeof(Lookup), new PropertyMetadata(null));

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Methods

        public Lookup()
        {
            OpenWindowCommand = new RelayCommand(OpenWindowExcecute, CanOpenWindowExecute);
            ClearCommand = new RelayCommand(Clear);
            SelectCommand = new RelayCommand<object>(SelectExecute);

            SelectedIndices = new List<int>();
            SearchFields = new List<IField>();

            InitializeComponent();
        }

        private void lookup_Loaded(object sender, RoutedEventArgs e)
        {
            if (Source != null)
            {
                var bindExpression = GetBindingExpression(SearchCommandProperty);
                if (bindExpression != null && bindExpression.ParentBinding != null)
                {
                    Binding newBind = bindExpression.ParentBinding.Clone();
                    newBind.Source = this.Source;
                    SetBinding(SearchCommandProperty, newBind);
                }
            }

            if (string.IsNullOrEmpty(CodeColumn))
                CodeColumn = IdColumn;

            if (IsFocused == true)
                this.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
        }

        public void Clear()
        {
            ClearLookup();
        }

        private static void OnSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var @this = (Lookup) d;

            @this.SelectionChanged?.Invoke(@this, new SelectionChangedEventArgs { Items = (IEnumerable) e.NewValue });

            if (e.NewValue != null)
            {
                @this.Code = string.Empty;
                @this.Value = string.Empty;
                @this.Details = string.Empty;

                foreach (var item in e.NewValue as IEnumerable)
                {
                    var value = item.GetType().GetProperty(@this.ValueColumn)?.GetValue(item)?.ToString();
                    if (!string.IsNullOrEmpty(value))
                        @this.Value = @this.Value + value + ",";

                    var code = item.GetType().GetProperty(@this.CodeColumn)?.GetValue(item)?.ToString();
                    if (!string.IsNullOrEmpty(code))
                        @this.Code = @this.Code + code + ",";

                    if (!string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(code))
                        @this.Details = @this.Details + $"{code}-{value} \n";
                }

                if (!string.IsNullOrEmpty(@this.Code))
                    @this.Code = @this.Code.Substring(0, @this.Code.Length - 1);

                if (!string.IsNullOrEmpty(@this.Value))
                    @this.Value = @this.Value.Substring(0, @this.Value.Length - 1);

                if (!string.IsNullOrEmpty(@this.Details))
                    @this.Details = @this.Details.Substring(0, @this.Details.Length - 2);
            }
            else
                @this.ClearLookup();

            @this.OnPropertyChanged("SelectedItem");
            @this.OnPropertyChanged("IsSelectedItem");
        }

        public void OnPropertyChanged([CallerMemberName]string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private void SelectExecute(object obj)
        {
            var list = new List<object> { obj };
            SelectedItems = list;
            lookupWindow?.Close();
        }

        private bool CanOpenWindowExecute()
        {
            if (string.IsNullOrEmpty(ValueColumn) || string.IsNullOrEmpty(IdColumn) || SearchCommand == null)
                return false;

            return true;
        }

        private void OpenWindowExcecute()
        {
            if (PreviewSelect != null)
            {
                PreviewSelectEventArgs arg = new PreviewSelectEventArgs();
                PreviewSelect(this, arg);

                if (arg.Cancel == true)
                {
                    ClearLookup();
                    return;
                }
            }

            lookupWindow = new LookupWindow();
            lookupWindow.FlowDirection = FlowDirection;
            lookupWindow.title.Text = Title;
            lookupWindow.searchpanel.SearchCommand = SearchCommand;
            lookupWindow.searchpanel.RowMargin = 5;
            lookupWindow.searchpanel.ColumnMargin = 10;
            lookupWindow.DataContext = Source;
            lookupWindow.SelectionMode = SelectionMode;
            lookupWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            foreach (var item in SearchFields)
                lookupWindow.searchpanel.SearchFields.Add(item);
            lookupWindow.datagrid.ColumnsToAdd = Columns;

            if (SelectionMode == LookupSelectionMode.Single)
            {
                CustomButtonColumn buttonCoulumn = new CustomButtonColumn();
                buttonCoulumn.Image = new BitmapImage(new Uri("/Infra.Wpf;component/Controls/Resources/Select-24.png", UriKind.RelativeOrAbsolute));
                buttonCoulumn.MouseOverImage = new BitmapImage(new Uri("/Infra.Wpf;component/Controls/Resources/SelectOver-24.png", UriKind.RelativeOrAbsolute));
                buttonCoulumn.Width = new C1.WPF.DataGrid.DataGridLength(32);
                buttonCoulumn.Order = 0;
                buttonCoulumn.Command = SelectCommand;
                lookupWindow.datagrid.ButtonColumns.Add(buttonCoulumn);
                lookupWindow.ShowDialog();
            }
            else
            {
                SearchCommand?.Execute(null);
                IEnumerable items = (IEnumerable) Source?.GetType()?.GetProperty("ItemsSource")?.GetValue(Source);
                MultiSelectVM = new List<CheckBoxViewModel>();

                if (items != null)
                {
                    foreach (var item in items)
                        MultiSelectVM.Add(new CheckBoxViewModel { Item = item, Selected = false, Key = IdColumn });
                }

                foreach (var item in SelectedIndices)
                {
                    if (item >= 0 && item < MultiSelectVM.Count)
                        MultiSelectVM[item].Selected = true;
                }

                CustomCheckBoxColumn checkColumn = new CustomCheckBoxColumn();
                checkColumn.IsSelectable = true;
                checkColumn.Binding = new Binding("Selected") { Source = MultiSelectVM };
                checkColumn.Width = new C1.WPF.DataGrid.DataGridLength(32);
                lookupWindow.datagrid.Columns.Add(checkColumn);
                if (lookupWindow.ShowDialog() == true)
                {
                    SelectedIndices.Clear();
                    var list = new List<object>();

                    foreach (var item in MultiSelectVM)
                    {
                        if (item.Selected)
                        {
                            SelectedIndices.Add(MultiSelectVM.IndexOf(item));
                            list.Add(item.Item);
                        }
                    }

                    if (list.Count > 0)
                        SelectedItems = list;
                    else
                        ClearLookup();
                }
            }
        }

        private void CustomTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F2 && OpenWindowCommand.CanExecute(null))
                OpenWindowCommand.Execute(null);
            else if (e.Key == Key.Enter)
            {
                if (string.IsNullOrEmpty(ValueColumn) || string.IsNullOrEmpty(IdColumn) || SearchCommand == null)
                    return;

                if (PreviewSelect != null)
                {
                    PreviewSelectEventArgs arg = new PreviewSelectEventArgs();
                    PreviewSelect(this, arg);

                    if (arg.Cancel == true)
                    {
                        ClearLookup();
                        return;
                    }
                }

                if (SelectionMode == LookupSelectionMode.Single)
                {
                    SearchCommand.Execute($@"{CodeColumn}=={(sender as CustomTextBox).Text}");
                    var items = (IEnumerable) Source?.GetType().GetProperty("ItemsSource")?.GetValue(Source);

                    if (items.Count() > 0)
                    {
                        foreach (var item in items)
                        {
                            SelectedItems = new List<object> { item };
                            break;
                        }
                    }
                    else
                        ClearLookup();
                }
                else
                {
                    SearchCommand.Execute(null);
                    var items = (IEnumerable) Source?.GetType().GetProperty("ItemsSource")?.GetValue(Source);

                    var strCode = (sender as CustomTextBox).Text.Split(',');
                    for (int i = 0; i < strCode.Length; i++)
                        strCode[i] = strCode[i].Trim();

                    strCode = strCode.Distinct().ToArray();

                    int counter = 0;
                    List<object> list = new List<object>();
                    SelectedIndices.Clear();

                    foreach (var item in items)
                    {
                        var code = item?.GetType().GetProperty(CodeColumn)?.GetValue(item)?.ToString();
                        if (!string.IsNullOrEmpty(code))
                        {
                            if (strCode.Any(x => x == code))
                            {
                                list.Add(item);
                                SelectedIndices.Add(counter);
                            }
                        }
                        counter++;
                    }

                    if (list.Any())
                        SelectedItems = list;
                    else
                        ClearLookup();
                }
            }
        }

        private void ClearLookup()
        {
            SelectedItems = null;
            Code = string.Empty;
            Value = string.Empty;
            Details = string.Empty;
            SelectedIndices.Clear();
        }

        #endregion
    }
}
