using System;

namespace Infra.Wpf.Controls
{
    public class NumericBoxValueChangedEventArgs : EventArgs
    {
        public readonly long? OldValue;

        public readonly long? NewValue;

        public NumericBoxValueChangedEventArgs(long? oldValue, long? newValue) : base()
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}