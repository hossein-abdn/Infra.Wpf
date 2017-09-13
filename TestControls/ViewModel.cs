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
    public class ViewModel:ViewModelBase<Product>
    {
        public ViewModel()
        {
            List<Product> _products = new List<Product>();
            for (int i = 0; i < 5; i++)
            {
                _products.Add(new Product());
            }

            ItemsSource = new ObservableCollection<Product>(_products);
        }

    }
}
