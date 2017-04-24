using System;

namespace Infra.Wpf.Mvvm
{
    public class ArgsParameter<TArgs> where TArgs : EventArgs
    {
        public TArgs e { get; set; }

        public object sender { get; set; }
    }
}
