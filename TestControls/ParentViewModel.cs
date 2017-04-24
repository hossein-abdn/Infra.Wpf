using DataAccess.Models;
using Infra.Wpf.Common.Helpers;
using Infra.Wpf.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestControls
{
    public class ParentViewModel:ViewModelBase
    {
        public RelayCommand<string> GetParentsCommand { get; set; }

        public ObservableCollection<GeneralBaseType> ItemsSource
        {
            get { return Get<ObservableCollection<GeneralBaseType>>(); }
            set { Set(value); }
        }

        public ParentViewModel()
        {
            GetParentsCommand = new RelayCommand<string>(GetParents);
        }

        private void GetParents(string whereClause)
        {
            FBSDBContext context = new FBSDBContext();

            var predicate = DynamicLinq.ConvertToExpression<GeneralBaseType>(whereClause);
            List<GeneralBaseType> list = context.GeneralBaseTypes.Where(predicate).ToList();

            ItemsSource = new ObservableCollection<GeneralBaseType>(list);
        }

        //private void GetParents(string whereClause)
        //{
        //    FBSDBContext context = new FBSDBContext();

        //    var predicate = DynamicLinq.ConvertToExpression<Parent>(whereClause);
        //    List<Parent> list = context.Parents.Where(predicate).ToList();

        //    ItemsSource = new ObservableCollection<Parent>(list);
        //}
    }
}
