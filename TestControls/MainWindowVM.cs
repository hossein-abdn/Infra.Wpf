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


        public ObservableCollection<User> UserList
        {
            get { return Get<ObservableCollection<User>>(); }
            set { Set(value); }
        }

        public ObservableCollection<string> UserList1
        {
            get { return Get<ObservableCollection<string>>(); }
            set { Set(value); }
        }

        public ObservableCollection<Person> PersonList
        {
            get { return Get<ObservableCollection<Person>>(); }
            set { Set(value); }
        }

        AccountingContext context = new AccountingContext();

        public MainWindowVM()
        {
            SubmitCommand = new RelayCommand(SubmitExecute);
            SearchCommand = new RelayCommand<string>(SearchExecute);

            ViewTitle = "افزودن شخص";
            Model = new Person();

            UserList = new ObservableCollection<User>();
            UserList = new ObservableCollection<User>(context.Users.ToList());

            UserList1 = new ObservableCollection<string> { "test1", "test2" };
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
            if (string.IsNullOrEmpty(obj))
                UserList = new ObservableCollection<User>(context.Users.ToList());
            else
                UserList = new ObservableCollection<User>(context.Users.Where(obj).ToList());
        }
    }
}
