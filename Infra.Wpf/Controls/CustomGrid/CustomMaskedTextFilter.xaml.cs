using System.Runtime.CompilerServices;
using C1.WPF;
using C1.WPF.DataGrid;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Infra.Wpf.Controls
{
    public partial class CustomMaskedTextFilter : UserControl, IDataGridFilterUnity
    {
        #region Methods
        
        public CustomMaskedTextFilter()
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
        
        private void OnPropertyChanged([CallerMemberName]string prop = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
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
        
        private string _Mask;
        
        public string Mask 
        { 
            get
            {
                return _Mask;
            }
            set
            {
                if (_Mask != value)
                {
                    _Mask = value;
                    OnPropertyChanged();
                }
            }
        }
        
        private MaskFormat _TextMaskFormat;
        
        public MaskFormat TextMaskFormat 
        {
            get
            {
                return _TextMaskFormat;
            }
            set
            {
                if (_TextMaskFormat != value)
                {
                    _TextMaskFormat = value;
                    OnPropertyChanged();
                }
            }
        }
        
        public System.Windows.FlowDirection? MaskFlowDirection
        {
            get
            {
                return (FlowDirection?)GetValue(MaskFlowDirectionProperty);
            }
            set
            {
                SetValue(MaskFlowDirectionProperty, value);
            }
        }
        
        public static readonly DependencyProperty MaskFlowDirectionProperty =
            DependencyProperty.Register("MaskFlowDirection", typeof(FlowDirection?), typeof(CustomMaskedTextFilter), new PropertyMetadata(null));
    
        #endregion
    }
}