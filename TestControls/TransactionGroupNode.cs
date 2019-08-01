using DataAccess.Models;
using Infra.Wpf.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestControls
{
    public class TransactionGroupNode:ViewModelBase
    {
        public TransactionGroup Item
        {
            get { return Get<TransactionGroup>(); }
            set { Set(value); }
        }

        public List<TransactionGroupNode> Members
        {
            get { return Get<List<TransactionGroupNode>>(); }
            set { Set(value); }
        }

        public bool IsExpanded
        {
            get { return Get<bool>(); }
            set { Set(value); }
        }
    }
}
