using System;
using System.Collections;

namespace Infra.Wpf.Controls
{
    public class SelectionChangedEventArgs:EventArgs
    {
        public IEnumerable Items { set; get; }
    }
}
