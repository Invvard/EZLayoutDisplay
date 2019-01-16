﻿using GalaSoft.MvvmLight;

namespace InvvardDev.ErgodoxLayoutDisplay.Desktop.ViewModel
{
    public class DisplayLayoutViewModel : ViewModelBase
    {
        private string _windowTitle;

        public string WindowTitle
        {
            get => _windowTitle;
            set { Set(() => WindowTitle, ref _windowTitle, value); }
        }

        public DisplayLayoutViewModel()
        {
            WindowTitle = "Ergodox Layout";
        }
    }
}