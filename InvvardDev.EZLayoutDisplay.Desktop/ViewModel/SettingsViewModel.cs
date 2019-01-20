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
        private string _applyCommandLabel;
        private string _closeCommandLabel;
        private string _cancelCommandLabel;
        private string _layoutUrlLabel;
        private string _layoutUrlContent;
        private string _hotkeyTitleLabel;

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
            get => _applyCommandLabel;
            set => Set(ref _applyCommandLabel, value);
        }

        public string BtnCloseLabel
        {
            get => _closeCommandLabel;
            set => Set(ref _closeCommandLabel, value);
        }

        public string BtnCancelLabel
        {
            get => _cancelCommandLabel;
            set => Set(ref _cancelCommandLabel, value);
        }

        public string LayoutUrlLabel
        {
            get => _layoutUrlLabel;
            set => Set(ref _layoutUrlLabel, value);
        }

        public string HotkeyTitleLabel
        {
            get => _hotkeyTitleLabel;
            set => Set(ref _hotkeyTitleLabel, value);
        }

        public string LayoutUrlContent
        {
            get => _layoutUrlContent;
            set
            {
                if (Set(ref _layoutUrlContent, value)) { UpdateButtonCanExecute(); }
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
            LayoutUrlLabel = "Configurator URL to your layout :";
            BtnApplyLabel = "Apply";
            BtnCloseLabel = "Close";
            BtnCancelLabel = "Cancel";
            HotkeyTitleLabel = "Hotkey to display layout";

            LayoutUrlContent = _settingsService.ErgodoxLayoutUrl;
        }

        #endregion

        #region Private methods

        private void SaveSettings()
        {
            _settingsService.ErgodoxLayoutUrl = LayoutUrlContent;

            _settingsService.Save();

            UpdateButtonCanExecute();
        }

        private void CancelSettings()
        {
            _settingsService.Cancel();

            LayoutUrlContent = _settingsService.ErgodoxLayoutUrl;
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
            var isDirty = _settingsService.ErgodoxLayoutUrl != _layoutUrlContent;

            return isDirty;
        }

        #endregion
    }
}