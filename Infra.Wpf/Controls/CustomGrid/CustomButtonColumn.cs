using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Infra.Wpf.Controls
{
    public class CustomButtonColumn : C1.WPF.DataGrid.DataGridBoundColumn
    {
        #region Methods
        
        public CustomButtonColumn()
        {
            resource = new ResourceDictionary();
            resource.Source = new Uri("pack://application:,,,/Infra.Wpf;component/Controls/Resources/ButtonStyle.xaml");
            buttonTemplate = resource["ButtonTemplate"] as ControlTemplate;
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
            button.HorizontalAlignment = HorizontalAlignment.Center;
            button.VerticalAlignment = VerticalAlignment.Center;
            button.Margin = new Thickness(0, 0, 0, 0);
            button.DataContext = ViewModel;
            if (ToolTip != null)
            {
                Binding bindToolTip = new Binding("ToolTip") { Source = this };
                button.SetBinding(Button.ToolTipProperty, bindToolTip); 
            }
            else
            {
                Binding bindToolTip = new Binding("Header") { Source = this };
                button.SetBinding(Button.ToolTipProperty, bindToolTip); 
            }
            
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

        public int? Order { get; set; }

        public ImageSource Image
        {
            get
            {
                return ViewModel.Image;
            }
            set
            {
                ViewModel.Image = value;
            }
        }
        
        public ImageSource MouseOverImage
        {
            get
            {
                return ViewModel.MouseOverImage;
            }
            set
            {
                ViewModel.MouseOverImage = value;
            }
        }
        
        public enum ColumnType
        {
            None,
            Edit,
            Delete,
            View
        }
        
        private ColumnType _ButtonType;
        
        public ColumnType ButtonType
        {
            get
            {
                return _ButtonType;
            }
            
            set
            {
                _ButtonType = value;
                if (Image == null || MouseOverImage == null)
                {
                    switch (_ButtonType)
                    {
                        case ColumnType.Edit:
                            Image = new BitmapImage(new Uri("pack://application:,,,/Infra.Wpf;component/Controls/Resources/Edit-24.png"));
                            MouseOverImage = new BitmapImage(new Uri("pack://application:,,,/Infra.Wpf;component/Controls/Resources/EditOver-24.png"));
                            ToolTip = "ویرایش";
                            break;
                        case ColumnType.Delete:
                            Image = new BitmapImage(new Uri("pack://application:,,,/Infra.Wpf;component/Controls/Resources/Delete-24.png"));
                            MouseOverImage = new BitmapImage(new Uri("pack://application:,,,/Infra.Wpf;component/Controls/Resources/DeleteOver-24.png"));
                            ToolTip = "حذف";
                            break;
                        case ColumnType.View:
                            Image = new BitmapImage(new Uri("pack://application:,,,/Infra.Wpf;component/Controls/Resources/View-24.png"));
                            MouseOverImage = new BitmapImage(new Uri("pack://application:,,,/Infra.Wpf;component/Controls/Resources/ViewOver-24.png"));
                            ToolTip = "نمایش";
                            break;
                    }
                }
            }
        }

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(CustomButtonColumn), new PropertyMetadata(null));

        public object ToolTip
        {
            get { return (object)GetValue(ToolTipProperty); }
            set { SetValue(ToolTipProperty, value); }
        }

        public static readonly DependencyProperty ToolTipProperty =
            DependencyProperty.Register("ToolTip", typeof(object), typeof(CustomButtonColumn), new PropertyMetadata(null));
   
        #endregion
    }
}