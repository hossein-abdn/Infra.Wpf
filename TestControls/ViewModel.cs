using DataAccess.Models;
using Infra.Wpf;
using Infra.Wpf.Common.Helpers;
using Infra.Wpf.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TestControls
{
    public class ViewModel:ViewModelBase<Student>
    {
        public RelayCommand<string> GetStudentsCommand { get; set; }

        public ViewModel()
        {
            GetStudentsCommand = new RelayCommand<string>(GetStudents);
        }

        private void GetStudents(string whereClause)
        {
            FBSDBContext context = new FBSDBContext();

            var predicate = DynamicLinq.ConvertToExpression<Student>(whereClause);
            List<Student> list = context.Students.Where(predicate).ToList();
            

            ItemsSource = new ObservableCollection<Student>(list);
        }

        public MyEnum? MyEnum
        {
            get { return Get<MyEnum?>(); }
            set { Set(value); }
        }

    }

    public enum MyEnum
    {
        [EnumDisplay("عدد 1")]
        num1,
        [EnumDisplay("عدد 2")]
        num2
    }
}
