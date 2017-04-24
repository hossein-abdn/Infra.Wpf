using Infra.Wpf.Controls;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Infra.Wpf.Mvvm
{
    public class NavigationService
    {
        private Frame NavigationFrame { get; set; }

        private Billboard Billboard { get; set; }

        public NavigationService()
        {
            NavigationFrame = FindObject<Frame>(Application.Current.MainWindow);
            Billboard = FindObject<Billboard>(Application.Current.MainWindow);
        }

        public NavigationService(Frame navigationFrame, Billboard billboard = null)
        {
            NavigationFrame = navigationFrame;
            Billboard = billboard;
        }

        private T FindObject<T>(DependencyObject current) where T : DependencyObject
        {
            foreach (var item in LogicalTreeHelper.GetChildren(current))
            {
                if (item.GetType() == typeof(T))
                    return (T) item;

                if (item is DependencyObject)
                {
                    var result = FindObject<T>((DependencyObject) item);
                    if (result != null)
                        return result;
                }
            }

            return null;
        }

        public void NavigateTo(Uri uri)
        {
            NavigationFrame?.Navigate(uri);
        }

        public void NavigateTo(IViewModelBase viewmodel)
        {
            var view = GetView(viewmodel);
            if (view != null)
                NavigationFrame?.Navigate(view);
        }

        private Page GetView(IViewModelBase viewmodel)
        {
            var attrib = viewmodel.GetType().GetCustomAttributes(typeof(ViewTypeAttribute), false).FirstOrDefault() as ViewTypeAttribute;
            if (attrib != null && attrib.PageType != null)
            {
                var page = (Page) Activator.CreateInstance(attrib.PageType);
                FillViewModel(viewmodel, page);
                page.DataContext = viewmodel;
                return page;
            }

            return null;
        }

        private void FillViewModel(IViewModelBase viewmodel, Page view)
        {
            viewmodel.NavigationService = this;
            viewmodel.Dispatcher = Application.Current.MainWindow.Dispatcher;
            viewmodel.View = view;
            viewmodel.Billboard = Billboard;
        }

        public void GoBack()
        {
            if (NavigationFrame?.CanGoBack ?? false)
            {
                NavigationFrame.GoBack();
            }
        }
    }
}
