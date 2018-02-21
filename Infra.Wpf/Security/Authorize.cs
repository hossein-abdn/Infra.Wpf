using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;

namespace Infra.Wpf.Security
{
    public static class Authorize
    {
        public static AuthorizeInfo GetAuthorizeInfo(DependencyObject obj)
        {
            return (AuthorizeInfo) obj.GetValue(AuthorizeInfoProperty);
        }

        public static void SetAuthorizeInfo(DependencyObject obj, AuthorizeInfo value)
        {
            obj.SetValue(AuthorizeInfoProperty, value);
        }

        public static readonly DependencyProperty AuthorizeInfoProperty =
            DependencyProperty.RegisterAttached("AuthorizeInfo", typeof(AuthorizeInfo), typeof(AuthorizeInfo), new PropertyMetadata(null, OnAuthorizeInfoChanged));

        private static void OnAuthorizeInfoChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var behaviorList = Interaction.GetBehaviors(d);

            if (e.NewValue != null && !behaviorList.Any(x => x.GetType() == typeof(AuthorizeBehavior)))
                behaviorList.Add(new AuthorizeBehavior(e.NewValue as AuthorizeInfo));
        }
    }
}
