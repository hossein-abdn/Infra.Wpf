using Infra.Wpf.Controls;
using System.Windows;
using System.Windows.Threading;

namespace Infra.Wpf.Mvvm
{
    public interface IViewModelBase
    {
        NavigationService NavigationService { get; set; }

        Billboard Billboard { get; set; }

        Dispatcher Dispatcher { get; set; }

        object View { get; set; }

        string ViewTitle { get; set; }

        MsgResult ShowMessageBox(string message, string caption, MsgButton button, MsgIcon icon, MsgResult defaultResult, Window owner = null);
    }
}
