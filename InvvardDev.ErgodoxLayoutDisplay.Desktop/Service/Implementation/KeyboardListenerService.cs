using System.Diagnostics;
using System.Windows;
using InvvardDev.ErgodoxLayoutDisplay.Desktop.Helper;
using InvvardDev.ErgodoxLayoutDisplay.Desktop.Service.Interface;
using NonInvasiveKeyboardHookLibrary;

namespace InvvardDev.ErgodoxLayoutDisplay.Desktop.Service.Implementation
{
    public class KeyboardListenerService : IKeyboardListenerService
    {
        private KeyboardHookManager _hook;

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
            _hook.UnregisterAll();
            _hook.Stop();
        }
    }
}