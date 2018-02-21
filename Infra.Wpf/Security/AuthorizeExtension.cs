using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Infra.Wpf.Security
{
    [MarkupExtensionReturnType(typeof(AuthorizeInfo))]
    public class AuthorizeExtension : MarkupExtension
    {
        public bool EffectOnVisibility { get; set; } = true;

        public string Roles { get; set; }

        public Type ParentType { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return new AuthorizeInfo
            {
                EffectOnVisibility = this.EffectOnVisibility,
                Roles = this.Roles,
                ParentType = this.ParentType
            };
        }
    }
}
