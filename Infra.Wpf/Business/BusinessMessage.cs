﻿using Infra.Wpf.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Wpf.Business
{
    public class BusinessMessage
    {
        public string Title { get; set; }

        public string Message { get; set; }

        public MessageType MessageType { get; set; }

        public BusinessMessage(string title, string message, MessageType messageType = MessageType.Error)
        {
            Title = title;
            Message = message;
            MessageType = messageType;
        }
    }
}
