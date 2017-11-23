using DataAccess.Models;
using Infra.Wpf.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TestControls
{
    public class StudentVm : ViewModelBase<Student>
    {
        public RelayCommand GetStudentsCommand { get; set; }

        public RelayCommand LoadedCommand { get; set; }

        public StudentVm()
        {
            GetStudentsCommand = new RelayCommand(GetStudents);
            LoadedCommand = new RelayCommand(LoadedExec);
        }

        private void LoadedExec()
        {
            GetStudents();
        }

        private void GetStudents()
        {
            
            FBSDBContext context = new FBSDBContext();

            List<Student> list = context.Students.ToList();
            ItemsSource = new ObservableCollection<Student>(list);
        }
    }
}
