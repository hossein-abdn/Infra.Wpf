using Infra.Wpf.Mvvm;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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

        public static readonly DependencyProperty RowMarginProperty = FieldGridWrapPanel.RowMarginProperty.AddOwner(typeof(EditViewPanel),new PropertyMetadata(RowMarginChanged));

        public Visibility VisibleTopButton
        {
            get { return (Visibility) GetValue(VisibleTopButtonProperty); }
            set { SetValue(VisibleTopButtonProperty, value); }
        }

        public static readonly DependencyProperty VisibleTopButtonProperty =
            DependencyProperty.Register("VisibleTopButton", typeof(Visibility), typeof(EditViewPanel), new PropertyMetadata(Visibility.Collapsed));

        #endregion

        #region Method

        public EditViewPanel()
        {
            EditFields = new FieldCollection();
            CancelCommand = new RelayCommand(CancelExecute);

            InitializeComponent();
        }

        private void mainpanel_Loaded(object sender, RoutedEventArgs e)
        {
            editpanel.Children.Clear();

            for (int i = 0; i < EditFields.Count; i++)
            {
                var item = EditFields[i];

                TextBlock displayText = new TextBlock();

                if (!string.IsNullOrWhiteSpace(item.Title))
                    displayText.Text = item.Title;
                else
                {
                    Binding bind = new Binding("DisplayName") { Source = item };
                    displayText.SetBinding(TextBlock.TextProperty, bind);
                }

                if (item is DateField)
                {
                    ((DateField) item).OpertatorVisible = false;
                    ((DateField) item).SuggestionVisible = false;
                }
                else if (item is NumericField)
                {
                    ((NumericField) item).OpertatorVisible = false;
                    ((NumericField) item).ShowButtons = true;
                }
                else if (item is TextField)
                    ((TextField) item).OpertatorVisible = false;

                displayText.HorizontalAlignment = HorizontalAlignment.Right;
                displayText.VerticalAlignment = VerticalAlignment.Center;
                displayText.Margin = new Thickness(0, 0, 5, 0);

                editpanel.Children.Add(displayText);
                editpanel.Children.Add((Control) item);
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
            var margin = @this.Submit2.Margin;
            @this.Submit1.Margin = new Thickness(margin.Left, margin.Top , margin.Right, margin.Bottom + (double) e.NewValue);
            @this.Submit2.Margin = new Thickness(margin.Left, margin.Top + (double) e.NewValue, margin.Right, margin.Bottom);

            margin = @this.Cancel2.Margin;
            @this.Cancel1.Margin = new Thickness(margin.Left, margin.Top , margin.Right, margin.Bottom + (double) e.NewValue);
            @this.Cancel2.Margin = new Thickness(margin.Left, margin.Top + (double) e.NewValue, margin.Right, margin.Bottom);
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
