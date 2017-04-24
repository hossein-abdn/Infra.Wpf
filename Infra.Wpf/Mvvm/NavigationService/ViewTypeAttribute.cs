using System;

namespace Infra.Wpf.Mvvm
{
    public class ViewTypeAttribute:Attribute
    {
        public ViewTypeAttribute(Type pageType)
        {
            PageType = pageType;
        }

        public Type PageType { get; }
    }
}
