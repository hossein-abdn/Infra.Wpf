using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Wpf.Security
{
    public class AuthorizeInfo
    {
        public bool EffectOnVisibility { get; set; }

        public string Roles { get; set; }

        public Type ParentType { get; set; }
    }
}
