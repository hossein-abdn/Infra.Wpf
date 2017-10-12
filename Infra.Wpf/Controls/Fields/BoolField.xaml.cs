using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace Infra.Wpf.Controls
{
    public partial class BoolField : UserControl, INotifyPropertyChanged, IField
    {
        #region Properties

        public string Title { get; set; }

        public string FilterField { get; set; }

        public string FilterText { get; set; }

        public string SearchPhrase
        {
            get
            {
                if (string.IsNullOrWhiteSpace(FilterText))
                    return "";

                return $@"{FilterField}=={FilterText.Trim()}";
            }
        }

        public bool IsChecked
        {
            get { return (bool) GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }

        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.Register("IsChecked", typeof(bool), typeof(BoolField), 
                new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIsCheckedChange));

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Methods

        public BoolField()
        {
            InitializeComponent();
        }

        public void OnPropertyChanged([CallerMemberName]string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public void Clear()
        {
            IsChecked = false;
            FilterText = "";
        }

        private static void OnIsCheckedChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var @this = (BoolField) d;
            if (@this != null)
                @this.FilterText = ((bool)e.NewValue).ToString();
        }

        #endregion
    }
}
