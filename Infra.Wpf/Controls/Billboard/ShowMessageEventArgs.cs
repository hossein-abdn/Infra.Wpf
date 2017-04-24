using System;
using System.Linq;

namespace Infra.Wpf.Controls
{
    public class ShowMessageEventArgs : EventArgs
    {
        public readonly String MsgText;
        public readonly MessageType MsgType;

        public ShowMessageEventArgs(MessageType msgType, String msgText) : base()
        {
            MsgText = msgText;
            MsgType = msgType;
        }
    }
}