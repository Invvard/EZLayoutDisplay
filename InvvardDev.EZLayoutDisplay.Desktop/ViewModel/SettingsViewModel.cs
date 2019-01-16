﻿using GalaSoft.MvvmLight;

namespace InvvardDev.EZLayoutDisplay.Desktop.ViewModel
{
    public class SettingsViewModel : ViewModelBase
    {
        private string _windowTitle;

        public string WindowTitle
        {
            get => _windowTitle;
            set => Set(ref _windowTitle, value);
        }

        public SettingsViewModel()
        {
            WindowTitle = "Settings";
        }
    }
}