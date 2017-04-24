using System;
using System.Windows.Data;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;

namespace Infra.Wpf.Controls
{
    public class CustomHyperlinkColumn : C1.WPF.DataGrid.DataGridBoundColumn
    {
        #region Methods

        public CustomHyperlinkColumn()
        {
            resource = new ResourceDictionary();
            resource.Source = new Uri("pack://application:,,,/Infra.Wpf;component/Controls/Resources/HyperlinkStyle.xaml");
            buttonTemplate = resource["HyperlinkTemplate"] as ControlTemplate;
        }

        public override object GetCellContentRecyclingKey(C1.WPF.DataGrid.DataGridRow row)
        {
            return typeof(Button);
        }

        public override System.Windows.FrameworkElement CreateCellContent(C1.WPF.DataGrid.DataGridRow row)
        {
            return new Button();
        }

        public override void BindCellContent(System.Windows.FrameworkElement cellContent, C1.WPF.DataGrid.DataGridRow row)
        {
            Button button = (Button)cellContent;

            button.HorizontalAlignment = base.HorizontalAlignment;
            button.VerticalAlignment = base.VerticalAlignment;
            button.Margin = new Thickness(4, 4, 4, 4);
            if (FlowDirection != null)
                button.FlowDirection = (FlowDirection)FlowDirection;

            if (!string.IsNullOrEmpty(Text))
                button.DataContext = new { Text = this.Text };


            Binding bindCommand = new Binding("Command") { Source = this };
            Binding bindCommandParameter = new Binding("DataItem") { Source = row };
            button.SetBinding(Button.CommandProperty, bindCommand);
            button.SetBinding(Button.CommandParameterProperty, bindCommandParameter);

            button.Template = buttonTemplate;
        }
        
        #endregion

        #region Properties

        private ResourceDictionary resource;

        private ControlTemplate buttonTemplate;

        public ButtonColumnViewModel ViewModel = new ButtonColumnViewModel();

        public string Text { get; set; }

        public ICommand Command
        {
            get
            {
                return (ICommand)GetValue(CommandProperty);
            }
            set
            {
                SetValue(CommandProperty, value);
            }
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(CustomHyperlinkColumn), new PropertyMetadata(null));

        public FlowDirection? FlowDirection
        {
            get
            {
                return (FlowDirection?)GetValue(FlowDirectionProperty);
            }
            set
            {
                SetValue(FlowDirectionProperty, value);
            }
        }

        public static readonly DependencyProperty FlowDirectionProperty =
            DependencyProperty.Register("FlowDirection", typeof(FlowDirection?), typeof(CustomHyperlinkColumn), new PropertyMetadata(null));

        #endregion
    }
}