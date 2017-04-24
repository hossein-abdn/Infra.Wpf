using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Data;

namespace Infra.Wpf.Controls
{
    public class ComboSearchField : CustomComboBox, INotifyPropertyChanged, ISearchField
    {
        #region Properties

        public string DisplayName { get; set; }

        public string FilterField { get; set; }

        private object _FilterItem;
        public object FilterItem
        {
            get { return _FilterItem; }
            set
            {
                _FilterItem = value;
                OnPropertyChanged();
            }
        }

        public string SearchPhrase
        {
            get
            {
                if (FilterItem == null || string.IsNullOrWhiteSpace(FilterField))
                    return "";

                int value = (int) FilterItem;
                return $@"{FilterField}=={value}";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Methods

        public ComboSearchField()
        {
            VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
            IsNullable = true;
            Binding bind = new Binding("FilterItem")
            {
                Source = this
            };
            SetBinding(SelectedItemProperty, bind);
        }

        public void Clear()
        {
            FilterItem = null;
        }

        public void OnPropertyChanged([CallerMemberName]string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        #endregion
    }
}
