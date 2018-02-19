using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Wpf.Security
{
    public class Identity : IIdentity
    {
        public Identity(string name, int id, IEnumerable<Role> roles, IEnumerable<Permission> permissions)
        {
            this.name = name;
            this.id = id;
            this.roles = roles;
            this.permissions = permissions;
        }

        public string AuthenticationType
        {
            get
            {
                return "Basic";
            }
        }

        public bool IsAuthenticated
        {
            get
            {
                return !string.IsNullOrEmpty(Name);
            }
        }

        private readonly string name;
        public string Name
        {
            get
            {
                return name;
            }
        }

        private readonly int id;
        public int Id
        {
            get
            {
                return id;
            }
        }

        private readonly IEnumerable<Role> roles;
        public IEnumerable<Role> Roles
        {
            get
            {
                return roles;
            }
        }

        private readonly IEnumerable<Permission> permissions;
        public IEnumerable<Permission> Permissions
        {
            get
            {
                return permissions;
            }
        }

    }
}
