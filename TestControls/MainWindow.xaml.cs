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
using System.Globalization;

namespace TestControls
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            var context = new MainWindowVM { View = this };
            DataContext = context;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var t = ((MainWindowVM)DataContext);
        }



        private string GetSepratedNumber(string digit)
        {
            if (string.IsNullOrWhiteSpace(digit))
                return "";

            string result = digit;
            for (int i = 1; i <= (digit.Length - 1) / 3; i++)
            {
                int sepratorPos = digit.Length - (i * 3);
                result = result.Insert(sepratorPos, ",");
            }

            return result;
        }
    }

    public class Test
    {
        public int Title { get; set; }
        public Test Test1 { get; set; }
    }

    public class Test1
    {
        public int Name { get; set; }
    }
}
