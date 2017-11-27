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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Infra.Wpf.Controls;
using System.Globalization;
using System.Threading;
using Infra.Wpf;
using C1.WPF;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections;
using Infra.Wpf.Business;
using System.Linq.Expressions;
using Infra.Wpf.Common.Helpers;
using Infra.Wpf.Repository;
using System.Data.Entity;
using Infra.Wpf.Mvvm;

namespace TestControls
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public RelayCommand<string> GetAllCommand { get; set; }

        private static void Execute(string obj)
        {
            int a = 1;
        }

        public MainWindow()
        {
            GetAllCommand = new RelayCommand<string>(Execute);

            InitializeComponent();

            DataContext = new StudentVm { Model = new DataAccess.Models.Person() };

            CultureInfo calture = new CultureInfo("fa-IR");
            calture.NumberFormat.DigitSubstitution = DigitShapes.NativeNational;
            Thread.CurrentThread.CurrentCulture = calture;
            Thread.CurrentThread.CurrentUICulture = calture;
            
        }
    }
}
