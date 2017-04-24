namespace Infra.Wpf.Controls
{
    public interface ISearchField
    {
        string DisplayName { get; set; }

        string SearchPhrase { get; }

        string FilterField { get; set; }

        void Clear();
    }
}
