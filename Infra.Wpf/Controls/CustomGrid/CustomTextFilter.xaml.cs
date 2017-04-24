using System.Runtime.CompilerServices;
using C1.WPF.DataGrid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;

namespace Infra.Wpf.Controls
{
    public partial class CustomTextFilter : UserControl, IDataGridFilterUnity
    {
        #region Methods

        public CustomTextFilter()
        {
            InitializeComponent();
            
            IsSetFilter = true;
            IsGetFilter = true;
            
            FilterOperationList = new List<string>
            {
                "شامل",
                "شروع شود با",
                "پایان یابد با",
                "مساوی",
                "نامساوی"
            };
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
                    if (item.FilterType == DataGridFilterType.Text)
                    {
                        filterInfo = item;
                        break;
                    }
                }
            }
            
            if (filterInfo != null)
            {
                TextFilter = filterInfo.Value.ToString();
                TextOperation = filterInfo.FilterOperation;
            }
            else
            {
                TextFilter = string.Empty;
                TextOperation = DataGridFilterOperation.Contains;
            }
            IsSetFilter = true;
        }
        
        private DataGridFilterState SetFilter()
        {
            List<DataGridFilterInfo> filterInfoList = new List<DataGridFilterInfo>();
            if (string.IsNullOrWhiteSpace(TextFilter) == false)
            {
                DataGridFilterInfo filterInfo = new DataGridFilterInfo();
                filterInfo.FilterType = DataGridFilterType.Text;
                filterInfo.Value = TextFilter;
                filterInfo.FilterOperation = TextOperation;
                if (filterInfo != null)
                    filterInfoList.Add(filterInfo);
            }
            
            if (Filter != null && Filter.FilterInfo != null)
            {
                foreach (var item in Filter.FilterInfo)
                {
                    if (item.FilterType != DataGridFilterType.Text)
                        filterInfoList.Add(item);
                }
            }
            
            if (filterInfoList.Any())
                return new DataGridFilterState { FilterInfo = filterInfoList };
            
            return null;
        }
        
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (IsSetFilter == true)
            {
                IsGetFilter = false;
                Filter = SetFilter();
                IsGetFilter = true;
            }
        }
        
        private void cmb_FilterOption_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (IsSetFilter == true)
            {
                IsGetFilter = false;
                Filter = SetFilter();
                IsGetFilter = true;
            }
        }

        private void OnPropertyChanged([CallerMemberName] string prop = null)
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
        
        private string _TextFilter;
        
        public string TextFilter 
        { 
            get
            {
                return _TextFilter;
            }
            
            set
            {
                if (_TextFilter != value)
                {
                    _TextFilter = value;
                    OnPropertyChanged();
                }
            }
        }
        
        private DataGridFilterOperation _TextOperation;
        
        public DataGridFilterOperation TextOperation 
        {
            get
            {
                return _TextOperation;
            }
            
            set
            {
                if (_TextOperation != value)
                {
                    _TextOperation = value;
                    OnPropertyChanged();
                }
            }
        }
        
        private List<string> _FilterOperationList;
        
        public List<string> FilterOperationList
        {
            get
            {
                return _FilterOperationList;
            }
            set
            {
                if (_FilterOperationList != value)
                {
                    _FilterOperationList = value;
                    OnPropertyChanged();
                }
            }
        }
        
        public bool IsSetFilter { get; set; }

        public bool IsGetFilter { get; set; }

        #endregion
    }
}