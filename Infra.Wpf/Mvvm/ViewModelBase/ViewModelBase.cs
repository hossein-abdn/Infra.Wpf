using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Threading;
using System.Windows;
using Infra.Wpf.Controls;
using System.Collections.ObjectModel;

namespace Infra.Wpf.Mvvm
{
    public class ViewModelBase : INotifyPropertyChanged, IViewModelBase
    {
        protected Dictionary<string, object> members = new Dictionary<string, object>();

        public NavigationService NavigationService { get; set; }

        public Billboard Billboard { get; set; }

        public Dispatcher Dispatcher { get; set; }

        public object View { get; set; }

        public string ViewTitle
        {
            get { return Get<string>(); }
            set
            {
                Set(value);
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public void Set<T>(T value, [CallerMemberName]String prop = null)
        {
            members[prop] = value;
            OnPropertyChanged(prop);
        }

        public T Get<T>([CallerMemberName]String prop = null)
        {
            object val;
            if (members.TryGetValue(prop, out val))
                return (T) val;

            return default(T);
        }

        public MsgResult ShowMessageBox(string message, string caption = null, MsgButton button = MsgButton.OK,
            MsgIcon icon = MsgIcon.None, MsgResult defaultResult = MsgResult.None, Window owner = null)
        {
            return WPFMessageBox.Show(message, caption, button, icon, defaultResult, owner);
        }
    }

    public class ViewModelBase<T> : ViewModelBase where T : class
    {
        public ObservableCollection<T> ItemsSource
        {
            get { return Get<ObservableCollection<T>>(); }
            set { Set(value); }
        }

        public object Item
        {
            get { return Get<object>(); }
            set { Set(value); }
        }
    }
}