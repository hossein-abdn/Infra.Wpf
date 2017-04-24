using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Infra.Wpf.Mvvm
{
    public class Messenger
    {
        private static Messenger _Default;
        private readonly static object objlock = new object();
        private bool IsThreadSafe;
        private ReferenceType ReferenceType;
        private List<MessageItem> messageCollection = new List<MessageItem>();

        public Messenger() : this(false)
        {
        }

        public Messenger(bool isThreadSafe, ReferenceType refrenceType = ReferenceType.WeakReference)
        {
            IsThreadSafe = isThreadSafe;
            ReferenceType = refrenceType;
        }

        public static Messenger Default
        {
            get
            {
                if (_Default == null)
                {
                    lock (objlock)
                    {
                        if (_Default == null)
                            _Default = new Messenger();
                    }
                }
                return _Default;
            }
        }

        public void Register<MsgType>(Action<MsgType> action, object token = null, bool isInherit = false)
        {
            try
            {
                if (IsThreadSafe)
                    Monitor.Enter(messageCollection);

                MessageItem message = new MessageItem
                {
                    MessageType = typeof(MsgType),
                    Token = token,
                    IsInherit = isInherit
                };

                if (ReferenceType == ReferenceType.StrongReference || action.Method.IsStatic)
                    message.ReferenceInvoker = new StrongReferenceInvoker<MsgType>(action);
                else
                    message.ReferenceInvoker = new WeakReferenceInvoker<MsgType>(action);

                messageCollection.Add(message);
            }
            finally
            {
                if (IsThreadSafe)
                    Monitor.Exit(messageCollection);
            }
        }

        public void Unregister<MsgType>(string actionName, object token = null)
        {
            var list = messageCollection.Where(x => (x.MessageType == typeof(MsgType) && x.Token.Equals(token)) ||
            (x.IsInherit == true && x.MessageType.IsAssignableFrom(typeof(MsgType)) && x.Token.Equals(token))).ToList();

            if (string.IsNullOrEmpty(actionName) == false)
                list = list.Where(x => x.ReferenceInvoker.MethodName == actionName).ToList();

            foreach (var item in list)
            {
                messageCollection.Remove(item);
            }
        }

        public void Send<MsgType>(MsgType message, Type targetType, object token = null)
        {
            try
            {
                if (IsThreadSafe)
                    Monitor.Enter(messageCollection);
                
                var list = messageCollection.Where(x => (x.MessageType == targetType && object.Equals(x.Token, token)) ||
                (x.IsInherit == true && x.MessageType.IsAssignableFrom(targetType) && object.Equals(x.Token, token)));
                foreach (var item in list)
                    item.ReferenceInvoker.Execute(message);
            }
            finally
            {
                if (IsThreadSafe)
                    Monitor.Exit(messageCollection);
            }
        }

        public void Send<MsgType>(MsgType message, object token = null)
        {
            Send(message, typeof(MsgType), token);
        }
    }
}
