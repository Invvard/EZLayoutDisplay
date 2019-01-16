using System;
using NonInvasiveKeyboardHookLibrary;

namespace InvvardDev.ErgodoxLayoutDisplay.Desktop.Service.Interface
{
    public interface IKeyboardListenerService : IDisposable
    {
        void RegisterKey(ModifierKeys modifiers, int virtualCode);
    }
}