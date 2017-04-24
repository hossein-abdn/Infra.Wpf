using Infra.Wpf.Common.Helpers;
using Infra.Wpf.Mvvm;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.ComponentModel;

namespace Infra.Wpf.Controls
{
    [ContentProperty("SearchFields")]
    public partial class SearchPanel : UserControl
    {
        #region Property

        public SearchFieldCollection SearchFields { get; set; }

        public string SearchPhrase
        {
            get
            {
                string result = "";
                foreach (var item in SearchFields)
                {
                    if (string.IsNullOrWhiteSpace(item.SearchPhrase) == false)
                    {
                        if (string.IsNullOrWhiteSpace(result) == false)
                            result = result + " AND ";
                        result = result + item.SearchPhrase;
                    }
                }

                return result;
            }
        }

        public RelayCommand ClearCommand { get; set; }

        public ICommand SearchCommand
        {
            get { return (ICommand) GetValue(SearchCommandProperty); }
            set { SetValue(SearchCommandProperty, value); }
        }


        public static readonly DependencyProperty SearchCommandProperty =
            DependencyProperty.Register("SearchCommand", typeof(ICommand), typeof(SearchPanel), new PropertyMetadata(null));

        public bool Stretch
        {
            get { return (bool) GetValue(StretchProperty); }
            set { SetValue(StretchProperty, value); }
        }

        public static readonly DependencyProperty StretchProperty = SearchGridWrapPanel.StretchProperty.AddOwner(typeof(SearchPanel));

        public double ColumnMargin
        {
            get { return (double) GetValue(ColumnMarginProperty); }
            set { SetValue(ColumnMarginProperty, value); }
        }

        public static readonly DependencyProperty ColumnMarginProperty = SearchGridWrapPanel.ColumnMarginProperty.AddOwner(typeof(SearchPanel));

        public double RowMargin
        {
            get { return (double) GetValue(RowMarginProperty); }
            set { SetValue(RowMarginProperty, value); }
        }

        public static readonly DependencyProperty RowMarginProperty = SearchGridWrapPanel.RowMarginProperty.AddOwner(typeof(SearchPanel));

        public bool IsExpanded
        {
            get { return (bool) GetValue(IsExpandedProperty); }
            set { SetValue(IsExpandedProperty, value); }
        }

        public static readonly DependencyProperty IsExpandedProperty = Expander.IsExpandedProperty.AddOwner(typeof(SearchPanel));

        #endregion

        #region Method

        public SearchPanel()
        {
            SearchFields = new SearchFieldCollection();
            ClearCommand = new RelayCommand(ClearExecute);

            InitializeComponent();
        }

        private string GetDisplayName(string filterField)
        {
            if (DataContext != null && !string.IsNullOrWhiteSpace(filterField))
            {
                var type = DataContext.GetType().GetProperty("ItemsSource").PropertyType;

                if (type.IsGenericType)
                {
                    var propInfo = type.GenericTypeArguments[0].GetProperty(filterField);
                    if (propInfo != null)
                    {
                        var attrib = propInfo.GetCustomAttributes(typeof(DisplayNameAttribute), false);
                        if (attrib != null && attrib.Count() > 0)
                            return ((DisplayNameAttribute) attrib[0]).DisplayName;
                    }
                }
            }

            return string.Empty;
        }
        private void mainpanel_Loaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < SearchFields.Count; i++)
            {
                var item = SearchFields[i];

                TextBlock displayText = new TextBlock();

                string displayAttribText = GetDisplayName(SearchFields[i].FilterField);
                if (!string.IsNullOrWhiteSpace(item.DisplayName))
                    displayText.Text = item.DisplayName;
                else if (!string.IsNullOrEmpty(displayAttribText))
                    displayText.Text = displayAttribText;
                else
                    displayText.Text = item.FilterField;

                displayText.HorizontalAlignment = HorizontalAlignment.Right;
                displayText.VerticalAlignment = VerticalAlignment.Center;
                displayText.Margin = new Thickness(0, 0, 5, 0);

                if (item is DateSearchField && (item as DateSearchField).DateFormat == DateFormat.Range)
                {
                    string appendText1 = " از";
                    string appendText2 = " تا";

                    if (!string.IsNullOrWhiteSpace(item.DisplayName))
                    {
                        if (item.DisplayName.EndsWith(" :"))
                        {
                            appendText1 = " از :";
                            appendText2 = " تا :";
                            item.DisplayName = item.DisplayName.Remove(item.DisplayName.Length - 2, 2);
                        }
                        else if (item.DisplayName.EndsWith(":"))
                        {
                            appendText1 = " از:";
                            appendText2 = " تا:";
                            item.DisplayName = item.DisplayName.Remove(item.DisplayName.Length - 1, 1);
                        }

                        displayText.Text = item.DisplayName + appendText1;
                    }
                    (item as DateSearchField).Operator = NumericOperator.GreaterThanEqual;

                    DateSearchField item2 = new DateSearchField();
                    item2.DateFormat = DateFormat.DateTime;
                    item2.OpertatorVisible = false;
                    item2.FilterField = (item as DateSearchField).FilterField;
                    if (!string.IsNullOrWhiteSpace(item.DisplayName))
                        item2.DisplayName = item.DisplayName + appendText2;
                    else
                        item2.DisplayName = item2.FilterField;
                    item2.Operator = NumericOperator.LessThanEqual;
                    SearchFields.Insert(i + 1, item2);
                }

                searchpanel.Children.Add(displayText);
                searchpanel.Children.Add((Control) item);
            }
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            new System.Threading.Thread(new System.Threading.ThreadStart(() =>
            {
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (SearchCommand != null)
                    {
                        SearchCommand.Execute(SearchPhrase);
                    }
                }));
            })).Start();
        }

        private void ClearExecute()
        {
            foreach (var item in SearchFields)
                item.Clear();
        }

        private static void RowMarginChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var @this = ((SearchPanel) d);
            var margin = @this.Submit.Margin;
            @this.Submit.Margin = new Thickness(margin.Left, margin.Top + (double) e.NewValue, margin.Right, margin.Bottom);

            margin = @this.Clear.Margin;
            @this.Clear.Margin = new Thickness(margin.Left, margin.Top + (double) e.NewValue, margin.Right, margin.Bottom);
        }
        private void mainpanel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Submit_Click(this, new RoutedEventArgs());
        }

        #endregion
    }
}
