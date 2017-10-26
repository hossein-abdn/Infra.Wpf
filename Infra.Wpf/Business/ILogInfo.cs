using Infra.Wpf.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Wpf.Business
{
    public interface ILogInfo
    {
        DateTime CreatedDate { get; set; }

        string Message { get; set; }

        MessageType LogLevel { get; set; }

        int UserId { get; set; }
    }
}
