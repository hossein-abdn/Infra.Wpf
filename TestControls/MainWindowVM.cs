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
using System.Linq.Expressions;
using DataAccess.Enums;
using Infra.Wpf.Repository;

namespace TestControls
{
    public class MainWindowVM : ViewModelBase
    {
        public RelayCommand<List<KeyValuePair<string, string>>> GetAllCommand { get; set; }

        public RelayCommand SubmitCommand { get; set; }

        public Label Model
        {
            get { return Get<Label>(); }
            set { Set(value); }
        }

        TransactionGroupRepository transactionGroupRepository;

        AccountingContext context = new AccountingContext();

        public MainWindowVM()
        {
            GetAllCommand = new RelayCommand<List<KeyValuePair<string, string>>>(GetAllExecute);
            SubmitCommand = new RelayCommand(SubmitExecute);
            transactionGroupRepository = new TransactionGroupRepository(context, null, false);

            ViewTitle = "افزودن شخص";
            Model = new Label();
            Model.Title = "";
        }

        private void SubmitExecute()
        {
            int i = 1;
        }

        private void GetAllExecute(List<KeyValuePair<string, string>> filterList)
        {
            
        }
    }
}
