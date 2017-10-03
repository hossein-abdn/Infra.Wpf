using DataAccess.Models;
using Infra.Wpf.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestControls
{
    public class StudentVm : ViewModelBase<Student>
    {
        public RelayCommand GetStudentsCommand { get; set; }

        public StudentVm()
        {
            GetStudentsCommand = new RelayCommand(GetStudents);
        }

        private void GetStudents()
        {
            FBSDBContext context = new FBSDBContext();

            List<Student> list = context.Students.ToList();
            ItemsSource = new ObservableCollection<Student>(list);
        }
    }
}
