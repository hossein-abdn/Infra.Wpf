namespace Infra.Wpf.Controls
{
    public interface ISearchField
    {
        string Title { get; set; }

        string SearchPhrase { get; }

        string FilterField { get; set; }

        void Clear();
    }
}
