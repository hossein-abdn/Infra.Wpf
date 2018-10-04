using System;

namespace Infra.Wpf.Controls
{
    public interface IField
    {
        string Title { get; set; }

        string SearchPhrase { get; }

        string FilterField { get; set; }

        string DisplayName { get; set; }

        Type ModelType { get; set; }

        void Clear();

        event SearchPhraseChangedEventHandler SearchPhraseChanged;
    }
}
