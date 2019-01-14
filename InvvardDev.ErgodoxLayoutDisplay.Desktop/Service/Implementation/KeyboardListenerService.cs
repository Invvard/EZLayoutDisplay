using System;
using System.Diagnostics;
using System.Windows;
using InvvardDev.ErgodoxLayoutDisplay.Desktop.Helper;
using InvvardDev.ErgodoxLayoutDisplay.Desktop.Service.Interface;
using NonInvasiveKeyboardHookLibrary;

namespace InvvardDev.ErgodoxLayoutDisplay.Desktop.Service.Implementation
{
    public class KeyboardListenerService : IKeyboardListenerService
    {
        private readonly KeyboardHookManager _hook;

        public KeyboardListenerService()
        {
            _hook = KeyboardHookManagerSingleton.Instance;
            _hook.Start();

            RegisterKey(ModifierKeys.Control, 0x60);
        }

        public void RegisterKey(ModifierKeys modifiers, int virtualCode)
        {
            _hook.RegisterHotkey(modifiers, virtualCode, OpenDialog);
        }

        private void OpenDialog()
        {
            Debug.Write("On passe ici !");
            var test = MessageBox.Show("Salut", "Ca va ?", MessageBoxButton.OK);

        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // The bulk of the clean-up code is implemented in Dispose(bool)
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _hook.UnregisterAll();
                _hook.Stop();
            }
        }
    }
}