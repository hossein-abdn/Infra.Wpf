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

        TransactionGroupRepository transactionGroupRepository;

        AccountingContext context = new AccountingContext();

        public System.Collections.ObjectModel.ObservableCollection<TransactionGroupTypeNode> ItemsSource
        {
            get { return Get<System.Collections.ObjectModel.ObservableCollection<TransactionGroupTypeNode>>(); }
            set { Set(value); }
        }

        public System.Collections.ObjectModel.ObservableCollection<TransactionGroup> FlatItemsSource
        {
            get { return Get<System.Collections.ObjectModel.ObservableCollection<TransactionGroup>>(); }
            set { Set(value); }
        }

        public MainWindowVM()
        {
            GetAllCommand = new RelayCommand<List<KeyValuePair<string, string>>>(GetAllExecute);
            SubmitCommand = new RelayCommand(SubmitExecute);
            transactionGroupRepository = new TransactionGroupRepository(context, null, false);

            ViewTitle = "افزودن شخص";

            GetAllExecute(null);
        }

        private void SubmitExecute()
        {
            int i = 1;
        }

        private string GeneratePredicate(List<KeyValuePair<string, string>> filterList)
        {
            string result = "";
            if (filterList == null)
                return result;

            foreach (var item in filterList)
            {
                if (!string.IsNullOrWhiteSpace(item.Value))
                {
                    if (!string.IsNullOrEmpty(result))
                        result += " AND ";

                    if (item.Key == "TypeId")
                        result += "TypeId == @1";
                    else
                        result += item.Value;
                }
            }

            return result;
        }

        private void GetAllExecute(List<KeyValuePair<string, string>> filterList)
        {
            var predicate = GeneratePredicate(filterList);
            object[] values = null;

            var typeId = filterList?.FirstOrDefault(x => x.Key == "TypeId");
            if (typeId != null && typeId.HasValue)
            {
                var typeIdString = typeId.Value.Value;
                if (!string.IsNullOrEmpty(typeIdString))
                {
                    var typeIdValue = Enum.Parse(typeof(TransactionGroupType), typeIdString.Substring(typeIdString.IndexOf("=") + 2));
                    values = new object[] { typeIdValue };
                }
            }

            var transactionGroupListResult = transactionGroupRepository.GetAll(predicate, values);
            FlatItemsSource = new ObservableCollection<TransactionGroup>(transactionGroupListResult);

            var costNode = new TransactionGroupTypeNode()
            {
                Type = TransactionGroupType.Cost,
                Members = FillTransactionGroupList(transactionGroupListResult, null, TransactionGroupType.Cost, predicate.Contains("Title")),
                IsExpanded = true
            };
            var IncomeNode = new TransactionGroupTypeNode()
            {
                Type = TransactionGroupType.Income,
                Members = FillTransactionGroupList(transactionGroupListResult, null, TransactionGroupType.Income, predicate.Contains("Title")),
                IsExpanded = true
            };
            ItemsSource = new System.Collections.ObjectModel.ObservableCollection<TransactionGroupTypeNode>() { costNode, IncomeNode };
        }

        private List<TransactionGroupNode> FillTransactionGroupList(List<TransactionGroup> list, int? parentId, TransactionGroupType type, bool isExpanded)
        {
            List<TransactionGroupNode> result = null;
            foreach (var item in list.Where(x => x.TypeId == type && x.ParentId == parentId))
            {
                if (result == null)
                    result = new List<TransactionGroupNode>();
                result.Add(new TransactionGroupNode
                {
                    Item = item,
                    Members = FillTransactionGroupList(list, item.TransactionGroupId, type, isExpanded),
                    IsExpanded = isExpanded
                });
            }

            return result;
        }
    }
}
