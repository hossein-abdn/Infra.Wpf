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

namespace TestControls
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window,INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();

            CultureInfo calture = new CultureInfo("fa-IR");
            calture.NumberFormat.DigitSubstitution = DigitShapes.NativeNational;
            Thread.CurrentThread.CurrentCulture = calture;
            Thread.CurrentThread.CurrentUICulture = calture;
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var list = new List<Product>();

            for (int i = 0; i < 15; i++)
                list.Add(new Product());

            productList = new ObservableCollection<Product>(list);
        }

        public static List<Product> GetDataModel
        {
            get
            {
                List<Product> _products = new List<Product>();
                for (int i = 0; i < 15; i++)
                {
                    _products.Add(new Product());
                }
                return _products;
            }
        }

        private ObservableCollection<Product> _productList;
        public ObservableCollection<Product> productList
        {
            get { return _productList; }
            set
            {
                _productList = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var item = multi.Items[4];
            var item2 = new Product();
            ObservableCollection<object> newlist = new ObservableCollection<object>();
            newlist.Add(item);
            newlist.Add(item2);



            multi.SelectedItems = newlist;
            

            
            

        }
    }
}
