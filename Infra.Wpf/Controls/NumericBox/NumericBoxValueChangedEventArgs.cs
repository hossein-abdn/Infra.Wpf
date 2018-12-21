using System;

namespace Infra.Wpf.Controls
{
    public class NumericBoxValueChangedEventArgs : EventArgs
    {
        public readonly double? OldValue;

        public readonly double? NewValue;

        public NumericBoxValueChangedEventArgs(double? oldValue, double? newValue) : base()
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}