using System;
using System.Reflection;

namespace Infra.Wpf.Mvvm
{
    public class WeakReferenceInvoker<MsgType> : IReferenceInvoker
    {
        private WeakReference weakInstance = null;

        private MethodInfo actionMethod { get; set; }

        public WeakReferenceInvoker(Action<MsgType> action)
        {
            weakInstance = action.Target == null ? null : new WeakReference(action.Target);
            actionMethod = action.Method;
        }

        public object Instance
        {
            get
            {
                return weakInstance?.Target;
            }
        }

        public string MethodName
        {
            get
            {
                return actionMethod.Name;
            }
        }

        public void Clean()
        {
            weakInstance = null;
            actionMethod = null;
        }

        public void Execute(object parameter)
        {
            if (actionMethod != null && weakInstance.IsAlive)
                actionMethod.Invoke(weakInstance.Target, new object[] { (MsgType) parameter });
        }
    }
}
