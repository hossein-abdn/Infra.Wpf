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
    public class ParentViewModel : ViewModelBase
    {
        public RelayCommand<string> GetParentsCommand { get; set; }

        public ObservableCollection<Parent> ItemsSource
        {
            get { return Get<ObservableCollection<Parent>>(); }
            set { Set(value); }
        }

        public ObservableCollection<Student> StudentList
        {
            get { return Get<ObservableCollection<Student>>(); }
            set { Set(value); }
        }


        public ParentViewModel()
        {
            GetParentsCommand = new RelayCommand<string>(GetParents);

            StudentVm studentVm = new StudentVm();
            studentVm.GetStudentsCommand.Execute(null);
            StudentList = studentVm.ItemsSource;
        }

        //private void GetParents(string whereClause)
        //{
        //    FBSDBContext context = new FBSDBContext();

        //    var predicate = DynamicLinq.ConvertToExpression<GeneralBaseType>(whereClause);
        //    List<GeneralBaseType> list = context.GeneralBaseTypes.Where(predicate).ToList();

        //    ItemsSource = new ObservableCollection<GeneralBaseType>(list);
        //}

        private void GetParents(string whereClause)
        {
            FBSDBContext context = new FBSDBContext();

            var predicate = DynamicLinq.ConvertToExpression<Parent>(whereClause);
            List<Parent> list = context.Parents.Where(predicate).ToList();

            ItemsSource = new ObservableCollection<Parent>(list);
        }
    }

    public enum RecordStatus
    {
        علی = 3,
        محسن = 4
    }
}
