using System.Collections.Generic;
using System.Windows;
using InvvardDev.EZLayoutDisplay.Desktop.Model.Service.Interface;

namespace InvvardDev.EZLayoutDisplay.Desktop.Model.Service.Implementation
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
                _windows[windowKey].Closing += WindowService_Closing;
            }

            _windows[windowKey].Show();
        }

        public void CloseWindow<T>()
        {
            var windowKey = typeof(T).ToString();

            if (_windows.ContainsKey(windowKey))
            {
                _windows[windowKey].Close();
            }
        }

        private void WindowService_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var windowKey = sender.GetType().ToString();

            if (_windows.ContainsKey(windowKey)) { _windows.Remove(windowKey); }
        }
    }
}