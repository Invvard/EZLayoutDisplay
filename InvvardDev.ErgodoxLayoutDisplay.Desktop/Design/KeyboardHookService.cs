using System.Diagnostics;
using InvvardDev.ErgodoxLayoutDisplay.Desktop.Model.Service.Interface;
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

        protected virtual void Dispose(bool disposing)
        {
            Debug.WriteLine("Disposing the Design.KeyboardHookService");
        }
    }
}