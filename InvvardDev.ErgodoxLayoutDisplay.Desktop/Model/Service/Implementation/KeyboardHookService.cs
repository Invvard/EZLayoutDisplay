using System;
using System.Diagnostics;
using System.Windows;
using InvvardDev.ErgodoxLayoutDisplay.Desktop.Model.Service.Interface;
using InvvardDev.ErgodoxLayoutDisplay.Desktop.View;
using NonInvasiveKeyboardHookLibrary;

namespace InvvardDev.ErgodoxLayoutDisplay.Desktop.Model.Service.Implementation
{
    public class KeyboardHookService : IKeyboardHookService
    {
        private bool disposed;
        private static KeyboardHookManager _hook;

        private IWindowService _windowService;

        public static KeyboardHookManager Hook => _hook ?? (_hook = new KeyboardHookManager());

        public KeyboardHookService(IWindowService windowService)
        {
            _windowService = windowService;

            InitKeyboardHook();
        }

        private void InitKeyboardHook()
        {
            Hook.Start();
            RegisterHotkey(ModifierKeys.Control | ModifierKeys.Alt, 0x60);
        }

        private void DisplayLayout()
        {
            Application.Current.Dispatcher.Invoke(delegate {
                                                      _windowService.ShowWindow<DisplayLayoutWindow>();
                                                  });
        }

        public void RegisterHotkey(ModifierKeys modifiers, int keyCode)
        {
            Hook.RegisterHotkey(modifiers, keyCode, DisplayLayout);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                Hook.UnregisterAll();
                Hook.Stop();
            }

            disposed = true;
        }
    }
}