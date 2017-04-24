using System;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Infra.Wpf.Mvvm
{
    public class EventToCommand : Behavior<DependencyObject>
    {
        public string EventName
        {
            get { return (string) GetValue(EventNameProperty); }
            set { SetValue(EventNameProperty, value); }
        }

        public static readonly DependencyProperty EventNameProperty =
            DependencyProperty.Register("EventName", typeof(string), typeof(EventToCommand), new PropertyMetadata(string.Empty));

        public ICommand Command
        {
            get { return (ICommand) GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(EventToCommand), new PropertyMetadata(null));

        public bool IsPassArgs
        {
            get { return (bool) GetValue(IsPassArgsProperty); }
            set { SetValue(IsPassArgsProperty, value); }
        }

        public static readonly DependencyProperty IsPassArgsProperty =
            DependencyProperty.Register("IsPassArgs", typeof(bool), typeof(EventToCommand), new PropertyMetadata(false));

        private Delegate eventHandler;

        protected override void OnAttached()
        {
            SubscribeToEvent();
            base.OnAttached();
        }

        protected override void OnDetaching()
        {
            UnsubscribeFromEvent();
            base.OnDetaching();
        }

        private void SubscribeToEvent()
        {
            if (AssociatedObject == null || string.IsNullOrWhiteSpace(EventName))
                return;

            var eventInfo = AssociatedObject.GetType().GetEvent(EventName);
            if (eventInfo == null)
                return;

            ParameterInfo pi = eventInfo.EventHandlerType.GetMethod("Invoke").GetParameters()[1];
            MethodInfo method = this.GetType().GetMethod("ExecuteCommand",BindingFlags.Instance|BindingFlags.NonPublic);
            MethodInfo generic = method.MakeGenericMethod(pi.ParameterType);

            eventHandler = Delegate.CreateDelegate(eventInfo.EventHandlerType, this, generic, false);

            if (eventHandler == null)
                return;

            eventInfo.AddEventHandler(AssociatedObject, eventHandler);
        }

        private void UnsubscribeFromEvent()
        {
            if (AssociatedObject == null || string.IsNullOrWhiteSpace(EventName))
                return;

            if (eventHandler == null)
                return;

            var info = AssociatedObject.GetType().GetEvent(EventName);

            info.RemoveEventHandler(AssociatedObject, eventHandler);
            eventHandler = null;
        }

        private void ExecuteCommand<TArgs>(object sender, TArgs e) where TArgs : EventArgs
        {
            if (Command == null)
                return;

            if (IsPassArgs)
                Command.Execute(new ArgsParameter<TArgs> { sender = sender, e = e });
            else
                Command.Execute(null);
        }
    }
}
