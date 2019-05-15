﻿using System;
using System.Collections.Generic;
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
        #region Fields

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly ISettingsService _settingsService;
        private readonly IWindowService _windowService;
        private readonly ILayoutService _layoutService;

        private ICommand _applySettingsCommand;
        private ICommand _updateLayoutCommand;
        private ICommand _closeSettingsCommand;
        private ICommand _cancelSettingsCommand;

        private string _layoutTitle;
        private string _keyboardModel;
        private string _layoutStatus;
        private ObservableCollection<string> _tags;
        private ObservableCollection<string> _layers;
        private Uri _hexFileUri;
        private Uri _sourcesZipUri;
        private bool _layoutIsCompiled;

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
            get => _tags;
            private set => Set(ref _tags, value);
        }

        public ObservableCollection<string> Layers
        {
            get => _layers;
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

        #endregion

        #region Constructor

        public SettingsViewModel(ISettingsService settingsService, IWindowService windowService, ILayoutService layoutService)
        {
            Logger.TraceConstructor();

            _settingsService = settingsService;
            _windowService = windowService;
            _layoutService = layoutService;

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
            if (IsInDesignMode)
            {
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
            var isDirty = _settingsService.ErgodoxLayoutUrl != _layoutUrlContent;

            return isDirty;
        }

        private async void UpdateErgoDoxInfo()
        {
            Logger.TraceMethod();

            var layoutHashId = ExtractLayoutHashId(LayoutUrlContent);

            try
            {
                var layoutInfo = await _layoutService.GetLayoutInfo(layoutHashId);
                Logger.Debug("LayoutInfo = {@value0}", layoutInfo);
                UpdateLayoutInfo(layoutInfo);
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

        private void UpdateLayoutInfo(ErgodoxLayout layoutInfo)
        {
            Logger.TraceMethod();

            LayoutTitle = layoutInfo.Title;

            if (layoutInfo.Tags != null && layoutInfo.Tags.Any())
            {
                Tags = new ObservableCollection<string>(layoutInfo.Tags.Select(t => t.Name));
            }

            if (layoutInfo.Revisions.Any())
            {
                var revision = layoutInfo.Revisions.First();

                KeyboardModel = revision.Model;

                UpdateLayoutButtons(revision);

                Layers = new ObservableCollection<string>(revision.Layers.Select(l => l.ToString()));
            }
        }

        private void UpdateLayoutButtons(Revision revision)
        {
            Logger.TraceMethod();

            _layoutIsCompiled = Uri.IsWellFormedUriString(revision.HexUrl, UriKind.Absolute) && Uri.IsWellFormedUriString(revision.SourcesUrl, UriKind.Absolute);

            if (!_layoutIsCompiled)
            {
                LayoutStatus = "Not compiled";

                return;
            }

            LayoutStatus = "Compiled";
            _hexFileUri = new Uri(revision.HexUrl);
            _sourcesZipUri = new Uri(revision.SourcesUrl);
        }

        private async Task UpdateLayout()
        {
            Logger.TraceMethod();

            var layoutHashId = ExtractLayoutHashId(LayoutUrlContent);

            try
            {
                var ergodoxLayout = await _layoutService.GetErgodoxLayout(layoutHashId);
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

        private string ExtractLayoutHashId(string layoutUrl)
        {
            Logger.TraceMethod();

            var layoutHashIdGroupName = "layoutHashId";
            var pattern = $"https://configure.ergodox-ez.com/ergodox-ez/layouts/(?<{layoutHashIdGroupName}>default|[a-zA-Z0-9]{{4,}})(?:/latest/[0-9])?";
            var layoutHashId = "default";

            var regex = new Regex(pattern);
            var match = regex.Match(layoutUrl);

            if (match.Success)
            {
                layoutHashId = match.Groups[layoutHashIdGroupName].Value;
            }

            Logger.Debug("Layout URL = {0}", layoutUrl);
            Logger.Debug("Layout Hash ID = {0}", layoutHashId);

            return layoutHashId;
        }

        #endregion
    }
}