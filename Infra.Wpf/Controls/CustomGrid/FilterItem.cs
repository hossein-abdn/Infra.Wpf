using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using C1.WPF;

namespace Infra.Wpf.Controls
{
    public class FilterItem : INotifyPropertyChanged
    {
        #region Properties
        
        private bool _IsChecked = true;
        
        public event PropertyChangedEventHandler PropertyChanged;
        
        public event EventHandler FilterItemChanged;
        
        private static C1MaskedTextBox maskedTextBox;
        
        static FilterItem()
        {
            maskedTextBox = new C1MaskedTextBox();
            maskedTextBox.TextMaskFormat = MaskFormat.IncludeLiterals;
        }
        
        public bool IsChecked
        {
            get
            {
                return _IsChecked;
            }
            set
            {
                if (_IsChecked != value)
                {
                    _IsChecked = value;
                    OnPropertyChanged();
                }
            }
        }
        
        private object _Value;
        
        public object Value
        {
            get
            {
                return _Value;
            }
            set
            {
                if (_Value != value)
                {
                    _Value = value;
                    OnPropertyChanged();
                }
            }
        }
        
        public string Mask { get; set; }
        
        public object Text
        {
            get
            {
                if (Mask == string.Empty)
                    return Value;
                else
                {
                    maskedTextBox.Mask = Mask;
                    maskedTextBox.Value = Value.ToString();
                    return maskedTextBox.Value;
                }
            }
        }
        
        #endregion
        
        #region Methods

        private void OnPropertyChanged([CallerMemberName] string prop = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
                FilterItemChanged?.Invoke(this, new EventArgs());
            }
        }
    
        #endregion
    }
}