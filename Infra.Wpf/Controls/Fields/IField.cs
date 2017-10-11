namespace Infra.Wpf.Controls
{
    public interface IField
    {
        string Title { get; set; }

        string SearchPhrase { get; }

        string FilterField { get; set; }

        void Clear();
    }
}
