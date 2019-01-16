using System.Collections.Generic;
using System.Windows;
using InvvardDev.ErgodoxLayoutDisplay.Desktop.Model.Service.Interface;

namespace InvvardDev.ErgodoxLayoutDisplay.Desktop.Model.Service.Implementation
{
    public class WindowService: IWindowService
    {
        private Dictionary<string, Window> _windows;

        public WindowService()
        {
            _windows = new Dictionary<string, Window>();
        }

        public void ShowWindow<T>()
            where T : Window, new()
        {
            var windowKey = typeof(T).ToString();
            if (!_windows.ContainsKey(windowKey))
            {
                _windows.Add(windowKey, new T());
            }

            _windows[windowKey].Show();
        }
    }
}