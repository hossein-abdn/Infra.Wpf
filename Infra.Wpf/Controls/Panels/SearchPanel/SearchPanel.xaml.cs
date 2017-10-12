using Infra.Wpf.Common.Helpers;
using Infra.Wpf.Mvvm;
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

        public FieldCollection SearchFields { get; set; }

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

        public static readonly DependencyProperty StretchProperty = FieldGridWrapPanel.StretchProperty.AddOwner(typeof(SearchPanel));

        public double ColumnMargin
        {
            get { return (double) GetValue(ColumnMarginProperty); }
            set { SetValue(ColumnMarginProperty, value); }
        }

        public static readonly DependencyProperty ColumnMarginProperty = FieldGridWrapPanel.ColumnMarginProperty.AddOwner(typeof(SearchPanel));

        public double RowMargin
        {
            get { return (double) GetValue(RowMarginProperty); }
            set { SetValue(RowMarginProperty, value); }
        }

        public static readonly DependencyProperty RowMarginProperty = FieldGridWrapPanel.RowMarginProperty.AddOwner(typeof(SearchPanel));

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
            SearchFields = new FieldCollection();
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
                if (!string.IsNullOrWhiteSpace(item.Title))
                    displayText.Text = item.Title;
                else if (!string.IsNullOrEmpty(displayAttribText))
                    displayText.Text = displayAttribText;
                else
                    displayText.Text = item.FilterField;

                displayText.HorizontalAlignment = HorizontalAlignment.Right;
                displayText.VerticalAlignment = VerticalAlignment.Center;
                displayText.Margin = new Thickness(0, 0, 5, 0);

                if (item is DateField && (item as DateField).DateFormat == DateFormat.RangeFrom)
                {
                    string appendText1 = " از";
                    string appendText2 = " تا";

                    if (!string.IsNullOrWhiteSpace(item.Title))
                    {
                        if (item.Title.EndsWith(" :"))
                        {
                            appendText1 = " از :";
                            appendText2 = " تا :";
                            item.Title = item.Title.Remove(item.Title.Length - 2, 2);
                        }
                        else if (item.Title.EndsWith(":"))
                        {
                            appendText1 = " از:";
                            appendText2 = " تا:";
                            item.Title = item.Title.Remove(item.Title.Length - 1, 1);
                        }

                        displayText.Text = item.Title + appendText1;
                    }
                    (item as DateField).Operator = NumericOperator.GreaterThanEqual;

                    DateField item2 = new DateField();
                    item2.DateFormat = DateFormat.RangeTo;
                    item2.OpertatorVisible = false;
                    item2.FilterField = (item as DateField).FilterField;
                    if (!string.IsNullOrWhiteSpace(item.Title))
                        item2.Title = item.Title + appendText2;
                    else
                        item2.Title = item2.FilterField;
                    item2.Operator = NumericOperator.LessThanEqual;
                    (item as DateField).Partner = item2;
                    item2.Partner = item as DateField;
                    SearchFields.Insert(i + 1, item2);
                }

                searchpanel.Children.Add(displayText);
                searchpanel.Children.Add((Control) item);
            }
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
            {
                if (SearchCommand != null)
                    SearchCommand.Execute(SearchPhrase);
            }
        }

        #endregion
    }
}
