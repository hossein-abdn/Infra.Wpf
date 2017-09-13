using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Controls;

namespace Infra.Wpf.Controls
{
    [ContentProperty("Items")]
    public class MultiSelect : Control
    {
        #region Properties

        public bool IsOpen
        {
            get { return (bool) GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register("IsOpen", typeof(bool), typeof(MultiSelect), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public double MaxDropDownHeight
        {
            get { return (double) GetValue(MaxDropDownHeightProperty); }
            set { SetValue(MaxDropDownHeightProperty, value); }
        }

        public static readonly DependencyProperty MaxDropDownHeightProperty =
            DependencyProperty.Register("MaxDropDownHeight", typeof(double), typeof(MultiSelect), new PropertyMetadata(200d));

        public Visibility SearchVisible
        {
            get { return (Visibility) GetValue(SearchVisibleProperty); }
            set { SetValue(SearchVisibleProperty, value); }
        }

        public static readonly DependencyProperty SearchVisibleProperty =
            DependencyProperty.Register("SearchVisible", typeof(Visibility), typeof(MultiSelect), new PropertyMetadata(Visibility.Visible));

        public string SearchText
        {
            get { return (string) GetValue(SearchTextProperty); }
            set { SetValue(SearchTextProperty, value); }
        }

        public static readonly DependencyProperty SearchTextProperty =
            DependencyProperty.Register("SearchText", typeof(string), typeof(MultiSelect), new PropertyMetadata(null));

        public Collection<object> Items
        {
            get { return (Collection<object>) GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(Collection<object>), typeof(MultiSelect),
                new FrameworkPropertyMetadata(new Collection<object>(), FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure));

        private StackPanel presenter { get; set; }

        private UIElementCollection containers { get; set; }

        #endregion

        #region Methods

        static MultiSelect()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MultiSelect), new FrameworkPropertyMetadata(typeof(MultiSelect)));
        }

        public MultiSelect()
        {
            Loaded += MultiSelect_Loaded;
        }

        public override void OnApplyTemplate()
        {
            presenter = Template.FindName("presenter", this) as StackPanel;
            if (presenter == null)
                throw new System.ArgumentException("presenter cannot be found.", "presenter");

            containers = presenter.Children;
            base.OnApplyTemplate();
        }

        private void MultiSelect_Loaded(object sender, RoutedEventArgs e)
        {
            //foreach (var item in Items)
            //    OnItemsChanged(Items, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, containers.Count));
        }

        private void OnItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // if itemsource throw exception

            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems)
                    containers.Insert(e.NewStartingIndex, GenerateContainer(item));
            }
        }

        private MultiSelectItem GenerateContainer(object element)
        {
            MultiSelectItem newElement = new MultiSelectItem();
            newElement.Content = element;
            return newElement;
        }
        #endregion
    }
}
