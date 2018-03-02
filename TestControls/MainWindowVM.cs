using DataAccess.Models;
using Infra.Wpf.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace TestControls
{
    public class MainWindowVM: ViewModelBase<Person>
    {
        public RelayCommand SubmitCommand { get; set; }

        public MainWindowVM()
        {
            Model = new Person();
            SubmitCommand = new RelayCommand(Submit);
        }

        private void Submit()
        {
            FocusByPropertyName(nameof(Model.CreateDate));
            var t = FocusManager.GetFocusedElement((DependencyObject)View);
        }
    }
}
