using Infra.Wpf.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Enums
{
    public enum TransactionGroupType
    {
        [EnumDisplay("هزینه")]
        Cost = 1,
        [EnumDisplay("درآمد")]
        Income = 2
    }
}
