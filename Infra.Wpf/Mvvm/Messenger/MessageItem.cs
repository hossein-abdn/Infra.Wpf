using System;

namespace Infra.Wpf.Mvvm
{
    public class MessageItem
    {
        public Type MessageType { get; set; }
        public object Token { get; set; }

        public IReferenceInvoker ReferenceInvoker { get; set; }

        public bool IsInherit { get; set; }
    }
}
