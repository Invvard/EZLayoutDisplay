using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using InvvardDev.EZLayoutDisplay.Desktop.Helper;
using InvvardDev.EZLayoutDisplay.Desktop.Model;
using InvvardDev.EZLayoutDisplay.Desktop.Model.Messenger;
using InvvardDev.EZLayoutDisplay.Desktop.Service.Interface;
using InvvardDev.EZLayoutDisplay.Desktop.View;
using NLog;

namespace InvvardDev.EZLayoutDisplay.Desktop.ViewModel
{
    public class SettingsViewModel : ViewModelBase
    {
        #region Fields

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly ISettingsService _settingsService;
        private readonly IWindowService _windowService;
        private readonly ILayoutService _layoutService;

        private ICommand _applySettingsCommand;
        private ICommand _updateLayoutCommand;
        private ICommand _closeSettingsCommand;
        private ICommand _cancelSettingsCommand;

        private string _windowTitle;
        private string _applyCommandLabel;
        private string _updateCommandLabel;
        private string _closeCommandLabel;
        private string _cancelCommandLabel;
        private string _layoutUrlLabel;
        private string _hotkeyTitleLabel;
        private string _altModifierLabel;
        private string _ctrlModifierLabel;
        private string _shiftModifierLabel;
        private string _windowsModifierLabel;

        private string _layoutUrlContent;
        private Hotkey _hotkeyDisplayLayout;

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
        /// Update the layout from Ergodox website.
        /// </summary>
        public ICommand UpdateLayoutCommand =>
            _updateLayoutCommand
            ?? (_updateLayoutCommand = new RelayCommand(SaveSettings));

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

        public string UpdateCommandLabel
        {
            get => _updateCommandLabel;
            set => Set(ref _updateCommandLabel, value);
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
                if (Set(ref _layoutUrlContent, value))
                {
                    UpdateButtonCanExecute();
                }
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

        public Hotkey HotkeyShowLayout
        {
            get => _hotkeyDisplayLayout;
            set => Set(ref _hotkeyDisplayLayout, value);
        }

        #endregion

        #region Constructor

        public SettingsViewModel(ISettingsService settingsService, IWindowService windowService, ILayoutService layoutService)
        {
            Logger.TraceConstructor();

            _settingsService = settingsService;
            _windowService = windowService;
            _layoutService = layoutService;

            SetLabelUi();

            SetSettingControls();
        }

        private void SetLabelUi()
        {
            WindowTitle = "Settings";
            LayoutUrlLabel = "Configurator URL to your layout :";
            ApplyCommandLabel = "Apply";
            UpdateCommandLabel = "Update";
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
            HotkeyShowLayout = _settingsService.HotkeyShowLayout;
        }

        #endregion

        #region Private methods

        private async void SaveSettings()
        {
            Logger.TraceMethod();

            await UpdateLayout();

            _settingsService.ErgodoxLayoutUrl = LayoutUrlContent;
            _settingsService.HotkeyShowLayout = HotkeyShowLayout;

            _settingsService.Save();

            Messenger.Default.Send(new UpdatedLayoutMessage());

            UpdateButtonCanExecute();
        }

        private void CancelSettings()
        {
            Logger.TraceRelayCommand();

            _settingsService.Cancel();

            LayoutUrlContent = _settingsService.ErgodoxLayoutUrl;
            HotkeyShowLayout = _settingsService.HotkeyShowLayout;
        }

        private void CloseSettingsWindow()
        {
            Logger.TraceRelayCommand();

            if (IsDirty())
            {
                SaveSettings();
            }

            _windowService.CloseWindow<SettingsWindow>();
        }

        private void UpdateButtonCanExecute()
        {
            Logger.TraceMethod();

            ((RelayCommand) ApplySettingsCommand).RaiseCanExecuteChanged();
            ((RelayCommand) CancelSettingsCommand).RaiseCanExecuteChanged();
        }

        private bool IsDirty()
        {
            Logger.TraceMethod();

            var isDirty = _settingsService.ErgodoxLayoutUrl != _layoutUrlContent;

            return isDirty;
        }

        private async Task UpdateLayout()
        {
            Logger.TraceMethod();

            var layoutHashId = ExtractLayoutHashId(LayoutUrlContent);

            try
            {
                var ergodoxLayout = await _layoutService.GetErgodoxLayout(layoutHashId);
                var ezLayout = _layoutService.PrepareEZLayout(ergodoxLayout);
                _settingsService.EZLayout = ezLayout;
            }
            catch (ArgumentNullException)
            {
                throw;
            }
            catch (ArgumentException aex)
            {
                _windowService.ShowWarning(aex.Message);
            }
        }

        private string ExtractLayoutHashId(string layoutUrl)
        {
            Logger.TraceMethod();

            var layoutHashIdGroupName = "layoutHashId";
            var pattern = $"https://configure.ergodox-ez.com/layouts/(?<{layoutHashIdGroupName}>default|[a-zA-Z0-9]{{4}})(?:/latest/[0-9])?";
            var layoutHashId = "default";

            var regex = new Regex(pattern);
            var match = regex.Match(layoutUrl);

            if (match.Success)
            {
                layoutHashId = match.Groups[layoutHashIdGroupName].Value;
            }

            return layoutHashId;
        }

        #endregion
    }
}