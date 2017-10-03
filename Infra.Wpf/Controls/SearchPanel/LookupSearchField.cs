namespace Infra.Wpf.Controls
{
    public class LookupSearchField : Lookup, ISearchField
    {
        public string Title { get; set; }

        public string FilterField { get; set; }

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
    }
}
