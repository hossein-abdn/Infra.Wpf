using Infra.Wpf.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public enum RecordStatus
    {
        [EnumDisplay("تأیید")]
        Confirm = 3,
        [EnumDisplay("حذف شده")]
        Deleted = 4
    }

    public enum BasijStatus
    {
        [EnumDisplay("فعال")]
        Active = 5,
        [EnumDisplay("غیر فعال")]
        Deactive = 6
    }
}
