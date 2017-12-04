using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Data;

namespace Infra.Wpf.Controls
{
    public class LookupField : Lookup, INotifyPropertyChanged, IField
    {
        #region Properties
        public string Title { get; set; }

        public string FilterField { get; set; }

        private string _DisplayName;
        public string DisplayName
        {
            get { return _DisplayName; }
            set
            {
                _DisplayName = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public event SearchPhraseChangedEventHandler SearchPhraseChanged;

        #endregion

        #region Methods

        public LookupField()
        {
            Loaded += LookupField_Loaded;
            SelectionChanged += LookupField_SelectionChanged;
        }

        private void LookupField_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SearchPhraseChanged?.Invoke();
        }

        private void LookupField_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            DisplayName = GetDisplayName();
        }

        public void OnPropertyChanged([CallerMemberName]string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public string SearchPhrase
        {
            get
            {
                if (SelectedItems == null)
                    return null;

                string query = "";
                foreach (var item in SelectedItems)
                {
                    string id = item?.GetType()?.GetProperty(IdColumn)?.GetValue(item)?.ToString();
                    if (string.IsNullOrEmpty(id))
                        return null;
                    query = query + $@"{FilterField}=={id.Trim()}" + " OR ";
                }

                if (!string.IsNullOrEmpty(query))
                {
                    query = query.Substring(0, query.Length - 4);
                    return query;
                }

                return null;
            }
        }

        private string GetDisplayName()
        {
            BindingExpression bindEx = BindingOperations.GetBindingExpression(this, SelectedItemsProperty);
            if (bindEx != null && !string.IsNullOrEmpty(bindEx.ResolvedSourcePropertyName))
            {
                var type = DataContext?.GetType().GetProperty("Model")?.PropertyType;
                if (type != null)
                {
                    var propInfo = type?.GetProperty(bindEx.ResolvedSourcePropertyName);
                    var attrib = propInfo?.GetCustomAttributes(typeof(DisplayAttribute), false);
                    if (attrib != null && attrib.Count() > 0)
                        return ((DisplayAttribute) attrib[0]).Name;
                }
                else
                {
                    var displayText = bindEx.ResolvedSourcePropertyName;
                    if (string.IsNullOrEmpty(displayText))
                        return displayText;
                }
            }

            if (!string.IsNullOrWhiteSpace(FilterField))
            {
                var type = DataContext?.GetType().GetProperty("ItemsSource")?.PropertyType;

                if (type != null && type.IsGenericType)
                {
                    var propInfo = type.GenericTypeArguments[0].GetProperty(FilterField);
                    if (propInfo != null)
                    {
                        var attrib = propInfo.GetCustomAttributes(typeof(DisplayAttribute), false);
                        if (attrib != null && attrib.Count() > 0)
                            return ((DisplayAttribute) attrib[0]).Name;
                    }
                }

                return FilterField;
            }

            return string.Empty;
        }

        #endregion
    }
}
