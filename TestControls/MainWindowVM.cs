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
using FluentValidation;
using System.Collections.ObjectModel;
using Infra.Wpf.Common.Helpers;
using System.Collections;

namespace TestControls
{
    public class MainWindowVM : ViewModelBase<Person>
    {
        public RelayCommand SubmitCommand { get; set; }

        public RelayCommand<string> SearchCommand { get; set; }

        public float Digit
        {
            get { return Get<float>(); }
            set { Set(value); }
        }

        AccountingContext context = new AccountingContext();

        public MainWindowVM()
        {
            SubmitCommand = new RelayCommand(SubmitExecute);
            SearchCommand = new RelayCommand<string>(SearchExecute);

            ViewTitle = "افزودن شخص";
            Model = new Person();
        }

        private void SubmitExecute()
        {
            
        }

        private void SearchExecute(string obj)
        {

        }
    }
}
