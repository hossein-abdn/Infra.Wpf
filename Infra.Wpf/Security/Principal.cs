using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Wpf.Security
{
    public class Principal : IPrincipal
    {
        public AuthorizeBasedOn AuthorizeBasedOn { get; }

        public Principal(Identity identity, AuthorizeBasedOn authorizeBasedOn = AuthorizeBasedOn.BasedOnRole)
        {
            AuthorizeBasedOn = authorizeBasedOn;
            this.identity = identity;
        }

        private readonly Identity identity;
        public IIdentity Identity
        {
            get
            {
                return identity;
            }
        }

        public bool IsInRole(string role)
        {
            if (string.IsNullOrEmpty(role) == true)
                return false;

            if (identity == null || identity.Roles == null)
                return false;

            return identity.Roles.Any(x => x.Name.Equals(role));
        }

        public bool IsInRole(int id)
        {
            if (identity == null || identity.Roles == null)
                return false;

            return identity.Roles.Any(x => x.RoleId == id);
        }

        public bool IsInPermission(string permission)
        {
            if (string.IsNullOrEmpty(permission) == true)
                return false;

            if (identity == null || identity.Permissions == null)
                return false;

            return identity.Permissions.Any(x => x.Url.Equals(permission));
        }

        public bool IsInPermission(int id)
        {
            if (identity == null || identity.Permissions == null)
                return false;

            return identity.Permissions.Any(x => x.PermissionId == id);
        }
    }
}
