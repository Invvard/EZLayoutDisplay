using System.Windows;

namespace InvvardDev.ErgodoxLayoutDisplay.Desktop.Model.Service.Interface
{
    public interface IWindowService
    {
        void ShowWindow<T>()
            where T : Window, new();
    }
}