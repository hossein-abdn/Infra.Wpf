using Infra.Wpf.Security;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Wpf.Common.Helpers
{
    public static class ConnectionStringHelper
    {
        public static string GetConnectionString(string connectionStringName, string key)
        {
            return EncryptText.Decryptor(ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString, key);
        }
    }
}
