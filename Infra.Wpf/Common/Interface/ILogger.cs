using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Wpf.Common
{
    public interface ILogger
    {
        List<ILogInfo> LogList { get; set; }

        void Log(ILogInfo logInfo);

        void Log(Exception ex, string callSite, int userId);

        void LogPendingList();
    }
}
