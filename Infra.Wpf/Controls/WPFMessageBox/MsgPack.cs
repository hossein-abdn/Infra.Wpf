namespace Infra.Wpf.Controls
{
    public class MsgPack
    {
        public readonly string Message;
        public readonly string Caption;
        public readonly MsgButton Button;
        public readonly MsgIcon Icon;
        public readonly MsgResult DefaultResult;

        public MsgPack(string message, string caption, MsgButton button, MsgIcon icon, MsgResult defaultResult)
        {
            Message = message;
            Caption = caption;
            Button = button;
            Icon = icon;
            DefaultResult = defaultResult;
        }
    }
}