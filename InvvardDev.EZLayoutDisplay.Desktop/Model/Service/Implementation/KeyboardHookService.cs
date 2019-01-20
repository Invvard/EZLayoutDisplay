using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using InvvardDev.EZLayoutDisplay.Desktop.Helper;
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

            var hotkeyShowLayout = _settingsService.HotkeyShowLayout;

            switch (hotkeyShowLayout.ModifierKeys.Count)
            {
                case 0:
                    Hook.RegisterHotkey(hotkeyShowLayout.KeyCode, DisplayLayout);
                    break;
                case 1:
                    Hook.RegisterHotkey(hotkeyShowLayout.ModifierKeys.First(), hotkeyShowLayout.KeyCode, DisplayLayout);
                    break;
                default:
                    var sumModifierKeys = SumModifiers(hotkeyShowLayout);
                    Hook.RegisterHotkey(sumModifierKeys, hotkeyShowLayout.KeyCode, DisplayLayout);
                    break;
            }
        }

        private static ModifierKeys SumModifiers(Hotkey hotkeyShowLayout)
        {
            var sumModifierKeys = hotkeyShowLayout.ModifierKeys[0];

            for (int i = 1; i < hotkeyShowLayout.ModifierKeys.Count; i++)
            {
                sumModifierKeys = sumModifierKeys | hotkeyShowLayout.ModifierKeys[i];
            }

            return sumModifierKeys;
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

        public void RegisterHotkey(int keyCode)
        {
            Hook.RegisterHotkey(keyCode, DisplayLayout);
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