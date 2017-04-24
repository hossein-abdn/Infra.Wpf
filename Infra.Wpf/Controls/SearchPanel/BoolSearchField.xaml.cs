using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

namespace Infra.Wpf.Controls
{
    public partial class BoolSearchField : UserControl, INotifyPropertyChanged, ISearchField
    {
        #region Properties

        public string DisplayName { get; set; }

        public string FilterField { get; set; }

        private bool _FilterValue;
        public bool FilterValue
        {
            get { return _FilterValue; }
            set
            {
                _FilterValue = value;
                FilterText = value.ToString();
                OnPropertyChanged();
            }
        }

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

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Methods

        public BoolSearchField()
        {
            InitializeComponent();
        }

        public void OnPropertyChanged([CallerMemberName]string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public void Clear()
        {
            FilterValue = false;
            FilterText = "";
        }

        #endregion
    }
}
