using Infra.Wpf.Mvvm;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Infra.Wpf.Controls
{
    internal class WPFMessageBoxViewModel : ViewModelBase
    {
        public WPFMessageBoxViewModel(MsgPack messagePack)
        {
            CreateCommands();
            MessagePack = messagePack;
            CloseVisibility = Visibility.Visible;
            Message = MessagePack.Message;
            Caption = MessagePack.Caption;

            OKVisibility = Visibility.Collapsed;
            CancelVisibility = Visibility.Collapsed;
            YesNoVisibility = Visibility.Collapsed;
            switch (MessagePack.Button)
            {
                case MsgButton.OK:
                    OKVisibility = Visibility.Visible;
                    break;
                case MsgButton.OKCancel:
                    OKVisibility = Visibility.Visible;
                    CancelVisibility = Visibility.Visible;
                    break;
                case MsgButton.YesNo:
                    YesNoVisibility = Visibility.Visible;
                    CloseVisibility = Visibility.Collapsed;
                    break;
                case MsgButton.YesNoCancel:
                    YesNoVisibility = Visibility.Visible;
                    CancelVisibility = Visibility.Visible;
                    break;
            }

            switch (MessagePack.Icon)
            {
                case MsgIcon.Error:
                    Icon = new BitmapImage(new Uri("pack://application:,,,/Infra.Wpf;component/Controls/Resources/Error-45.png"));
                    break;
                case MsgIcon.Information:
                    Icon = new BitmapImage(new Uri("pack://application:,,,/Infra.Wpf;component/Controls/Resources/Information-45.png"));
                    break;
                case MsgIcon.Warning:
                    Icon = new BitmapImage(new Uri("pack://application:,,,/Infra.Wpf;component/Controls/Resources/Warning-45.png"));
                    break;
                case MsgIcon.Question:
                    Icon = new BitmapImage(new Uri("pack://application:,,,/Infra.Wpf;component/Controls/Resources/Question-45.png"));
                    break;
                case MsgIcon.None:
                    Icon = null;
                    break;
            }

            switch (MessagePack.DefaultResult)
            {
                case MsgResult.None:
                    if (MessagePack.Button == MsgButton.OK || MessagePack.Button == MsgButton.OKCancel)
                        DefaultOK = true;
                    if (MessagePack.Button == MsgButton.YesNo || MessagePack.Button == MsgButton.YesNoCancel)
                        DefaultYes = true;
                    break;
                case MsgResult.OK:
                    if (MessagePack.Button == MsgButton.YesNo || MessagePack.Button == MsgButton.YesNoCancel)
                        DefaultYes = true;
                    else
                        DefaultOK = true;
                    break;
                case MsgResult.Cancel:
                    if (MessagePack.Button == MsgButton.YesNo)
                        DefaultYes = true;
                    else
                        DefaultCancel = true;
                    break;
                case MsgResult.Yes:
                    if (MessagePack.Button == MsgButton.OK || MessagePack.Button == MsgButton.OKCancel)
                        DefaultOK = true;
                    else
                        DefaultYes = true;
                    break;
                case MsgResult.No:
                    if (MessagePack.Button == MsgButton.OK || MessagePack.Button == MsgButton.OKCancel)
                        DefaultOK = true;
                    else
                        DefaultNo = true;
                    break;
            }
        }

        #region [ Commands ]

        public RelayCommand OKCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }
        public RelayCommand YesCommand { get; private set; }
        public RelayCommand NoCommand { get; private set; }
        public RelayCommand CloseCommand { get; private set; }

        private void CreateCommands()
        {
            OKCommand = new RelayCommand(OKCommandExecute);
            CancelCommand = new RelayCommand(CancelCommandExecute);
            YesCommand = new RelayCommand(YesCommandExecute);
            NoCommand = new RelayCommand(NoCommandExecute);
            CloseCommand = new RelayCommand(CloseCommandExecute);
        }

        #endregion

        #region [ Properties ]

        public string Message
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        public string Caption
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        public ImageSource Icon
        {
            get { return Get<ImageSource>(); }
            set { Set(value); }
        }

        public bool DefaultOK
        {
            get { return Get<bool>(); }
            set { Set(value); }
        }

        public bool DefaultCancel
        {
            get { return Get<bool>(); }
            set { Set(value); }
        }

        public bool DefaultYes
        {
            get { return Get<bool>(); }
            set { Set(value); }
        }

        public bool DefaultNo
        {
            get { return Get<bool>(); }
            set { Set(value); }
        }

        public Visibility YesNoVisibility
        {
            get { return Get<Visibility>(); }
            set { Set(value); }
        }

        public Visibility OKVisibility
        {
            get { return Get<Visibility>(); }
            set { Set(value); }
        }

        public Visibility CancelVisibility
        {
            get { return Get<Visibility>(); }
            set { Set(value); }
        }

        public Visibility CloseVisibility
        {
            get { return Get<Visibility>(); }

            set { Set(value); }
        }

        private MsgResult Result;

        private MsgPack MessagePack;

        #endregion

        #region [ CommandExecute ]

        private void OKCommandExecute()
        {
            Result = MsgResult.OK;
            Messenger.Default.Send(Result);
        }

        private void CancelCommandExecute()
        {
            Result = MsgResult.Cancel;
            Messenger.Default.Send(Result);
        }

        private void YesCommandExecute()
        {
            Result = MsgResult.Yes;
            Messenger.Default.Send(Result);
        }

        private void NoCommandExecute()
        {
            Result = MsgResult.No;
            Messenger.Default.Send(Result);
        }

        private void CloseCommandExecute()
        {
            switch (MessagePack.Button)
            {
                case MsgButton.OK:
                    Result = MsgResult.OK;
                    break;
                case MsgButton.OKCancel:
                case MsgButton.YesNoCancel:
                    Result = MsgResult.Cancel;
                    break;
            }
            Messenger.Default.Send(Result);
        }

        #endregion
    }
}
