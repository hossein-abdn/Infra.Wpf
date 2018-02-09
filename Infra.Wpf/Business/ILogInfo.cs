using Infra.Wpf.Controls;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Wpf.Business
{
    public interface ILogInfo
    {
        DbEntityEntry Entry { get; set; }

        LogType LogType { get; set; }

        string CallSite { get; set; }

        string Message { get; set; }

        Exception Exception { get; set; }

        int UserId { get; set; }
    }
}
