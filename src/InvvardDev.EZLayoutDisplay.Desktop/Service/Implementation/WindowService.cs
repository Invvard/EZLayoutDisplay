using System.Collections.Generic;
using System.Windows;
using InvvardDev.EZLayoutDisplay.Desktop.Helper;
using InvvardDev.EZLayoutDisplay.Desktop.Service.Interface;
using NLog;

namespace InvvardDev.EZLayoutDisplay.Desktop.Service.Implementation
{
    public class WindowService : IWindowService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly Dictionary<string, Window> _windows;

        public WindowService()
        {
            Logger.TraceConstructor();
            _windows = new Dictionary<string, Window>();
        }

        #region IWindowService implementation

        public void ShowWindow<T>()
            where T : Window, new()
        {
            Logger.TraceMethod();
            Logger.Info("Opening {windowType} window", typeof(T));

            var windowKey = typeof(T).ToString();

            Logger.Debug("Windows opened list : {@windows}", _windows);

            if (!_windows.ContainsKey(windowKey))
            {
                Logger.Debug("{windowType} window added", typeof(T));

                _windows.Add(windowKey, new T());
                _windows[windowKey].Closing += WindowService_Closing;
            }

            _windows[windowKey].Show();
            _windows[windowKey].Activate();
        }

        public void CloseWindow<T>()
        {
            Logger.TraceMethod();
            Logger.Info("Closing {windowType} window", typeof(T));

            var windowKey = typeof(T).ToString();

            if (_windows.ContainsKey(windowKey))
            {
                Logger.Debug("{windowType} window is going to be closed", typeof(T));
                _windows[windowKey].Close();
            }
        }

        public bool ShowWarning(string warningMessage)
        {
            Logger.TraceMethod();

            var result = MessageBox.Show(warningMessage, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning) == MessageBoxResult.OK;

            Logger.DebugOutputParam(nameof(result), result);

            return result;
        }

        #endregion

        private void WindowService_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Logger.TraceMethod();

            var windowKey = sender.GetType().ToString();
            Logger.Debug("Window {window} is closing", windowKey);

            if (_windows.ContainsKey(windowKey))
            {
                Logger.Debug("Finalizing {windowType} window close", windowKey);

                _windows[windowKey].Closing -= WindowService_Closing;
                _windows.Remove(windowKey);
            }
        }
    }
}