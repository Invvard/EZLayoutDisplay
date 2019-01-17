using GalaSoft.MvvmLight;
using InvvardDev.EZLayoutDisplay.Desktop.Model.Service.Interface;

namespace InvvardDev.EZLayoutDisplay.Desktop.ViewModel
{
    public class SettingsViewModel : ViewModelBase
    {
        private readonly ISettingsService _settingsService;

        private string _windowTitle;
        private string _btnOkText;
        private string _tbLayoutUrlText;
        private string _txtLayoutUrlText;

        public string WindowTitle
        {
            get => _windowTitle;
            set => Set(ref _windowTitle, value);
        }

        public string BtnOkText
        {
            get => _btnOkText;
            set => Set(ref _btnOkText, value);
        }

        public string TbLayoutUrlText
        {
            get => _tbLayoutUrlText;
            set => Set(ref _tbLayoutUrlText, value);
        }

        public string TxtLayoutUrlText
        {
            get => _txtLayoutUrlText;
            set => Set(ref _txtLayoutUrlText, value);
        }

        public SettingsViewModel(ISettingsService settingsService)
        {
            _settingsService = settingsService;

            WindowTitle = "Settings";
            TbLayoutUrlText = "Configurator URL to your layout :";
            TxtLayoutUrlText = "https://configure.ergodox-ez.com/layouts/default/latest/0";
            BtnOkText = "OK";
        }
    }
}