using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infra.Wpf.Controls;

namespace Infra.Wpf.Business
{
    public class LogInfo : ILogInfo
    {
        public DateTime CreatedDate { get; set; }

        public MessageType LogLevel { get; set; }

        public string Message { get; set; }

        public int UserId { get; set; }
    }
}
