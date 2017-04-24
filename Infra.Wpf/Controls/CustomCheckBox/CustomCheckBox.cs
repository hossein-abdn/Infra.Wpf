using System.Windows;
using System.Windows.Controls;

namespace Infra.Wpf.Controls
{
    public class CustomCheckBox : CheckBox
    {
        static CustomCheckBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomCheckBox), new FrameworkPropertyMetadata(typeof(CustomCheckBox)));
        }
    }
}
