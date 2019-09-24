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
using System.Threading;
using System.Diagnostics;

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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Task.Run(focuse);

            Debug.WriteLine(textField.IsFocused);
            Debug.WriteLine(textField.IsKeyboardFocused);
            Debug.WriteLine(textField.IsKeyboardFocusWithin);
            Debug.WriteLine(FocusManager.GetFocusedElement(this)?.ToString());

        }

        private void focuse()
        {
            Thread.Sleep(10);
            Dispatcher.Invoke(() =>
            {
                Keyboard.Focus(textField);
                Debug.WriteLine(textField.IsFocused);
                Debug.WriteLine(textField.IsKeyboardFocused);
                Debug.WriteLine(textField.IsKeyboardFocusWithin);
                Debug.WriteLine(FocusManager.GetFocusedElement(this)?.ToString());
            });
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
            {
                Debug.WriteLine(FocusManager.GetFocusedElement(this)?.ToString());
                Debug.WriteLine(textField.IsFocused);
                Debug.WriteLine(textField.IsKeyboardFocused);
                Debug.WriteLine(textField.IsKeyboardFocusWithin);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(textField);
            Debug.WriteLine(textField.IsFocused);
            Debug.WriteLine(textField.IsKeyboardFocused);
            Debug.WriteLine(textField.IsKeyboardFocusWithin);
        }
    }
}
