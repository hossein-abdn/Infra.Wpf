using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using C1.WPF.DataGrid;
using Infra.Wpf.Common;

namespace Infra.Wpf.Controls
{
    public partial class CustomDateTimeFilter : UserControl, IDataGridFilterUnity
    {
        #region Methods
        
        public CustomDateTimeFilter()
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

            FromDate = null;
            ToDate = null;
            FromTime = null;
            ToTime = null;

            if (Filter != null && Filter.FilterInfo != null)
            {
                foreach (var item in Filter.FilterInfo)
                {
                    if (item.FilterType == DataGridFilterType.DateTime)
                    {
                        if (item.FilterOperation == DataGridFilterOperation.GreaterThanOrEqual)
                        {
                            DateTime date = (DateTime)item.Value;
                            if (IsTimeVisible)
                            {
                                FromDate = new PersianDate(date.Date);
                                FromTime = date.TimeOfDay;
                            }
                            else
                                FromDate = new PersianDate(date);
                        }
                        
                        if (item.FilterOperation == DataGridFilterOperation.LessThanOrEqual)
                        {
                            DateTime date = (DateTime)item.Value;
                            if (IsTimeVisible)
                            {
                                ToDate = new PersianDate(date.Date);
                                ToTime = date.TimeOfDay;
                            }
                            else
                                ToDate = new PersianDate(date);
                        }
                    }
                }
            }
            
            IsSetFilter = true;
        }
        
        private DataGridFilterState SetFilter()
        {
            List<DataGridFilterInfo> filterInfoList = new List<DataGridFilterInfo>();
            
            DataGridFilterInfo filterFrom = new DataGridFilterInfo();
            filterFrom.FilterType = DataGridFilterType.DateTime;
            filterFrom.FilterOperation = DataGridFilterOperation.GreaterThanOrEqual;

            if (FromDate != null)
            {
                PersianDate from = null;
                if (IsTimeVisible && FromTime.HasValue)
                    from = new PersianDate(FromDate, FromTime.Value);
                else
                    from = new PersianDate(FromDate.Year, FromDate.Month, FromDate.Day, 0, 0, 0);

                filterFrom.Value = from.ToDateTime();
                filterInfoList.Add(filterFrom);
            }

            
            DataGridFilterInfo filterTo = new DataGridFilterInfo();
            filterTo.FilterType = DataGridFilterType.DateTime;
            filterTo.FilterOperation = DataGridFilterOperation.LessThanOrEqual;
            filterTo.FilterCombination = filterFrom.Value != null ? DataGridFilterCombination.And : DataGridFilterCombination.None;

            if (ToDate != null)
            {
                PersianDate to = null;
                if (IsTimeVisible && ToTime.HasValue)
                    to = new PersianDate(ToDate, ToTime.Value);
                else
                    to = new PersianDate(ToDate.Year, ToDate.Month, ToDate.Day, 23, 59, 59);

                filterTo.Value = to.ToDateTime();
                filterInfoList.Add(filterTo);
            }
            
            if (filterInfoList.Any())
                return new DataGridFilterState { FilterInfo = filterInfoList };
            
            return null;
        }
        
        private void PersianDatePicker_SelectedDateChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            if (IsSetFilter == true)
            {
                IsGetFilter = false;
                Filter = SetFilter();
                IsGetFilter = true;
            }
        }
        
        private void TimeEditor_ValueChanged(object sender, TimeEditorValueChangedEventArgs e)
        {
            if (IsSetFilter == true)
            {
                IsGetFilter = false;
                Filter = SetFilter();
                IsGetFilter = true;
            }
        }
        
        private void OnPropertyChanged([CallerMemberName]
                                       string prop = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        
        #endregion
        
        #region Properties
        
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
        
        private PersianDate _FromDate;
        
        public PersianDate FromDate
        {
            get
            {
                return _FromDate;
            }
            set
            {
                if (_FromDate != value)
                {
                    _FromDate = value;
                    OnPropertyChanged();
                }
            }
        }
        
        private PersianDate _ToDate;
        
        public PersianDate ToDate
        {
            get
            {
                return _ToDate;
            }
            set
            {
                if (_ToDate != value)
                {
                    _ToDate = value;
                    OnPropertyChanged();
                }
            }
        }
        
        private TimeSpan? _FromTime;
        
        public TimeSpan? FromTime
        {
            get
            {
                return _FromTime;
            }
            set
            {
                if (_FromTime != value)
                {
                    _FromTime = value;
                    OnPropertyChanged();
                }
            }
        }
        
        private TimeSpan? _ToTime;
        
        public TimeSpan? ToTime
        {
            get
            {
                return _ToTime;
            }
            set
            {
                if (_ToTime != value)
                {
                    _ToTime = value;
                    OnPropertyChanged();
                }
            }
        }
        
        private bool _IsTimeVisible;
        
        public bool IsTimeVisible
        {
            get
            {
                return _IsTimeVisible;
            }
            set 
            {
                if (_IsTimeVisible != value)
                {
                    _IsTimeVisible = value;
                    OnPropertyChanged();
                }
            }
        }
        
        public bool IsSetFilter { get; set; }
        
        public bool IsGetFilter { get; set; }
        
        public event PropertyChangedEventHandler PropertyChanged;
    
        #endregion
    }
}