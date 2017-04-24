using System;

namespace Infra.Wpf.Controls
{
    public class TimeEditorValueChangedEventArgs : EventArgs
    {
        public readonly TimeSpan? OldValue;

        public readonly TimeSpan? NewValue;

        public TimeEditorValueChangedEventArgs(TimeSpan? oldValue, TimeSpan? newValue) : base()
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}