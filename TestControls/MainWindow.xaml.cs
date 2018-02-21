using Infra.Wpf.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TestControls
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            IEnumerable<Role> roles = new List<Role>() { new Role { Name = "admin1", RoleId = 1 }, new Role { Name = "admin", RoleId = 2 } };
            IEnumerable<Permission> permission = new List<Permission>() { new Permission { PermissionId = 1, Url = "TestControls.MainWindow.Test1" } };
            Identity i = new Identity("hossein", 1, roles, permission);
            Principal p = new Principal(i, AuthorizeBasedOn.BaseOnPermission);
            AppDomain.CurrentDomain.SetThreadPrincipal(p);

            AuthorizeBehavior.AuthorizeAgain();
        }
    }
}
