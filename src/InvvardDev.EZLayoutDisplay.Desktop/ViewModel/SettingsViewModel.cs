using System;
using System.Collections.ObjectModel;
using System.Linq;
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
        #region Constants

        private const string TagSearchBaseUri = "https://configure.ergodox-ez.com/{0}/search?q={1}";
        private const string DefaultLatestRevisionId = "latest";

        #endregion

        #region Fields

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly ISettingsService _settingsService;
        private readonly IWindowService _windowService;
        private readonly ILayoutService _layoutService;
        private readonly IProcessService _processService;

        private ICommand _openTagSearchCommand;
        private ICommand _downloadHexFileCommand;
        private ICommand _downloadSourcesCommand;
        private ICommand _applySettingsCommand;
        private ICommand _updateLayoutCommand;
        private ICommand _closeSettingsCommand;
        private ICommand _cancelSettingsCommand;

        private string _currentLayoutHashId;
        private string _currentLayoutRevisionId;
        private string _layoutTitle;
        private string _keyboardModel;
        private string _layoutStatus;
        private ObservableCollection<string> _tags;
        private ObservableCollection<string> _layers;
        private string _hexFileUri;
        private string _sourcesZipUri;
        private bool _layoutIsCompiled;
        private string _keyboardGeometry;

        private string _altModifierLabel;
        private string _ctrlModifierLabel;
        private string _shiftModifierLabel;
        private string _windowsModifierLabel;

        private string _layoutUrlContent;
        private Hotkey _hotkeyDisplayLayout;

        #endregion

        #region Relay commands

        /// <summary>
        /// Open tag search command.
        /// </summary>
        public ICommand OpenTagSearchCommand =>
            _openTagSearchCommand
            ?? (_openTagSearchCommand = new RelayCommand<string>(OpenTagSearchUrl));

        /// <summary>
        /// Download HEX file.
        /// </summary>
        public ICommand DownloadHexFileCommand =>
            _downloadHexFileCommand
            ?? (_downloadHexFileCommand = new RelayCommand(DownloadHexFile, LayoutIsCompiled));

        /// <summary>
        /// Download Sources ZIP.
        /// </summary>
        public ICommand DownloadSourcesCommand =>
            _downloadSourcesCommand
            ?? (_downloadSourcesCommand = new RelayCommand(DownloadSources, LayoutIsCompiled));

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

        public string WindowTitle { get; set; }

        public string LayoutInfoGroupName { get; set; }

        public string LayoutTitleLabel { get; set; }

        public string KeyboardModelLabel { get; set; }

        public string TagsLabel { get; set; }

        public string StatusLabel { get; set; }

        public string LayersLabel { get; set; }

        public string HexFileCommandLabel { get; set; }

        public string SourcesZipCommandLabel { get; set; }

        public string ApplyCommandLabel { get; set; }

        public string UpdateCommandLabel { get; set; }

        public string CloseCommandLabel { get; set; }

        public string CancelCommandLabel { get; set; }

        public string LayoutUrlLabel { get; set; }

        public string LayoutUrlContent
        {
            get => _layoutUrlContent;
            set
            {
                if (!Set(ref _layoutUrlContent, value)) return;

                UpdateButtonCanExecute();
                UpdateErgoDoxInfo();
            }
        }

        public string LayoutTitle
        {
            get => _layoutTitle;
            private set => Set(ref _layoutTitle, value);
        }

        public string KeyboardModel
        {
            get => _keyboardModel;
            private set => Set(ref _keyboardModel, value);
        }

        public string LayoutStatus
        {
            get => _layoutStatus;
            private set => Set(ref _layoutStatus, value);
        }

        public ObservableCollection<string> Tags
        {
            get => _tags ?? (_tags = new ObservableCollection<string>());
            private set => Set(ref _tags, value);
        }

        public ObservableCollection<string> Layers
        {
            get => _layers ?? (_layers = new ObservableCollection<string>());
            private set => Set(ref _layers, value);
        }

        public string HotkeyTitleLabel { get; set; }

        public string AltModifierLabel
        {
            get => _altModifierLabel;
            private set => Set(ref _altModifierLabel, value);
        }

        public string CtrlModifierLabel
        {
            get => _ctrlModifierLabel;
            private set => Set(ref _ctrlModifierLabel, value);
        }

        public string ShiftModifierLabel
        {
            get => _shiftModifierLabel;
            private set => Set(ref _shiftModifierLabel, value);
        }

        public string WindowsModifierLabel
        {
            get => _windowsModifierLabel;
            private set => Set(ref _windowsModifierLabel, value);
        }

        private Hotkey HotkeyShowLayout
        {
            get => _hotkeyDisplayLayout;
            set => Set(ref _hotkeyDisplayLayout, value);
        }

        public string CurrentLayoutHashId
        {
            get => _currentLayoutHashId;
            set => Set(ref _currentLayoutHashId, value);
        }

        public string CurrentLayoutRevisionId
        {
            get => _currentLayoutRevisionId;
            set => Set(ref _currentLayoutRevisionId, value);
        }

        #endregion

        #region Constructor

        public SettingsViewModel(ISettingsService settingsService, IWindowService windowService, ILayoutService layoutService, IProcessService processService)
        {
            Logger.TraceConstructor();

            _settingsService = settingsService;
            _windowService = windowService;
            _layoutService = layoutService;
            _processService = processService;

            SetLabelUi();
            SetDesignTimeLabelUi();

            SetSettingControls();
        }

        private void SetLabelUi()
        {
            WindowTitle = "Settings";
            LayoutUrlLabel = "Configurator URL to your layout :";

            LayoutInfoGroupName = "Layout information";
            LayoutTitleLabel = "Title :";
            KeyboardModelLabel = "Keyboard model :";
            TagsLabel = "Tags :";
            StatusLabel = "Layout status :";
            LayersLabel = "Layers :";
            HexFileCommandLabel = "HEX File";
            SourcesZipCommandLabel = "Sources zip";

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

        private void SetDesignTimeLabelUi()
        {
            if (!IsInDesignMode) return;

            LayoutTitle = "Layout title v1.0";
            KeyboardModel = "ErgoDox EZ Glow";
            Tags = new ObservableCollection<string>() {
                                                          "Tag 1",
                                                          "Tag 2"
                                                      };
            LayoutStatus = "Compiled";
            Layers = new ObservableCollection<string>() {
                                                            "Layer 1",
                                                            "Layer 2",
                                                            "Layer 3",
                                                            "Layer 4",
                                                            "Layer 5"
                                                        };
        }

        private void SetSettingControls()
        {
            LayoutUrlContent = _settingsService.ErgodoxLayoutUrl;
            HotkeyShowLayout = _settingsService.HotkeyShowLayout;
        }

        #endregion

        #region Command handlers

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

        private void OpenTagSearchUrl(string tag)
        {
            Logger.TraceRelayCommand();

            if (string.IsNullOrWhiteSpace(tag) || string.IsNullOrWhiteSpace(_keyboardGeometry))
            {
                return;
            }

            var tagSearchUri = string.Format(TagSearchBaseUri, _keyboardGeometry, tag);
            _processService.StartWebUrl(tagSearchUri);
        }

        private void DownloadHexFile()
        {
            Logger.TraceRelayCommand();

            _processService.StartWebUrl(_hexFileUri);
        }

        private void DownloadSources()
        {
            Logger.TraceRelayCommand();

            _processService.StartWebUrl(_sourcesZipUri);
        }

        private void UpdateButtonCanExecute()
        {
            Logger.TraceMethod();

            ((RelayCommand) ApplySettingsCommand).RaiseCanExecuteChanged();
            ((RelayCommand) CancelSettingsCommand).RaiseCanExecuteChanged();
        }

        private bool IsDirty()
        {
            var isDirty = _settingsService.ErgodoxLayoutUrl != _layoutUrlContent;

            return isDirty;
        }

        private bool LayoutIsCompiled()
        {
            return _layoutIsCompiled;
        }

        #endregion

        #region Private methods

        private async void UpdateErgoDoxInfo()
        {
            Logger.TraceMethod();

            (CurrentLayoutHashId, CurrentLayoutRevisionId) = ExtractLayoutUrlIds(LayoutUrlContent);

            try
            {
                var layoutInfo = await _layoutService.GetLayoutInfo(CurrentLayoutHashId, CurrentLayoutRevisionId);
                Logger.Debug("LayoutInfo = {@value0}", layoutInfo);

                ClearLayoutInfo();

                if (layoutInfo != null)
                {
                    CheckLatestRevisionId(layoutInfo);
                    UpdateLayoutInfo(layoutInfo);
                }
            }
            catch (ArgumentNullException anex)
            {
                Logger.Error(anex);
                _windowService.ShowWarning(anex.Message);
            }
            catch (ArgumentException aex)
            {
                Logger.Error(aex);
                _windowService.ShowWarning(aex.Message);
            }
        }

        private void CheckLatestRevisionId(ErgodoxLayout layoutInfo)
        {
            if (CurrentLayoutRevisionId.ToLower() != DefaultLatestRevisionId)
            {
                return;
            }

            CurrentLayoutRevisionId = layoutInfo.Revision.HashId;
        }

        private void ClearLayoutInfo()
        {
            Tags.Clear();
            Layers.Clear();

            LayoutTitle = "";
            KeyboardModel = "";
            LayoutStatus = "";

            _keyboardGeometry = "";
            _layoutIsCompiled = false;
        }

        private void UpdateLayoutInfo(ErgodoxLayout layoutInfo)
        {
            Logger.TraceMethod();

            LayoutTitle = layoutInfo.Title;
            _keyboardGeometry = layoutInfo.Geometry;

            if (layoutInfo.Tags?.Any() != null)
            {
                Tags = new ObservableCollection<string>(layoutInfo.Tags.Select(t => t.Name));
            }

            if (layoutInfo.Revision != null)
            {
                KeyboardModel = GetKeyBoardDescription(_keyboardGeometry, layoutInfo.Revision.Model);
                UpdateLayoutButtons(layoutInfo.Revision);
                LayoutStatus = !_layoutIsCompiled ? "Not compiled" : "Compiled";

                Layers = new ObservableCollection<string>(layoutInfo.Revision.Layers.Select(l => l.ToString()));
            }
        }

        private string GetKeyBoardDescription(string keyboardGeometry, string revisionModel)
        {
            string keyboardDescription;

            switch (keyboardGeometry)
            {
                case "ergodox-ez":
                    keyboardDescription = "ErgoDox EZ ";

                    break;
                case "planck-ez":
                    keyboardDescription = "Planck EZ ";

                    break;
                case "moonlander":
                    keyboardDescription = "Moonlander ";
                    break;
                default:
                    keyboardDescription = $"{keyboardGeometry} ";

                    break;
            }

            keyboardDescription += char.ToUpper(revisionModel[0]) + revisionModel.Substring(1);

            return keyboardDescription;
        }

        private void UpdateLayoutButtons(Revision revision)
        {
            Logger.TraceMethod();

            _layoutIsCompiled = Uri.IsWellFormedUriString(revision.HexUrl, UriKind.Absolute) && Uri.IsWellFormedUriString(revision.SourcesUrl, UriKind.Absolute);

            ((RelayCommand) DownloadHexFileCommand).RaiseCanExecuteChanged();
            ((RelayCommand) DownloadSourcesCommand).RaiseCanExecuteChanged();

            _hexFileUri = revision.HexUrl;
            _sourcesZipUri = revision.SourcesUrl;
        }

        private async Task UpdateLayout()
        {
            Logger.TraceMethod();

            try
            {
                var ergodoxLayout = await _layoutService.GetErgodoxLayout(CurrentLayoutHashId, CurrentLayoutRevisionId);
                Logger.Debug("ergodoxLayout = {@value0}", ergodoxLayout);

                var ezLayout = _layoutService.PrepareEZLayout(ergodoxLayout);
                Logger.Debug("ezLayout = {@value0}", ezLayout);

                _settingsService.EZLayout = ezLayout;
            }
            catch (ArgumentNullException anex)
            {
                Logger.Error(anex);
                _windowService.ShowWarning(anex.Message);
            }
            catch (ArgumentException aex)
            {
                Logger.Error(aex);
                _windowService.ShowWarning(aex.Message);
            }
        }

        private (string layoutHashId, string layoutRevisionId) ExtractLayoutUrlIds(string layoutUrl)
        {
            Logger.TraceMethod();

            var layoutHashIdGroupName = "layoutHashId";
            var layoutRevisionIdGroupName = "layoutRevisionId";
            var pattern =
                $"https://configure.ergodox-ez.com/(?:ergodox-ez|planck-ez|moonlander)/layouts/(?<{layoutHashIdGroupName}>default|[a-zA-Z0-9]{{4,}})(?:/(?<{layoutRevisionIdGroupName}>latest|[a-zA-Z0-9]+)(?:/[0-9]{{1,2}})?)?";
            var layoutHashId = "default";
            var layoutRevisionId = "latest";

            var regex = new Regex(pattern);
            var match = regex.Match(layoutUrl);

            if (match.Success)
            {
                layoutHashId = match.Groups[layoutHashIdGroupName].Value;

                var revisionId = match.Groups[layoutRevisionIdGroupName].Value;
                layoutRevisionId = string.IsNullOrWhiteSpace(revisionId) ? layoutRevisionId : revisionId;
            }

            Logger.Debug("Layout URL = {0}", layoutUrl);
            Logger.Debug("Layout Hash ID = {0}", layoutHashId);
            Logger.Debug("Layout Revision ID = {0}", layoutRevisionId);

            return (layoutHashId, layoutRevisionId);
        }

        #endregion
    }
}