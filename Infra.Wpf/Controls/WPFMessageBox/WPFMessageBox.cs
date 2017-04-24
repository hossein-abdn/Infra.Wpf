using Infra.Wpf.Mvvm;
using System.Media;
using System.Windows;

namespace Infra.Wpf.Controls
{
    static public class WPFMessageBox
    {
        private static MsgResult Result;
        private static WPFMessageBoxControl msgBox;
             
        static WPFMessageBox()
        {
            Messenger.Default.Register<MsgResult>(OnClose);
        }

        public static MsgResult Show(string message, string caption = null, MsgButton button = MsgButton.OK,
            MsgIcon icon = MsgIcon.None, MsgResult defaultResult = MsgResult.None, Window owner = null)
        {
            msgBox = new WPFMessageBoxControl(owner);
            MsgPack messagePack = new MsgPack(message, caption, button, icon, defaultResult);
            WPFMessageBoxViewModel viewmodel = new WPFMessageBoxViewModel(messagePack);
            msgBox.DataContext = viewmodel;
            PlayMessageBeep(icon);
            msgBox.ShowDialog();
            return Result;
        }

        private static void PlayMessageBeep(MsgIcon icon)
        {
            switch (icon)
            {
                case MsgIcon.Error:
                    SystemSounds.Hand.Play();
                    break;
                case MsgIcon.Warning:
                    SystemSounds.Exclamation.Play();
                    break;
                case MsgIcon.Question:
                    SystemSounds.Question.Play();
                    break;
                case MsgIcon.Information:
                    SystemSounds.Asterisk.Play();
                    break;
            }
        }

        private static void OnClose(MsgResult result)
        {
            Result = result;
            msgBox.Close();
        }
    }
}