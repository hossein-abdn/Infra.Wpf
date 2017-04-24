namespace Infra.Wpf.Mvvm
{
    public interface IReferenceInvoker
    {
        object Instance { get; }

        void Execute(object parameter);

        void Clean();

        string MethodName { get; }
    }
}
