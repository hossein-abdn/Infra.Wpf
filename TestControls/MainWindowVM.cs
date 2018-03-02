using DataAccess.Models;
using Infra.Wpf.Common.Helpers;
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
        public string SearchPhrase
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        public RelayCommand<string> SubmitCommand { get; set; }

        public MainWindowVM()
        {
            Model = new Person();
            SubmitCommand = new RelayCommand<string>(Submit);
        }

        private void Submit(string predicate)
        {
            var context = new AccountingContext();

            var result = context.People.Where(predicate).ToList();
        }
    }
}
