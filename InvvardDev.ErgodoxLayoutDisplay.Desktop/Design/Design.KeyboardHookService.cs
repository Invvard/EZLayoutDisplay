using System.Diagnostics;
using InvvardDev.ErgodoxLayoutDisplay.Desktop.Service.Interface;
using NonInvasiveKeyboardHookLibrary;

namespace InvvardDev.ErgodoxLayoutDisplay.Desktop.Design
{
    public class KeyboardHookService : IKeyboardHookService
    {
        public void RegisterHotkey(ModifierKeys modifiers, int keyCode)
        {
            Debug.WriteLine("Hotkey registered");
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public void Dispose(bool disposing)
        {
            Debug.WriteLine("Disposing the Design.KeyboardHookService");
        }
    }
}