using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using InvvardDev.EZLayoutDisplay.Desktop.Model;
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
        private string _hotkeyTitleLabel;
        private string _altModifierLabel;
        private string _ctrlModifierLabel;
        private string _shiftModifierLabel;
        private string _windowsModifierLabel;

        private string _layoutUrlContent;
        private Hotkey _displayLayoutHotkey;

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

        public string ApplyCommandLabel
        {
            get => _applyCommandLabel;
            set => Set(ref _applyCommandLabel, value);
        }

        public string CloseCommandLabel
        {
            get => _closeCommandLabel;
            set => Set(ref _closeCommandLabel, value);
        }

        public string CancelCommandLabel
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
        
        public string AltModifierLabel
        {
            get => _altModifierLabel;
            set => Set(ref _altModifierLabel, value);
        }
        
        public string CtrlModifierLabel
        {
            get => _ctrlModifierLabel;
            set => Set(ref _ctrlModifierLabel, value);
        }
        
        public string ShiftModifierLabel
        {
            get => _shiftModifierLabel;
            set => Set(ref _shiftModifierLabel, value);
        }
        
        public string WindowsModifierLabel
        {
            get => _windowsModifierLabel;
            set => Set(ref _windowsModifierLabel, value);
        }

        public Hotkey DisplayLayoutHotkey
        {
            get => _displayLayoutHotkey;
            set => Set(ref _displayLayoutHotkey, value);
        }

        #endregion

        #region Constructor

        public SettingsViewModel(ISettingsService settingsService, IWindowService windowService)
        {
            _settingsService = settingsService;
            _windowService = windowService;

            SetLabelUi();

            SetSettingControls();
        }

        private void SetLabelUi()
        {
            WindowTitle = "Settings";
            LayoutUrlLabel = "Configurator URL to your layout :";
            ApplyCommandLabel = "Apply";
            CloseCommandLabel = "Close";
            CancelCommandLabel = "Cancel";
            HotkeyTitleLabel = "Hotkey to display layout";
            AltModifierLabel = "ALT";
            CtrlModifierLabel = "CTRL";
            ShiftModifierLabel = "SHIFT";
            WindowsModifierLabel = "Windows";
        }

        private void SetSettingControls()
        {
            LayoutUrlContent = _settingsService.ErgodoxLayoutUrl;
            DisplayLayoutHotkey = _settingsService.HotkeyShowLayout;
        }

        #endregion

        #region Private methods

        private void SaveSettings()
        {
            _settingsService.ErgodoxLayoutUrl = LayoutUrlContent;
            _settingsService.HotkeyShowLayout = DisplayLayoutHotkey;

            _settingsService.Save();

            UpdateButtonCanExecute();
        }

        private void CancelSettings()
        {
            _settingsService.Cancel();

            LayoutUrlContent = _settingsService.ErgodoxLayoutUrl;
            DisplayLayoutHotkey = _settingsService.HotkeyShowLayout;
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