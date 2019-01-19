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

        private ICommand _applySettingsCommand;
        private ICommand _closeSettingsCommand;
        private ICommand _cancelSettingsCommand;

        private string _windowTitle;
        private string _btnApplyText;
        private string _btnCloseText;
        private string _btnCancelText;
        private string _tbLayoutUrlText;
        private string _txtLayoutUrlText;

        #endregion

        #region Relay commands

        /// <summary>
        /// Cancel settings edition.
        /// </summary>
        public ICommand CancelSettingsCommand =>
            _cancelSettingsCommand
            ?? (_cancelSettingsCommand = new RelayCommand(CancelSettings, IsDirty));

        /// <summary>
        /// Applies the settings.
        /// </summary>
        public ICommand ApplySettingsCommand =>
            _applySettingsCommand
            ?? (_applySettingsCommand = new RelayCommand(SaveSettings, IsDirty));

        /// <summary>
        /// Closes the settings window.
        /// </summary>
        public ICommand CloseSettingsCommand =>
            _closeSettingsCommand
            ?? (_closeSettingsCommand = new RelayCommand(CloseSettingsWindow));

        #endregion

        #region Properties

        public string WindowTitle
        {
            get => _windowTitle;
            set => Set(ref _windowTitle, value);
        }

        public string BtnApplyText
        {
            get => _btnApplyText;
            set => Set(ref _btnApplyText, value);
        }

        public string BtnCloseText
        {
            get => _btnCloseText;
            set => Set(ref _btnCloseText, value);
        }

        public string BtnCancelText
        {
            get => _btnCancelText;
            set => Set(ref _btnCancelText, value);
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
                if (Set(ref _txtLayoutUrlText, value)) { UpdateButtonCanExecute(); }
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
            BtnApplyText = "Apply";
            BtnCloseText = "Close";
            BtnCancelText = "Cancel";
        }

        #region Private methods

        private void SaveSettings()
        {
            _settingsService.ErgodoxLayoutUrl = TxtLayoutUrlText;

            _settingsService.Save();

            UpdateButtonCanExecute();
        }

        private void CancelSettings()
        {
            _settingsService.Cancel();

            TxtLayoutUrlText = _settingsService.ErgodoxLayoutUrl;
        }

        private void CloseSettingsWindow()
        {
            SaveSettings();
            _windowService.CloseWindow<SettingsWindow>();
        }

        private void UpdateButtonCanExecute()
        {
            ((RelayCommand)ApplySettingsCommand).RaiseCanExecuteChanged();
            ((RelayCommand)CancelSettingsCommand).RaiseCanExecuteChanged();
        }

        private bool IsDirty()
        {
            var isDirty = _settingsService.ErgodoxLayoutUrl != _txtLayoutUrlText;

            return isDirty;
        }

        #endregion
    }
}