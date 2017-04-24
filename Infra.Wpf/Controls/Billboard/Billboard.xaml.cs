using System;
using System.Windows;
using System.Windows.Controls;

namespace Infra.Wpf.Controls
{
    public partial class Billboard : UserControl
    {
        public delegate void ShowMessageHandler(object sender, ShowMessageEventArgs msg);

        public event ShowMessageHandler ShowMessageEvent;

        public Billboard()
        {
            InitializeComponent();
        }

        public void ShowMessage(MessageType msgType, String strMessage)
        {
            ShowMessageEventArgs msgArgs = new ShowMessageEventArgs(msgType, strMessage);

            ShowMessageEvent?.Invoke(this.Parent, msgArgs);
        }
    }
}