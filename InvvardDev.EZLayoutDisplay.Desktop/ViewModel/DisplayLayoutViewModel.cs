using GalaSoft.MvvmLight;

namespace InvvardDev.EZLayoutDisplay.Desktop.ViewModel
{
    public class DisplayLayoutViewModel : ViewModelBase
    {
        private string _windowTitle;

        public string WindowTitle
        {
            get => _windowTitle;
            set => Set(ref _windowTitle, value);
        }

        public DisplayLayoutViewModel()
        {
            SetLabelUi();
        }

        private void SetLabelUi()
        {
            WindowTitle = "Ergodox Layout";
        }
    }
}