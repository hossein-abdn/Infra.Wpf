using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Infra.Wpf.Controls
{
    public class HierarchicalData
    {
        public Type DataType { get; set; }

        public BindingBase ItemsSource { get; set; }

        public ImageSource Image { get; set; }

        public string Title { get; set; }

        public string SelectableField { get; set; }
    }
}
