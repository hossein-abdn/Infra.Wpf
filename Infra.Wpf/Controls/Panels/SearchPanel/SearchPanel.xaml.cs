using Infra.Wpf.Common.Helpers;
using Infra.Wpf.Mvvm;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations;
using System.Windows.Data;
using System;
using System.Collections.Generic;

namespace Infra.Wpf.Controls
{
    [ContentProperty("SearchFields")]
    public partial class SearchPanel : UserControl, INotifyPropertyChanged
    {
        #region Property

        public FieldCollection SearchFields { get; set; }

        public List<KeyValuePair<string, string>> SearchPhraseList
        {
            get { return (List<KeyValuePair<string, string>>)GetValue(SearchPhraseListProperty); }
            set { SetValue(SearchPhraseListProperty, value); }
        }

        public static readonly DependencyProperty SearchPhraseListProperty =
            DependencyProperty.Register("SearchPhraseList", typeof(List<KeyValuePair<string, string>>), typeof(SearchPanel), new PropertyMetadata(null));

        public RelayCommand ClearCommand { get; set; }

        public ICommand SearchCommand
        {
            get { return (ICommand)GetValue(SearchCommandProperty); }
            set { SetValue(SearchCommandProperty, value); }
        }

        public static readonly DependencyProperty SearchCommandProperty =
            DependencyProperty.Register("SearchCommand", typeof(ICommand), typeof(SearchPanel), new PropertyMetadata(null));

        public bool Stretch
        {
            get { return (bool)GetValue(StretchProperty); }
            set { SetValue(StretchProperty, value); }
        }

        public string Test { get; set; }

        public static readonly DependencyProperty StretchProperty = FieldGridWrapPanel.StretchProperty.AddOwner(typeof(SearchPanel));

        public double ColumnMargin
        {
            get { return (double)GetValue(ColumnMarginProperty); }
            set { SetValue(ColumnMarginProperty, value); }
        }

        public static readonly DependencyProperty ColumnMarginProperty = FieldGridWrapPanel.ColumnMarginProperty.AddOwner(typeof(SearchPanel));

        public double RowMargin
        {
            get { return (double)GetValue(RowMarginProperty); }
            set { SetValue(RowMarginProperty, value); }
        }

        public static readonly DependencyProperty RowMarginProperty = FieldGridWrapPanel.RowMarginProperty.AddOwner(typeof(SearchPanel), new PropertyMetadata(RowMarginChanged));

        public bool IsExpanded
        {
            get { return (bool)GetValue(IsExpandedProperty); }
            set { SetValue(IsExpandedProperty, value); }
        }

        public Type ModelType { get; set; }

        public static readonly DependencyProperty IsExpandedProperty = Expander.IsExpandedProperty.AddOwner(typeof(SearchPanel));

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Method

        public SearchPanel()
        {
            SearchFields = new FieldCollection();
            ClearCommand = new RelayCommand(ClearExecute);

            InitializeComponent();
        }

        private void mainpanel_Loaded(object sender, RoutedEventArgs e)
        {
            searchpanel.Children.Clear();

            for (int i = 0; i < SearchFields.Count; i++)
            {
                var item = SearchFields[i];
                item.ModelType = this.ModelType;

                TextBlock displayText = new TextBlock();

                if (!string.IsNullOrWhiteSpace(item.Title))
                    displayText.Text = item.Title;
                else
                {
                    Binding bind = new Binding("DisplayName") { Source = item };
                    displayText.SetBinding(TextBlock.TextProperty, bind);
                }

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
                    item2.OperatorVisible = false;
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

                item.SearchPhraseChanged += GenerateShearchPhrase;

                searchpanel.Children.Add(displayText);
                searchpanel.Children.Add((Control)item);
            }
        }

        private void GenerateShearchPhrase()
        {
            var result = new List<KeyValuePair<string, string>>();
            foreach (var item in SearchFields)
            {
                if (string.IsNullOrWhiteSpace(item.SearchPhrase) == false)
                    result.Add(new KeyValuePair<string, string>(item.FilterField, item.SearchPhrase));
            }

            SetValue(SearchPhraseListProperty, result);
        }

        private void ClearExecute()
        {
            foreach (var item in SearchFields)
                item.Clear();
        }

        private static void RowMarginChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var @this = ((SearchPanel)d);
            var margin = @this.Submit.Margin;
            @this.Submit.Margin = new Thickness(margin.Left, margin.Top + (double)e.NewValue, margin.Right, margin.Bottom);

            margin = @this.Clear.Margin;
            @this.Clear.Margin = new Thickness(margin.Left, margin.Top + (double)e.NewValue, margin.Right, margin.Bottom);
        }

        private void mainpanel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (SearchCommand != null)
                    SearchCommand.Execute(SearchPhraseList);
            }
        }

        public void OnPropertyChanged([CallerMemberName]string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        #endregion
    }
}
