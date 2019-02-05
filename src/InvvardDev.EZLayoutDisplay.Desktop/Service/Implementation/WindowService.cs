using System.Collections.Generic;
using System.Windows;
using InvvardDev.EZLayoutDisplay.Desktop.Service.Interface;

namespace InvvardDev.EZLayoutDisplay.Desktop.Service.Implementation
{
    public class WindowService: IWindowService
    {
        private readonly Dictionary<string, Window> _windows;

        public WindowService()
        {
            _windows = new Dictionary<string, Window>();
        }

        #region IWindowService implementation

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
            _windows[windowKey].Activate();
        }

        public void CloseWindow<T>()
        {
            var windowKey = typeof(T).ToString();

            if (_windows.ContainsKey(windowKey))
            {
                _windows[windowKey].Close();
            }
        }

        public bool ShowWarning(string warningMessage)
        {
            var result = MessageBox.Show(warningMessage, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning) == MessageBoxResult.OK;

            return result;
        }

        #endregion

        private void WindowService_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var windowKey = sender.GetType().ToString();

            if (_windows.ContainsKey(windowKey)) { _windows.Remove(windowKey); }
        }
    }
}