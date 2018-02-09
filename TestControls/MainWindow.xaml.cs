using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Infrastructure;
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

            var context = new AccountingContext();

            //var person = new Person { Name = "Test", CreateDate = DateTime.Now, RecordStatusId = 1, UserId = 1 };

            var person = context.People.First(x=>x.PersonId==76);

            var entry = context.Entry(person);

            context.People.Remove(person);

            context.SaveChanges();

            
        }
    }
}
