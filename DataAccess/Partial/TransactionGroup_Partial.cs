using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infra.Wpf.Common;

namespace DataAccess.Models
{
    public partial class TransactionGroup : ISelectable
    {
        [NotMapped]
        public bool IsSelected { get { return Get<bool>(); } set { Set(value); } }
    }
}
