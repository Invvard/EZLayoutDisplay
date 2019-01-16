﻿using System;
using NonInvasiveKeyboardHookLibrary;

namespace InvvardDev.ErgodoxLayoutDisplay.Desktop.Model.Service.Interface
{
    public interface IKeyboardHookService : IDisposable
    {
        void RegisterHotkey(ModifierKeys modifiers, int keyCode);
    }
}