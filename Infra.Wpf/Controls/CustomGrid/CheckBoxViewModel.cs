using Infra.Wpf.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Wpf.Controls
{
    public class CheckBoxViewModel : ViewModelBase
    {
        public object Item
        {
            get { return Get<object>(); }
            set { Set(value); }
        }

        public bool IsSelected
        {
            get { return Get<bool>(); }
            set { Set(value); }
        }
    }
}
