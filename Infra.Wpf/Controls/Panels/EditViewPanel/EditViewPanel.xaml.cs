using Infra.Wpf.Mvvm;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;

namespace Infra.Wpf.Controls
{
    [ContentProperty("EditFields")]
    public partial class EditViewPanel : UserControl
    {
        #region Property

        public FieldCollection EditFields { get; set; }

        public RelayCommand CancelCommand { get; set; }

        public ICommand SubmitCommand
        {
            get { return (ICommand) GetValue(SubmitCommandProperty); }
            set { SetValue(SubmitCommandProperty, value); }
        }

        public static readonly DependencyProperty SubmitCommandProperty =
            DependencyProperty.Register("SubmitCommand", typeof(ICommand), typeof(EditViewPanel), new PropertyMetadata(null));

        public bool Stretch
        {
            get { return (bool) GetValue(StretchProperty); }
            set { SetValue(StretchProperty, value); }
        }

        public static readonly DependencyProperty StretchProperty = FieldGridWrapPanel.StretchProperty.AddOwner(typeof(EditViewPanel));

        public double ColumnMargin
        {
            get { return (double) GetValue(ColumnMarginProperty); }
            set { SetValue(ColumnMarginProperty, value); }
        }

        public static readonly DependencyProperty ColumnMarginProperty = FieldGridWrapPanel.ColumnMarginProperty.AddOwner(typeof(EditViewPanel));

        public double RowMargin
        {
            get { return (double) GetValue(RowMarginProperty); }
            set { SetValue(RowMarginProperty, value); }
        }

        public static readonly DependencyProperty RowMarginProperty = FieldGridWrapPanel.RowMarginProperty.AddOwner(typeof(EditViewPanel));

        #endregion

        #region Method

        public EditViewPanel()
        {
            EditFields = new FieldCollection();
            CancelCommand = new RelayCommand(CancelExecute);

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
            for (int i = 0; i < EditFields.Count; i++)
            {
                var item = EditFields[i];

                TextBlock displayText = new TextBlock();

                string displayAttribText = GetDisplayName(EditFields[i].FilterField);
                if (!string.IsNullOrWhiteSpace(item.Title))
                    displayText.Text = item.Title;
                else if (!string.IsNullOrEmpty(displayAttribText))
                    displayText.Text = displayAttribText;
                else
                {
                    BindingExpression bindExpression = null;
                    if (item is BoolField)
                        bindExpression = BindingOperations.GetBindingExpression((BoolField) item, BoolField.IsCheckedProperty);
                    else if (item is ComboField)
                        bindExpression = BindingOperations.GetBindingExpression((ComboField) item, CustomComboBox.SelectedItemProperty);
                    else if (item is DateField)
                    {
                        bindExpression = BindingOperations.GetBindingExpression((DateField) item, DateField.SelectedDateProperty);
                        ((DateField) item).OpertatorVisible = false;
                        ((DateField) item).SuggestionVisible = false;
                    }
                    else if (item is LookupField)
                        bindExpression = BindingOperations.GetBindingExpression((LookupField) item, Lookup.SelectedItemsProperty);
                    else if (item is MultiSelect)
                        bindExpression = BindingOperations.GetBindingExpression((MultiSelect) item, MultiSelect.SelectedItemsProperty);
                    else if (item is NumericField)
                    {
                        bindExpression = BindingOperations.GetBindingExpression((NumericField) item, NumericField.ValueProperty);
                        ((NumericField) item).OpertatorVisible = false;
                        ((NumericField) item).ShowButtons = true;
                    }
                    else if (item is TextField)
                    {
                        bindExpression = BindingOperations.GetBindingExpression((TextField) item, TextField.TextProperty);
                        ((TextField) item).OpertatorVisible = false;
                    }

                    displayText.Text = bindExpression?.ParentBinding?.Path?.Path;
                }

                displayText.HorizontalAlignment = HorizontalAlignment.Right;
                displayText.VerticalAlignment = VerticalAlignment.Center;
                displayText.Margin = new Thickness(0, 0, 5, 0);

                searchpanel.Children.Add(displayText);
                searchpanel.Children.Add((Control) item);
            }
        }

        private void CancelExecute()
        {
            var vm = DataContext as IViewModelBase;
            vm?.NavigationService?.GoBack();
        }

        private static void RowMarginChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var @this = ((EditViewPanel) d);
            var margin = @this.Submit.Margin;
            @this.Submit.Margin = new Thickness(margin.Left, margin.Top + (double) e.NewValue, margin.Right, margin.Bottom);

            margin = @this.Cancel.Margin;
            @this.Cancel.Margin = new Thickness(margin.Left, margin.Top + (double) e.NewValue, margin.Right, margin.Bottom);
        }
        private void mainpanel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (SubmitCommand != null)
                    SubmitCommand.Execute(EditFields);
            }
        }

        #endregion
    }
}
