using Infra.Wpf.Common.Helpers;
using Infra.Wpf.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;

namespace Infra.Wpf.Controls
{
    public class CustomComboBox : ComboBox
    {
        #region Properties

        public Type EnumType
        {
            get
            {
                return (Type) GetValue(EnumTypeProperty);
            }
            set
            {
                SetValue(EnumTypeProperty, value);
            }
        }

        public static readonly DependencyProperty EnumTypeProperty = DependencyProperty.Register("EnumType", typeof(Type),
            typeof(CustomComboBox), new PropertyMetadata(null, EnumTypeChanged));

        public Visibility SearchVisible
        {
            get { return (Visibility) GetValue(SearchVisibleProperty); }
            set { SetValue(SearchVisibleProperty, value); }
        }

        public static readonly DependencyProperty SearchVisibleProperty =
            DependencyProperty.Register("SearchVisible", typeof(Visibility), typeof(CustomComboBox), new PropertyMetadata(Visibility.Collapsed));

        public string SearchText
        {
            get { return (string) GetValue(SearchTextProperty); }
            set { SetValue(SearchTextProperty, value); }
        }

        public static readonly DependencyProperty SearchTextProperty =
            DependencyProperty.Register("SearchText", typeof(string), typeof(CustomComboBox), new PropertyMetadata(null));

        public RelayCommand ClearCommand { get; set; }

        public bool IsNullable { get; set; }

        IDictionary displayValues;

        IDictionary enumValues;

        TextBox SearchElement;

        CollectionView itemsViewOriginal;

        #endregion

        #region Methods

        static CustomComboBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CustomComboBox), new FrameworkPropertyMetadata(typeof(CustomComboBox)));
        }

        protected override void OnDropDownClosed(EventArgs e)
        {
            base.OnDropDownClosed(e);

            SearchText = string.Empty;

            itemsViewOriginal.Filter = x => true;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            SearchElement = (TextBox) Template.FindName("Search_PART", this);
            SearchElement.TextChanged += SearchTextBox_TextChanged;
            SearchElement.PreviewKeyDown += SearchElement_KeyDown;

            PreviewKeyDown += CustomComboBox_PreviewKeyDown;
        }

        private void SearchElement_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Down || e.Key == Key.Up)
                ((ComboBoxItem) this.ItemContainerGenerator.ContainerFromIndex(0))?.Focus();
            else if (e.Key == Key.Enter && itemsViewOriginal.Count > 0)
                SelectedItem = itemsViewOriginal.GetItemAt(0);
        }

        private void CustomComboBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key != Key.Down && e.Key != Key.Up)
                SearchElement.Focus();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            itemsViewOriginal.Filter = x =>
            {
                if (String.IsNullOrEmpty(SearchText))
                    return true;
                else
                {
                    if (((ComboItem) x).DisplayName.Contains(SearchText))
                        return true;
                    else
                        return false;
                }
            };

            itemsViewOriginal.Refresh();
        }

        private void ClearExecute()
        {
            this.SelectedIndex = -1;
        }

        private static void EnumTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null || ((CustomComboBox) d).IsLoaded == false)
                return;

            if (!((Type) e.NewValue).IsEnum)
                throw new ArgumentException("Parameter is not an Enumermated type", "EnumType");

            var customComboBox = (CustomComboBox) d;
            customComboBox.ItemsSource = customComboBox.CreateEnumDispalyItems();
            customComboBox.ItemTemplate = customComboBox.CreateItemTemplate();

            IValueConverter converter = new CustomConverter((v, t, p, c) =>
            {
                if (v != null)
                    return customComboBox.enumValues[v];
                return null;
            }, (v, t, p, c) =>
            {
                if (v != null)
                    return customComboBox.displayValues[v];
                return null;
            });

            BindingExpression bindExpression = BindingOperations.GetBindingExpression(customComboBox, SelectedItemProperty);
            if (bindExpression != null && bindExpression.ResolvedSource != null)
            {
                var selectedItemType = bindExpression.ResolvedSource.GetType().GetProperty(bindExpression.ResolvedSourcePropertyName).PropertyType;

                if (customComboBox.IsNullable && selectedItemType.IsGenericType)
                    selectedItemType = selectedItemType.GetGenericArguments()[0];
                if (customComboBox.EnumType != selectedItemType && selectedItemType.IsAssignableFrom(customComboBox.EnumType) == false)
                    throw new ArgumentException("EnumType and SelectedItem Type is not equal.", "EnumType");

                Binding newBinding = bindExpression.ParentBinding.Clone();
                if (newBinding.Converter == null)
                    newBinding.Converter = converter;

                customComboBox.SetBinding(SelectedItemProperty, newBinding);
            }
        }

        public CustomComboBox()
        {
            Loaded += CustomComboBox_Loaded;
            ClearCommand = new RelayCommand(ClearExecute);
        }

        private void CustomComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            EnumTypeChanged(this, new DependencyPropertyChangedEventArgs(EnumTypeProperty, null, EnumType));

            itemsViewOriginal = (CollectionView) CollectionViewSource.GetDefaultView(ItemsSource);
        }

        private IEnumerable CreateEnumDispalyItems()
        {
            Type displayValuesType = typeof(Dictionary<,>).MakeGenericType(typeof(ComboItem), EnumType);
            displayValues = (IDictionary) Activator.CreateInstance(displayValuesType);

            Type enumValueType = typeof(Dictionary<,>).MakeGenericType(EnumType, typeof(ComboItem));
            enumValues = (IDictionary) Activator.CreateInstance(enumValueType);

            var displayList = new List<ComboItem>();

            FieldInfo[] fields = EnumType.GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (var field in fields)
            {
                var attrib = field.GetCustomAttributes(typeof(EnumDisplayAttribute), false);

                if (attrib.Count() > 0)
                {
                    ComboItem item = new ComboItem();

                    if (string.IsNullOrEmpty(((EnumDisplayAttribute) attrib[0]).DisplayName) == false)
                        item.DisplayName = ((EnumDisplayAttribute) attrib[0]).DisplayName;
                    else
                        item.DisplayName = field.GetValue(null).ToString();

                    item.Image = ((EnumDisplayAttribute) attrib[0]).Image;

                    displayList.Add(item);
                    enumValues[field.GetValue(null)] = item;
                    displayValues[item] = field.GetValue(null);
                }
                else
                {
                    ComboItem item = new ComboItem { DisplayName = field.GetValue(null).ToString() };

                    displayList.Add(item);
                    enumValues[field.GetValue(null)] = item;
                    displayValues[item] = field.GetValue(null);
                }
            }
            return displayList;
        }

        private DataTemplate CreateItemTemplate()
        {
            FrameworkElementFactory image = new FrameworkElementFactory(typeof(Image));
            image.SetValue(Image.VerticalAlignmentProperty, VerticalAlignment.Center);
            image.SetValue(Image.HeightProperty, 16d);
            image.SetValue(Image.MarginProperty, new Thickness(0, 0, 5, 0));
            image.SetBinding(Image.SourceProperty, new Binding("Image"));

            FrameworkElementFactory textBlock = new FrameworkElementFactory(typeof(TextBlock));
            textBlock.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
            textBlock.SetBinding(TextBlock.TextProperty, new Binding("DisplayName"));

            FrameworkElementFactory stackPanel = new FrameworkElementFactory(typeof(StackPanel));
            stackPanel.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);
            stackPanel.AppendChild(image);
            stackPanel.AppendChild(textBlock);

            return new DataTemplate { VisualTree = stackPanel };
        }

        #endregion
    }
}