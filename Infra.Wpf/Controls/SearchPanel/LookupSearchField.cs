namespace Infra.Wpf.Controls
{
    public class LookupSearchField : Lookup, ISearchField
    {
        public string DisplayName { get; set; }

        public string FilterField { get; set; }

        public string SearchPhrase
        {
            get
            {
                if (SelectedItems == null)
                    return null;

                string command = "";
                foreach (var item in SelectedItems)
                {
                    var id = item?.GetType()?.GetProperty(IdColumn)?.GetValue(item)?.ToString();
                    if (string.IsNullOrEmpty(id))
                        return null;
                    command = command + $@"{FilterField}=={id.Trim()}" + " OR ";
                }

                if (!string.IsNullOrEmpty(command))
                {
                    command = command.Substring(0, command.Length - 4);
                    return command;
                }

                return null;
            }
        }
    }
}
