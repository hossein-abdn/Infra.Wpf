using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Infra.Wpf.Controls
{
    public partial class LookupWindow : Window, INotifyPropertyChanged
    {
        public enum LookupModeEnum
        {
            Grid,
            Tree
        }

        public LookupModeEnum LookupMode { get; set; }

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

        public event PropertyChangedEventHandler PropertyChanged;

        public LookupWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Binding bind = new Binding(".");
            if (LookupMode == LookupModeEnum.Grid)
            {
                datagrid.Visibility = Visibility.Visible;
                datagrid.SetBinding(CustomGrid.ItemsSourceProperty, bind);
            }
            else
            {
                treeview.Visibility = Visibility.Visible;
                treeview.SetBinding(TreeView.ItemsSourceProperty, bind);
            }

            if (SelectionMode == LookupSelectionMode.Single)
                stackpanel.Visibility = Visibility.Collapsed;
            if (searchpanel.SearchCommand == null || searchpanel.SearchFields.Count == 0)
                searchpanel.Visibility = Visibility.Collapsed;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            searchpanel.searchpanel.Children.RemoveRange(0, searchpanel.searchpanel.Children.Count);
            base.OnClosing(e);
        }

        public void OnPropertyChanged([CallerMemberName]string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private void confirm_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            searchpanel.ClearCommand.Execute(null);
            Close();
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            searchpanel.ClearCommand.Execute(null);
        }

        private void searchpanel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                e.Handled = true;
        }
    }
}
