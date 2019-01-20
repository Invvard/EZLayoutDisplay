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
        private string _btnApplyLabel;
        private string _btnCloseLabel;
        private string _btnCancelLabel;
        private string _tbLayoutUrlLabel;
        private string _txtLayoutUrlContent;
        private string _tbHotkeyLabel;

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

        public string BtnApplyLabel
        {
            get => _btnApplyLabel;
            set => Set(ref _btnApplyLabel, value);
        }

        public string BtnCloseLabel
        {
            get => _btnCloseLabel;
            set => Set(ref _btnCloseLabel, value);
        }

        public string BtnCancelLabel
        {
            get => _btnCancelLabel;
            set => Set(ref _btnCancelLabel, value);
        }

        public string TbLayoutUrlLabel
        {
            get => _tbLayoutUrlLabel;
            set => Set(ref _tbLayoutUrlLabel, value);
        }

        public string TbHotkeyLabel
        {
            get => _tbHotkeyLabel;
            set => Set(ref _tbHotkeyLabel, value);
        }

        public string TxtLayoutUrlContent
        {
            get => _txtLayoutUrlContent;
            set
            {
                if (Set(ref _txtLayoutUrlContent, value)) { UpdateButtonCanExecute(); }
            }
        }

        #endregion

        #region Constructor
        
        public SettingsViewModel(ISettingsService settingsService, IWindowService windowService)
        {
            _settingsService = settingsService;
            _windowService = windowService;

            SetLabelUi();
        }

        private void SetLabelUi()
        {
            WindowTitle = "Settings";
            TbLayoutUrlLabel = "Configurator URL to your layout :";
            BtnApplyLabel = "Apply";
            BtnCloseLabel = "Close";
            BtnCancelLabel = "Cancel";
            TbHotkeyLabel = "Hotkey to display layout";

            TxtLayoutUrlContent = _settingsService.ErgodoxLayoutUrl;
        }

        #endregion

        #region Private methods

        private void SaveSettings()
        {
            _settingsService.ErgodoxLayoutUrl = TxtLayoutUrlContent;

            _settingsService.Save();

            UpdateButtonCanExecute();
        }

        private void CancelSettings()
        {
            _settingsService.Cancel();

            TxtLayoutUrlContent = _settingsService.ErgodoxLayoutUrl;
        }

        private void CloseSettingsWindow()
        {
            if (IsDirty())
            {
                SaveSettings();
            }
            _windowService.CloseWindow<SettingsWindow>();
        }

        private void UpdateButtonCanExecute()
        {
            ((RelayCommand)ApplySettingsCommand).RaiseCanExecuteChanged();
            ((RelayCommand)CancelSettingsCommand).RaiseCanExecuteChanged();
        }

        private bool IsDirty()
        {
            var isDirty = _settingsService.ErgodoxLayoutUrl != _txtLayoutUrlContent;

            return isDirty;
        }

        #endregion
    }
}