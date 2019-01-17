using System;
using System.Diagnostics;
using System.Windows;
using InvvardDev.EZLayoutDisplay.Desktop.Model.Service.Interface;
using InvvardDev.EZLayoutDisplay.Desktop.View;
using NonInvasiveKeyboardHookLibrary;

namespace InvvardDev.EZLayoutDisplay.Desktop.Model.Service.Implementation
{
    public class KeyboardHookService : IKeyboardHookService
    {
        private bool disposed;
        private static KeyboardHookManager _hook;

        private readonly IWindowService _windowService;
        private readonly ISettingsService _settingsService;

        public static KeyboardHookManager Hook => _hook ?? (_hook = new KeyboardHookManager());

        public KeyboardHookService(IWindowService windowService, ISettingsService settingsService)
        {
            _windowService = windowService;
            _settingsService = settingsService;

            InitKeyboardHook();
        }

        private void InitKeyboardHook()
        {
            Hook.Start();

            var hotkeyShowLayout = _settingsService.GetHotKeyShowLayout();
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