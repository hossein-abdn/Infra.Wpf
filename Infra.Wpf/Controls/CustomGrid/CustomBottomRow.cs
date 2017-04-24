using C1.WPF.DataGrid;
using Infra.Wpf.Mvvm;
using System;
using System.Linq;
using System.Windows;

namespace Infra.Wpf.Controls
{
    public class CustomBottomRow : DataGridRow
    {
        #region Properties

        public RelayCommand RefreshCommand { get; private set; }

        public RelayCommand ExcelCommand { get; private set; }

        private string _Count;

        public string Count
        {
            get
            {
                return _Count;
            }
            set
            {
                if (_Count != value)
                {
                    _Count = value;
                    OnPropertyChanged("Count");
                }
            }
        }

        #endregion

        #region Methods

        public CustomBottomRow()
        {
            RefreshCommand = new RelayCommand(RefreshExecute, () => true);
            ExcelCommand = new RelayCommand(ExcelExecute, () => true);
            Count = "0";
        }

        private void ItemsSourceChanged(object sender, EventArgs e)
        {
            int sourceCount = (DataGrid as CustomGrid).ItemsSourceCount;
            int rowCount = DataGrid.Rows.Count(x => x.Type == DataGridRowType.Item);

            if (sourceCount == rowCount)
                Count = sourceCount.ToString();
            else
                Count = string.Format("{0} از {1}", rowCount, sourceCount);
        }

        protected override void BindRowPresenter(DataGridRowPresenter presenter)
        {
            presenter.DataContext = this;
            ItemsSourceChanged(this, new EventArgs());
        }

        protected override void OnLoaded()
        {
            ResourceDictionary resource = new ResourceDictionary();
            resource.Source = new Uri("pack://application:,,,/Infra.Wpf;component/Controls/Resources/RowStyle.xaml");

            Style rowStyle = rowStyle = resource["RowStyle"] as Style;
            RowStyle = rowStyle;

            (DataGrid as CustomGrid).ItemsSourceChanged += ItemsSourceChanged;
            DataGrid.FilterChanged += ItemsSourceChanged;
        }

        private void RefreshExecute()
        {
            DataGrid.Refresh();
        }

        private void ExcelExecute()
        {
            ExportExcel dialog = new ExportExcel(DataGrid);
            dialog.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            dialog.FlowDirection = DataGrid.FlowDirection;
            dialog.ShowDialog();
        }

        #endregion
    }
}