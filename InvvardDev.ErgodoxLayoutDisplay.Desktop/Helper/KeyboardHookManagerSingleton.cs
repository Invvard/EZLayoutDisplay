using NonInvasiveKeyboardHookLibrary;

namespace InvvardDev.ErgodoxLayoutDisplay.Desktop.Helper
{
    public class KeyboardHookManagerSingleton
    {
        private static KeyboardHookManager _keyboardHookManager;

        public static KeyboardHookManager Instance => _keyboardHookManager ?? (_keyboardHookManager = new KeyboardHookManager());
    }
}