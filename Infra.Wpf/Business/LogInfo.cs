using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Infra.Wpf.Controls;

namespace Infra.Wpf.Business
{
    public class LogInfo : ILogInfo
    {
        private string callSite;
        public string CallSite
        {
            get
            {
                return callSite;
            }
            set
            {
                callSite = value;
                if (!string.IsNullOrEmpty(callSite) && callSite.Any(x => x == '`'))
                    callSite = callSite.Substring(0, callSite.IndexOf('`'));
            }
        }

        public Exception Exception { get; set; }

        public string Message { get; set; }

        public int UserId { get; set; }

        public DbEntityEntry Entry { get; set; }

        public LogType LogType { get; set; }
    }
}
