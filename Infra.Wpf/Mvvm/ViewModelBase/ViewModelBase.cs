using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Threading;
using System.Windows;
using Infra.Wpf.Controls;
using System.Collections.ObjectModel;
using Infra.Wpf.Repository;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;

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
            set { Set(value); }
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

        public void FocusByName(string name, bool isDropdownOpen = false)
        {
            var view = View as FrameworkElement;

            if (view != null)
            {
                var obj = view.FindName(name) as UIElement;
                if (obj != null)
                {
                    obj?.Focus();
                    obj?.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
                    if (obj is TextBox)
                        (obj as TextBox).SelectAll();
                    if (obj is ComboBox && isDropdownOpen)
                        (obj as ComboBox).IsDropDownOpen = true;
                    if (obj is MultiSelect && isDropdownOpen)
                        (obj as MultiSelect).IsOpen = true;
                    if (obj is PasswordBox)
                        (obj as PasswordBox).SelectAll();
                }
            }
        }

        public void FocusByPropertyName(string propName, bool isDropdownOpen = false)
        {
            var view = View as UIElement;
            if (view != null)
            {
                var obj = FindObject(view, propName);
                if (obj != null)
                {
                    obj?.Focus();
                    obj?.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
                    if (obj is TextBox)
                        (obj as TextBox).SelectAll();
                    if (obj is ComboBox && isDropdownOpen)
                        (obj as ComboBox).IsDropDownOpen = true;
                    if (obj is MultiSelect && isDropdownOpen)
                        (obj as MultiSelect).IsOpen = true;
                }
            }
        }

        private UIElement FindObject(DependencyObject element, string propName)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
            {
                var item = VisualTreeHelper.GetChild(element, i);
                if (item is UIElement)
                {
                    var bindEx = GetBindingExpression(item as UIElement, propName);
                    if (bindEx != null)
                        return (UIElement) item;
                }

                if (item is DependencyObject)
                {
                    var result = FindObject(item as DependencyObject, propName);
                    if (result != null)
                        return result;
                }
            }

            return null;
        }

        private BindingExpression GetBindingExpression(UIElement element, string propName)
        {
            if (element == null)
                return null;

            DependencyProperty prop = null;

            if (element is TextBox)
                prop = TextBox.TextProperty;
            else if (element is TextField)
                prop = TextField.TextProperty;
            else if (element is ComboBox)
                prop = ComboBox.SelectedItemProperty;
            else if (element is RadioButton)
                prop = RadioButton.IsCheckedProperty;
            else if (element is PersianDatePicker)
                prop = PersianDatePicker.SelectedDateProperty;
            else if (element is NumericBox)
                prop = NumericBox.ValueProperty;
            else if (element is NumericField)
                prop = NumericField.ValueProperty;
            else if (element is MultiSelect)
                prop = MultiSelect.SelectedItemsProperty;
            else if (element is Lookup)
                prop = Lookup.SelectedItemsProperty;
            else if (element is DateField)
                prop = DateField.SelectedDateProperty;
            else if (element is CheckBox)
                prop = CheckBox.IsCheckedProperty;
            else if (element is BoolField)
                prop = BoolField.IsCheckedProperty;
            else
                return null;

            var bindEx = BindingOperations.GetBindingExpression(element, prop);
            if (bindEx != null && bindEx.ResolvedSourcePropertyName == propName)
                return bindEx;
            else
                return null;
        }
    }

    public class ViewModelBase<T> : ViewModelBase where T : class
    {
        public ObservableCollection<T> ItemsSource
        {
            get { return Get<ObservableCollection<T>>(); }
            set { Set(value); }
        }

        public T Model
        {
            get { return Get<T>(); }
            set { Set(value); }
        }
    }
}