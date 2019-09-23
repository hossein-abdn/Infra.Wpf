using System;
using System.Threading;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Infra.Wpf.Mvvm;
using System.Windows.Threading;
using System.Windows;
using Infra.Wpf.Common;

namespace Infra.Wpf.Controls
{
    internal class BillboardViewModel : ViewModelBase
    {
        public BillboardViewModel()
        {
            CreateCommands();
            Dispatcher = Application.Current.Dispatcher;
        }

        #region [ Commands ]

        public RelayCommand<ArgsParameter<ShowMessageEventArgs>> ShowMessageCommand { get; private set; }

        public RelayCommand ClearMessageCommand { get; private set; }

        private void CreateCommands()
        {
            ShowMessageCommand = new RelayCommand<ArgsParameter<ShowMessageEventArgs>>(ShowMessageExecute);
            ClearMessageCommand = new RelayCommand(ClearMessageExecute);
        }

        #endregion

        #region [ Properties ]

        public string TextMessage
        {
            get { return Get<string>(); }
            set { Set(value); }
        }

        public ImageSource ImageMessage
        {
            get { return Get<ImageSource>(); }
            set { Set(value); }
        }

        public Brush ColorMessage
        {
            get { return Get<Brush>(); }
            set { Set(value); }
        }

        private string Title { get; set; }

        #endregion

        #region [ CommandExecute ]

        private void ClearMessageExecute()
        {
            TextMessage = string.IsNullOrEmpty(Title) ? string.Empty : Title;

            ColorMessage = string.IsNullOrEmpty(Title) ? null : Brushes.Blue;

            ImageMessage = null;
        }

        private void ShowMessageExecute(ArgsParameter<ShowMessageEventArgs> msg)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(ShowMessageThread), msg.e);
        }

        int iThreadCount = 0;

        object Token = new object();

        private void ShowMessageThread(object Parameter)
        {
            ShowMessageEventArgs msgArgs = Parameter as ShowMessageEventArgs;

            if (msgArgs.MsgType != MessageType.Title)
            {
                iThreadCount++;
                if (iThreadCount > 3)
                {
                    iThreadCount--;
                    return;
                }
            }

            lock (Token)
            {
                if (msgArgs == null)
                    return;
                Dispatcher.BeginInvoke((Action) (() =>
                 {
                     TextMessage = msgArgs.MsgText;

                     switch (msgArgs.MsgType)
                     {
                         case MessageType.Information:
                             ImageMessage = new BitmapImage(new Uri("pack://application:,,,/Infra.Wpf;component/Controls/Resources/Information-32.png"));
                             ColorMessage = Brushes.Blue;
                             break;
                         case MessageType.Warning:
                             ImageMessage = new BitmapImage(new Uri("pack://application:,,,/Infra.Wpf;component/Controls/Resources/Warning-32.png"));
                             ColorMessage = Brushes.DarkGoldenrod;
                             break;
                         case MessageType.Error:
                             ImageMessage = new BitmapImage(new Uri("pack://application:,,,/Infra.Wpf;component/Controls/Resources/Error-32.png"));
                             ColorMessage = Brushes.DarkRed;
                             break;
                         case MessageType.None:
                             ImageMessage = null;
                             ColorMessage = Brushes.Blue;
                             break;
                         case MessageType.Title:
                             ImageMessage = null;
                             ColorMessage = Brushes.Blue;
                             Title = msgArgs.MsgText;
                             break;
                     }
                 }));

                if (msgArgs.MsgType != MessageType.Title)
                {
                    Thread.Sleep(3000);
                    if (iThreadCount == 1)
                        ClearMessageCommand.Execute(null);
                    iThreadCount--;
                }
            }
        }

        #endregion
    }
}