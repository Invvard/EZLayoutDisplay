using System.Diagnostics;
using System.Windows;
using InvvardDev.EZLayoutDisplay.Desktop.Service.Interface;

namespace InvvardDev.EZLayoutDisplay.Desktop.Design
{
    public class WindowService : IWindowService
    {
        public void ShowWindow<T>()
            where T : Window, new()
        {
            Debug.WriteLine($"Opens window {typeof(T)}");
        }

        public void CloseWindow<T>()
        {
            Debug.WriteLine($"Closes window {typeof(T)}");
        }

        public bool ShowWarning(string warningMessage)
        {
            Debug.WriteLine($"Opens a warning dialog with \"{warningMessage}\"");

            return true;
        }
    }
}