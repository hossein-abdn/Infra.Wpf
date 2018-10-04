using Infra.Wpf.Mvvm;
using System;
using System.Collections.Generic;
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

namespace Infra.Wpf.Controls
{
    [ContentProperty("EditFields")]
    public class EditViewPanel : Control
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

        public static readonly DependencyProperty RowMarginProperty = FieldGridWrapPanel.RowMarginProperty.AddOwner(typeof(EditViewPanel), new PropertyMetadata(RowMarginChanged));

        public Visibility VisibleTopButton
        {
            get { return (Visibility) GetValue(VisibleTopButtonProperty); }
            set { SetValue(VisibleTopButtonProperty, value); }
        }

        public static readonly DependencyProperty VisibleTopButtonProperty =
            DependencyProperty.Register("VisibleTopButton", typeof(Visibility), typeof(EditViewPanel), new PropertyMetadata(Visibility.Collapsed));

        public Type ModelType { get; set; }

        private FieldGridWrapPanel editpanel { get; set; }

        private Button cancel1 { get; set; }

        private Button cancel2 { get; set; }

        private Button submit1 { get; set; }

        private Button submit2 { get; set; }
        #endregion

        #region Method

        static EditViewPanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EditViewPanel), new FrameworkPropertyMetadata(typeof(EditViewPanel)));
        }

        public override void OnApplyTemplate()
        {
            editpanel = Template.FindName("editpanel", this) as FieldGridWrapPanel;
            submit1 = Template.FindName("Submit1", this) as Button;
            submit2 = Template.FindName("Submit2", this) as Button;
            cancel1 = Template.FindName("Cancel1", this) as Button;
            cancel2 = Template.FindName("Cancel2", this) as Button;

            RowMarginChanged(this, new DependencyPropertyChangedEventArgs(RowMarginProperty, null, this.GetValue(RowMarginProperty)));

            base.OnApplyTemplate();
        }

        public EditViewPanel()
        {
            EditFields = new FieldCollection();
            CancelCommand = new RelayCommand(CancelExecute);

            this.Loaded += mainpanel_Loaded;
            this.KeyDown += mainpanel_KeyDown;

            this.IsTabStop = false;
        }

        private void mainpanel_Loaded(object sender, RoutedEventArgs e)
        {
            editpanel.Children.Clear();

            for (int i = 0; i < EditFields.Count; i++)
            {
                var item = EditFields[i];
                item.ModelType = this.ModelType;

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
                    ((DateField) item).OperatorVisible = false;
                    ((DateField) item).SuggestionVisible = false;
                }
                else if (item is NumericField)
                {
                    ((NumericField) item).OperatorVisible = false;
                    ((NumericField) item).ShowButtons = true;
                }
                else if((item is TimeField))
                {
                    ((TimeField) item).OperatorVisible = false;
                    ((TimeField) item).ShowButtons = true;
                }
                else if (item is TextField)
                    ((TextField) item).OperatorVisible = false;

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

            if (@this.submit1 == null)
                return;

            var margin = @this.submit2.Margin;
            @this.submit1.Margin = new Thickness(margin.Left, margin.Top, margin.Right, margin.Bottom + (double) e.NewValue);
            @this.submit2.Margin = new Thickness(margin.Left, margin.Top + (double) e.NewValue, margin.Right, margin.Bottom);

            margin = @this.cancel2.Margin;
            @this.cancel1.Margin = new Thickness(margin.Left, margin.Top, margin.Right, margin.Bottom + (double) e.NewValue);
            @this.cancel2.Margin = new Thickness(margin.Left, margin.Top + (double) e.NewValue, margin.Right, margin.Bottom);
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
