using System;
using System.Data.Entity.Infrastructure;

namespace Infra.Wpf.Common
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
