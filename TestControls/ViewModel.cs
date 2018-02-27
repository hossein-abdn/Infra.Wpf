using DataAccess.Models;
using Infra.Wpf.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestControls
{
    public class ViewModel: ViewModelBase<Person>
    {
        public ViewModel()
        {
            Model = new Person();
        }
    }
}
