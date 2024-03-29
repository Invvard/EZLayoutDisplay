﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using InvvardDev.EZLayoutDisplay.Desktop.Helper;
using InvvardDev.EZLayoutDisplay.Desktop.Model;
using InvvardDev.EZLayoutDisplay.Desktop.Model.Enum;
using InvvardDev.EZLayoutDisplay.Desktop.Model.Ez.Content;
using InvvardDev.EZLayoutDisplay.Desktop.Model.Messenger;
using InvvardDev.EZLayoutDisplay.Desktop.Properties;
using InvvardDev.EZLayoutDisplay.Desktop.Service.Interface;
using InvvardDev.EZLayoutDisplay.Desktop.View;
using Newtonsoft.Json;
using NLog;
using Key = InvvardDev.EZLayoutDisplay.Desktop.Model.Ez.Key;

namespace InvvardDev.EZLayoutDisplay.Desktop.ViewModel
{
    public class DisplayLayoutViewModel : ViewModelBase
    {
        #region Fields

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IWindowService _windowService;
        private readonly ILayoutService _layoutService;
        private readonly ISettingsService _settingsService;

        private ICommand _lostFocusCommand;
        private ICommand _hideWindowCommand;
        private ICommand _nextLayerCommand;
        private ICommand _scrollLayerCommand;

        private List<List<KeyTemplate>> _layoutTemplates;
        private ObservableCollection<KeyTemplate> _currentLayoutTemplate;
        private int _currentLayerIndex;
        private EZLayout _ezLayout;

        private bool _isWindowPinned;

        private string _windowTitle;
        private string _noLayoutWarningFirstLine;
        private string _noLayoutWarningSecondLine;
        private string _currentLayerNameTitle;
        private string _currentLayerName;
        private string _controlHintSpaceLabel;
        private string _controlHintEscapeLabel;
        private string _toggleBtnPinWindowContent;
        private string _toggleBtnPinWindowTooltip;
        private bool _noLayoutAvailable;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the window title.
        /// </summary>
        public string WindowTitle
        {
            get => _windowTitle;
            private set => Set(ref _windowTitle, value);
        }

        /// <summary>
        /// Gets or sets the no layout warning first line.
        /// </summary>
        public string NoLayoutWarningFirstLine
        {
            get => _noLayoutWarningFirstLine;
            private set => Set(ref _noLayoutWarningFirstLine, value);
        }

        /// <summary>
        /// Gets or sets the no layout warning second line.
        /// </summary>
        public string NoLayoutWarningSecondLine
        {
            get => _noLayoutWarningSecondLine;
            private set => Set(ref _noLayoutWarningSecondLine, value);
        }

        /// <summary>
        /// Gets or sets the current layer name title.
        /// </summary>
        public string CurrentLayerNameTitle
        {
            get => _currentLayerNameTitle;
            private set => Set(ref _currentLayerNameTitle, value);
        }

        /// <summary>
        /// Gets or sets the current layer name.
        /// </summary>
        public string CurrentLayerName
        {
            get => _currentLayerName;
            private set => Set(ref _currentLayerName, value);
        }

        /// <summary>
        /// Gets or sets the control hint label for the Space bar.
        /// </summary>
        public string ControlHintSpaceLabel
        {
            get => _controlHintSpaceLabel;
            private set => Set(ref _controlHintSpaceLabel, value);
        }

        /// <summary>
        /// Gets or sets the control hint label for the Space bar.
        /// </summary>
        public string ControlHintEscapeLabel
        {
            get => _controlHintEscapeLabel;
            private set => Set(ref _controlHintEscapeLabel, value);
        }

        /// <summary>
        /// Gets or sets the Pin window toggle button label.
        /// </summary>
        public string ToggleBtnPinWindowContent
        {
            get => _toggleBtnPinWindowContent;
            private set => Set(ref _toggleBtnPinWindowContent, value);
        }

        /// <summary>
        /// Gets or sets the Pin window toggle button label.
        /// </summary>
        public string ToggleBtnPinWindowTooltip
        {
            get => _toggleBtnPinWindowTooltip;
            private set => Set(ref _toggleBtnPinWindowTooltip, value);
        }

        /// <summary>
        /// Gets or sets the no layout available indicator.
        /// </summary>
        public bool NoLayoutAvailable
        {
            get => _noLayoutAvailable;
            private set => Set(ref _noLayoutAvailable, value);
        }

        /// <summary>
        /// Gets or sets the layout template.
        /// </summary>
        public ObservableCollection<KeyTemplate> CurrentLayoutTemplate
        {
            get => _currentLayoutTemplate;
            set => Set(ref _currentLayoutTemplate, value);
        }

        /// <summary>
        /// Gets or sets the current layer index.
        /// </summary>
        public int CurrentLayerIndex
        {
            get => _currentLayerIndex;
            private set => Set(ref _currentLayerIndex, value);
        }

        /// <summary>
        /// Gets or sets the pinned status.
        /// </summary>
        public bool IsWindowPinned
        {
            get => _isWindowPinned;
            set => Set(ref _isWindowPinned, value);
        }

        #endregion

        #region Relay commands

        /// <summary>
        /// Lost focus command.
        /// </summary>
        public ICommand LostFocusCommand =>
            _lostFocusCommand ??= new RelayCommand(LostFocus, LostFocusCanExecute);

        /// <summary>
        /// Hide window command.
        /// </summary>
        public ICommand HideWindowCommand =>
            _hideWindowCommand ??= new RelayCommand(LostFocus);

        /// <summary>
        /// Next layer command.
        /// </summary>
        public ICommand NextLayerCommand =>
            _nextLayerCommand ??= new RelayCommand(NextLayer, NextLayerCanExecute);

        /// <summary>
        /// Next layer command.
        /// </summary>
        public ICommand ScrollLayerCommand =>
            _scrollLayerCommand ??= new RelayCommand<MouseWheelEventArgs>(ScrollLayer);

        #endregion

        public DisplayLayoutViewModel(IWindowService windowService, ILayoutService layoutService, ISettingsService settingsService)
        {
            Logger.TraceConstructor();

            _windowService = windowService;
            _layoutService = layoutService;
            _settingsService = settingsService;

            Messenger.Default.Register<UpdatedLayoutMessage>(this, LoadCompleteLayout);

            CurrentLayoutTemplate = new ObservableCollection<KeyTemplate>();

            SetLabelUi();
            SetWindowParameters();
            LoadCompleteLayout();
        }

        #region Private methods

        private void SetLabelUi()
        {
            WindowTitle = "ErgoDox Layout";
            CurrentLayerNameTitle = "Current layer :";
            CurrentLayerName = "";
            ControlHintSpaceLabel = "Scroll up/down or press 'Space' to display next layer";
            ControlHintEscapeLabel = "Press 'Escape' to hide window";
            ToggleBtnPinWindowContent = "_Pin window";
            ToggleBtnPinWindowTooltip = "Press 'P' to toggle";
        }

        private void SetWindowParameters()
        {
            IsWindowPinned = false;
        }

        private async void LoadCompleteLayout()
        {
            Logger.TraceMethod();
            CurrentLayerIndex = 0;

            if (IsInDesignModeStatic)
            {
                LoadDesignTimeModel();

                return;
            }

            _ezLayout = _settingsService.EZLayout;
            Logger.Debug("EZLayout = {@value0}", _ezLayout);

            _layoutTemplates = new List<List<KeyTemplate>>();

            if (_ezLayout?.EZLayers == null
                || !_ezLayout.EZLayers.Any()
                || !_ezLayout.EZLayers.SelectMany(l => l.Keys).Any())
            {
                Logger.Info("No layout available");
                NoLayoutWarningFirstLine = "No layout available!";
                NoLayoutWarningSecondLine = "Please, go to the settings and update the layout.";
                NoLayoutAvailable = true;

                return;
            }
            else if (!_layoutService.SupportsGeometry(_ezLayout.Geometry))
            {
                Logger.Info("Geometry not supported");
                NoLayoutWarningFirstLine = "Not supported!";
                NoLayoutWarningSecondLine = "Sorry, your keyboard is not supported yet.";
                NoLayoutAvailable = true;

                return;
            }

            NoLayoutAvailable = false;

            await PopulateLayoutTemplates(_ezLayout.Geometry);

            SwitchLayer();
        }

        private void LoadDesignTimeModel()
        {
            Logger.TraceMethod();

            NoLayoutAvailable = false;
            CurrentLayerName = "Current Layer Name";

            var json = Encoding.Default.GetString(Resources.layoutDefinition);
            var layoutDefinition = JsonConvert.DeserializeObject<IEnumerable<KeyTemplate>>(json) as List<KeyTemplate>;

            Debug.Assert(layoutDefinition != null, nameof(layoutDefinition) + " != null");

            // ReSharper disable once UseObjectOrCollectionInitializer
            CurrentLayoutTemplate = new ObservableCollection<KeyTemplate>(layoutDefinition);
            CurrentLayoutTemplate[0].Key = new Key
            {
                Primary = new BaseContent { Label = "Shift + =" },
                DisplayMode = KeyDisplayMode.Base,
                GlowColor = "#111"
            };

            CurrentLayoutTemplate[1].Key = new Key
            {
                Primary = new Layer { Label = "\u2192", Id = 1 },
                Secondary = new Layer { Label = "\u2192", Id = 2 },
                DisplayMode = KeyDisplayMode.DualFunction,
                GlowColor = "#BBB"
            };

            for (int i = 2; i < CurrentLayoutTemplate.Count; i++)
            {
                CurrentLayoutTemplate[i].Key = new Key
                {
                    Primary = new BaseContent { Label = "Shift + E", Tag = "fr" },
                    DisplayMode = KeyDisplayMode.Base,
                    GlowColor = "#777"
                };
            }
        }

        private async Task PopulateLayoutTemplates(string geometry)
        {
            Logger.TraceMethod();

            foreach (var ezLayer in _ezLayout.EZLayers)
            {
                if (await LoadLayoutDefinition(geometry) is not List<KeyTemplate> layoutTemplate)
                    break;

                if (layoutTemplate.Count == 0) return;

                for (int j = 0; j < layoutTemplate.Count; j++)
                {
                    layoutTemplate[j].Key = ezLayer.Keys[j];
                }

                _layoutTemplates.Add(layoutTemplate);
            }
        }

        private async Task<IEnumerable<KeyTemplate>> LoadLayoutDefinition(string geometry)
        {
            Logger.TraceMethod();
            var layoutDefinition = await _layoutService.GetLayoutTemplate(geometry);

            return layoutDefinition;
        }

        private void SwitchLayer()
        {
            Logger.TraceMethod();
            Logger.Info("Switch to Layer {0} on {1}", CurrentLayerIndex, _layoutTemplates.Count - 1);

            if (_layoutTemplates.Any())
            {
                CurrentLayoutTemplate = new ObservableCollection<KeyTemplate>(_layoutTemplates[CurrentLayerIndex]);
                ChangeLayerName();
            }
        }

        private void ChangeLayerName()
        {
            CurrentLayerName = $"{_ezLayout.EZLayers[CurrentLayerIndex].Name} {_ezLayout.EZLayers[CurrentLayerIndex].Index}";
        }

        #region Delegates

        private void LoadCompleteLayout(UpdatedLayoutMessage obj)
        {
            Logger.TraceMethod("Intercept {0} message");
            LoadCompleteLayout();
        }

        private void LostFocus()
        {
            Logger.TraceRelayCommand();
            _windowService.CloseWindow<DisplayLayoutWindow>();
        }

        private void NextLayer()
        {
            Logger.TraceRelayCommand();
            VaryLayer(1);
        }

        private void ScrollLayer(MouseWheelEventArgs e)
        {
            Logger.TraceRelayCommand();

            var variation = e switch
            {
                { Delta: < 0 } => 1,
                { Delta: > 0 } => -1,
                _ => 0
            };

            VaryLayer(variation);
        }

        private void VaryLayer(int variation)
        {
            Logger.TraceRelayCommand();

            var maxLayerIndex = _ezLayout.EZLayers.Count - 1;

            CurrentLayerIndex = variation switch
            {
                < 0 when CurrentLayerIndex <= 0 => maxLayerIndex,
                < 0 when CurrentLayerIndex > 0 => --CurrentLayerIndex,
                > 0 when CurrentLayerIndex >= maxLayerIndex => 0,
                > 0 when CurrentLayerIndex < maxLayerIndex => ++CurrentLayerIndex,
                _ => CurrentLayerIndex
            };

            SwitchLayer();
        }

        private bool NextLayerCanExecute() => _layoutTemplates.Any();

        private bool LostFocusCanExecute() => !IsWindowPinned;

        #endregion

        #endregion
    }
}