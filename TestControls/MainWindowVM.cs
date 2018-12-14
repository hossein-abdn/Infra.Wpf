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

        public List<User> UserList { get; set; }

        AccountingContext context = new AccountingContext();

        public MainWindowVM()
        {
            SubmitCommand = new RelayCommand(SubmitExecute);
            SearchCommand = new RelayCommand<string>(SearchExecute);

            ViewTitle = "افزودن شخص";
            Model = new Person();

            UserList = context.Users.ToList();

            Model.AddRule(x => x.UserList.Count).GreaterThan(0).OverridePropertyName("UserList").WithMessage("حداقل یک کاربر انتخاب کنید.");
        }

        private void SubmitExecute()
        {
            var result = Model.Validate();
            if (Model.HasErrors)
            {
                MessageBox.Show(result.Errors[0].ErrorMessage);
                FocusByPropertyName(result.Errors[0].PropertyName);
            }
        }

        private void SearchExecute(string obj)
        {

        }
    }
}
