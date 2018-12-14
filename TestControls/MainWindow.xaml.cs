using DataAccess.Models;
using Infra.Wpf.Security;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using Infra.Wpf.Common.Helpers;
using System.Windows.Controls.Primitives;

namespace TestControls
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<Infra.Wpf.Controls.CheckBoxViewModel> MultiSelectVM { get; set; }

        public MainWindow()
        {
            var context = new MainWindowVM { View = this };
            DataContext = context;
            InitializeComponent();

            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var t = ((MainWindowVM)DataContext);//.Model;

            var result = Validation.GetHasError(multi);
        }
    }
}
