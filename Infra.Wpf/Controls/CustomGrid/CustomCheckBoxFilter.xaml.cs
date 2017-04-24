using System.Runtime.CompilerServices;
using C1.WPF.DataGrid;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Infra.Wpf.Controls
{
    public partial class CustomCheckBoxFilter : UserControl, IDataGridFilterUnity
    {
        #region Methods
        
        public CustomCheckBoxFilter()
        {
            InitializeComponent();
            IsSetFilter = true;
            IsGetFilter = true;
        }
        
        private void GetFilter()
        {
            if (IsGetFilter == false)
                return;
            
            IsSetFilter = false;
            DataGridFilterInfo filterInfo = null;
            if (Filter != null && Filter.FilterInfo != null)
            {
                foreach (var item in Filter.FilterInfo)
                {
                    if (item.FilterType == DataGridFilterType.CheckBox)
                    {
                        filterInfo = item;
                        break;
                    }
                }
            }

            if (filterInfo != null)
                Selected = (filterInfo.Value as bool?);
            else
                Selected = null;
            
            IsSetFilter = true;
        }
        
        private DataGridFilterState SetFilter(bool isNullFilter = false)
        {
            List<DataGridFilterInfo> filterList = new List<DataGridFilterInfo>();
            DataGridFilterInfo filterInfo = new DataGridFilterInfo();
            filterInfo.FilterType = DataGridFilterType.CheckBox;
            if (isNullFilter == false)
                filterInfo.Value = Selected;
            else
                filterInfo.Value = null;
            filterInfo.FilterOperation = DataGridFilterOperation.Equal;
            filterList.Add(filterInfo);
            
            return new DataGridFilterState { FilterInfo = filterList };
        }
        
        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (IsSetFilter == true)
            {
                IsGetFilter = false;
                Filter = SetFilter();
                IsGetFilter = true;
            }
        }
        
        private void OnPropertyChanged([CallerMemberName]string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        
        #endregion
        
        #region Properties
        
        public event PropertyChangedEventHandler PropertyChanged;
        
        private DataGridFilterState _Filter;
        
        public DataGridFilterState Filter
        {
            get
            {
                return _Filter;
            }
            set
            {
                if (_Filter != value)
                {
                    _Filter = value;                   
                    OnPropertyChanged();
                    GetFilter();
                }
            }
        }
        
        private bool? _Selected;
        
        public bool? Selected
        {
            get
            {
                return _Selected;
            }
            set
            {
                if (_Selected != value)
                {
                    _Selected = value;
                    OnPropertyChanged();
                }
            }
        }
        
        public bool IsGetFilter { get; set; }
        
        public bool IsSetFilter { get; set; }
    
        #endregion
    }
}