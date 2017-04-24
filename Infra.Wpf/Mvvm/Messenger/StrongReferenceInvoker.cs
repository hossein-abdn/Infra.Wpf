using System;

namespace Infra.Wpf.Mvvm
{

    public class StrongReferenceInvoker<MsgType> : IReferenceInvoker
    {
        private WeakReference weakInstance = null;

        private Action<MsgType> actionToInvoke { get; set; }

        public string MethodName
        {
            get
            {
                return actionToInvoke.Method.Name;
            }
        }


        public StrongReferenceInvoker(Action<MsgType> action)
        {
            weakInstance = action.Target == null ? null : new WeakReference(action.Target);
            actionToInvoke = action;
        }

        public object Instance
        {
            get
            {
                return weakInstance?.Target;
            }
        }

        public void Clean()
        {
            actionToInvoke = null;
            weakInstance = null;
        }

        public void Execute(object parameter)
        {
            if (Instance != null || actionToInvoke.Method.IsStatic)
                actionToInvoke((MsgType)parameter);
        }
    }
}
