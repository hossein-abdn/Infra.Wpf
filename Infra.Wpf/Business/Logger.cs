using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Wpf.Business
{
    public abstract class Logger
    {
        public bool LogOnException { get; set; }

        public abstract void Log(ILogInfo logInfo);

        public void Log(Exception e, int userId = 0)
        {
            var logInfo = new LogInfo()
            {
                Message = e.Message,
                LogLevel = Controls.MessageType.Error,
                CreatedDate = DateTime.Now,
                UserId = userId
            };

            Log(logInfo);
        }
    }
}
