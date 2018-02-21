using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;

namespace Infra.Wpf.Security
{
    public class AuthorizeBehavior : Behavior<FrameworkElement>
    {
        private AuthorizeInfo authorizeInfo { get; set; }

        private bool needAuthorize { get; set; }

        private static List<AuthorizeBehavior> behaviors { get; set; }

        static AuthorizeBehavior()
        {
            behaviors = new List<AuthorizeBehavior>();
        }

        public static void AuthorizeAgain()
        {
            foreach (var item in behaviors)
                item.needAuthorize = true;
        }

        public AuthorizeBehavior() : this(new AuthorizeInfo { Roles = "", EffectOnVisibility = true, ParentType = null })
        {

        }

        public AuthorizeBehavior(AuthorizeInfo authorizeInfo)
        {
            this.authorizeInfo = authorizeInfo;
            needAuthorize = true;
        }

        protected override void OnAttached()
        {
            behaviors.Add(this);
            AssociatedObject.LayoutUpdated += AssociatedObject_LayoutUpdated;
            AssociatedObject.Unloaded += AssociatedObject_Unloaded;
            base.OnAttached();
        }

        private void AssociatedObject_LayoutUpdated(object sender, EventArgs e)
        {
            if (needAuthorize == false)
                return;

            needAuthorize = false;

            var principal = Thread.CurrentPrincipal as Principal;
            if (principal == null)
                return;

            var identity = principal.Identity as Identity;
            if (identity == null)
                return;

            if (principal.AuthorizeBasedOn == AuthorizeBasedOn.BasedOnRole)
            {
                var roles = authorizeInfo.Roles?.Split(',');
                if (roles == null)
                    return;

                var result = identity.Roles?.Any(x => roles.Contains(x.Name));
                CommitPermission(result);
            }
            else
            {
                if (authorizeInfo.ParentType == null)
                    return;

                var permission = authorizeInfo.ParentType.FullName + "." + AssociatedObject.Name;
                var result = identity.Permissions?.Any(x => x.Url == permission);
                CommitPermission(result);
            }
        }

        private void CommitPermission(bool? isAuthorized)
        {
            if (isAuthorized == true)
            {
                if (authorizeInfo.EffectOnVisibility)
                    AssociatedObject.SetValue(FrameworkElement.VisibilityProperty, Visibility.Visible);
                else
                    AssociatedObject.SetValue(FrameworkElement.IsEnabledProperty, true);
            }
            else
            {
                if (authorizeInfo.EffectOnVisibility)
                    AssociatedObject.SetValue(FrameworkElement.VisibilityProperty, Visibility.Collapsed);
                else
                    AssociatedObject.SetValue(FrameworkElement.IsEnabledProperty, false);
            }
        }

        private void AssociatedObject_Unloaded(object sender, RoutedEventArgs e)
        {
            OnDetaching();
        }

        protected override void OnDetaching()
        {
            AssociatedObject.LayoutUpdated -= AssociatedObject_LayoutUpdated;
            AssociatedObject.Unloaded -= AssociatedObject_Unloaded;
            behaviors.Remove(this);

            base.OnDetaching();
        }
    }
}
