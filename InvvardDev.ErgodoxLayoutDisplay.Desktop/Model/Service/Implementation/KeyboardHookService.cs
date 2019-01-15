using System;
using System.Diagnostics;
using InvvardDev.ErgodoxLayoutDisplay.Desktop.Model.Service.Interface;
using NonInvasiveKeyboardHookLibrary;

namespace InvvardDev.ErgodoxLayoutDisplay.Desktop.Model.Service.Implementation
{
    public class KeyboardHookService : IKeyboardHookService
    {
        bool disposed;
        private static KeyboardHookManager _hook;

        public static KeyboardHookManager Hook => _hook ?? (_hook = new KeyboardHookManager());

        public KeyboardHookService()
        {
            InitKeyboardHook();
        }

        private void InitKeyboardHook()
        {
            Hook.Start();
            RegisterHotkey(ModifierKeys.Control | ModifierKeys.Alt, 0x60);
        }

        private void DisplayLayout()
        {
            Debug.WriteLine("On passe par là !");
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