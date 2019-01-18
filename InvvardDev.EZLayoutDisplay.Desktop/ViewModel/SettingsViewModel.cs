using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using InvvardDev.EZLayoutDisplay.Desktop.Model.Service.Interface;
using InvvardDev.EZLayoutDisplay.Desktop.View;

namespace InvvardDev.EZLayoutDisplay.Desktop.ViewModel
{
    public class SettingsViewModel : ViewModelBase
    {
        #region Fields

        private readonly ISettingsService _settingsService;
        private readonly IWindowService _windowService;

        private ICommand _saveSettingsCommand;

        private string _windowTitle;
        private string _btnOkText;
        private string _tbLayoutUrlText;
        private string _txtLayoutUrlText;

        #endregion

        #region Properties

        /// <summary>
        /// Shows the Layout window.
        /// </summary>
        public ICommand SaveSettingsCommand =>
            _saveSettingsCommand
            ?? (_saveSettingsCommand = new RelayCommand(() => {
                                                            _settingsService.Save();
                                                            _windowService.CloseWindow<SettingsWindow>();
                                                        }));

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
            set
            {
                _settingsService.ErgodoxLayoutUrl = value;
                Set(ref _txtLayoutUrlText, value);
            }
        }

        #endregion

        public SettingsViewModel(ISettingsService settingsService, IWindowService windowService)
        {
            _settingsService = settingsService;
            _windowService = windowService;

            WindowTitle = "Settings";
            TbLayoutUrlText = "Configurator URL to your layout :";
            TxtLayoutUrlText = _settingsService.ErgodoxLayoutUrl;
            BtnOkText = "OK";
        }
    }
}