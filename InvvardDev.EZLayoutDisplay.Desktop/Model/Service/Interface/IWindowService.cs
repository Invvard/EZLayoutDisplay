using System.Windows;

namespace InvvardDev.EZLayoutDisplay.Desktop.Model.Service.Interface
{
    public interface IWindowService
    {
        void ShowWindow<T>()
            where T : Window, new();

        void CloseWindow<T>();
    }
}