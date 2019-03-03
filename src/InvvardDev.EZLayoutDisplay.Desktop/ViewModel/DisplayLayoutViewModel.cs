using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using InvvardDev.EZLayoutDisplay.Desktop.Model;
using InvvardDev.EZLayoutDisplay.Desktop.Model.Enum;
using InvvardDev.EZLayoutDisplay.Desktop.Model.Messenger;
using InvvardDev.EZLayoutDisplay.Desktop.Properties;
using InvvardDev.EZLayoutDisplay.Desktop.Service.Interface;
using InvvardDev.EZLayoutDisplay.Desktop.View;
using Newtonsoft.Json;

namespace InvvardDev.EZLayoutDisplay.Desktop.ViewModel
{
    public class DisplayLayoutViewModel : ViewModelBase
    {
        #region Fields

        private readonly IWindowService _windowService;
        private readonly ILayoutService _layoutService;
        private readonly ISettingsService _settingsService;

        private ICommand _lostFocusCommand;
        private ICommand _nextLayerCommand;

        private List<List<KeyTemplate>> _layoutTemplates;
        private ObservableCollection<KeyTemplate> _currentLayoutTemplate;
        private int _currentLayerIndex;
        private EZLayout _ezLayout;

        private string _windowTitle;
        private bool _noLayoutAvailable;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the window title.
        /// </summary>
        public string WindowTitle
        {
            get => _windowTitle;
            set => Set(ref _windowTitle, value);
        }

        /// <summary>
        /// Gets or sets the no layout available indicator.
        /// </summary>
        public bool NoLayoutAvailable
        {
            get => _noLayoutAvailable;
            set => Set(ref _noLayoutAvailable, value);
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
            set => Set(ref _currentLayerIndex, value);
        }

        #endregion

        #region Relay commands

        /// <summary>
        /// Lost focus command.
        /// </summary>
        public ICommand LostFocusCommand =>
            _lostFocusCommand
            ?? (_lostFocusCommand = new RelayCommand(LostFocus));

        /// <summary>
        /// Next layer command.
        /// </summary>
        public ICommand NextLayerCommand =>
            _nextLayerCommand
            ?? (_nextLayerCommand = new RelayCommand(NextLayer, NextLayerCanExecute));

        #endregion

        public DisplayLayoutViewModel(IWindowService windowService, ILayoutService layoutService, ISettingsService settingsService)
        {
            _windowService = windowService;
            _layoutService = layoutService;
            _settingsService = settingsService;

            Messenger.Default.Register<UpdatedLayoutMessage>(this, LoadCompleteLayout);

            CurrentLayoutTemplate = new ObservableCollection<KeyTemplate>();

            SetLabelUi();
            LoadCompleteLayout();
        }

        #region Private methods

        private void SetLabelUi()
        {
            WindowTitle = "ErgoDox Layout";
        }

        private async void LoadCompleteLayout()
        {
            CurrentLayerIndex = 0;

            if (IsInDesignModeStatic)
            {
                LoadDesignTimeModel();

                return;
            }

            _ezLayout = _settingsService.EZLayout;

            _layoutTemplates = new List<List<KeyTemplate>>();

            if (_ezLayout?.EZLayers == null
                || !_ezLayout.EZLayers.Any()
                || !_ezLayout.EZLayers.SelectMany(l => l.EZKeys).Any())
            {
                NoLayoutAvailable = true;
                return;
            }

            NoLayoutAvailable = false;
            await PopulateLayoutTemplates();

            SwitchLayer();
        }

        private void LoadDesignTimeModel()
        {
            NoLayoutAvailable = false;

            var json = Encoding.Default.GetString(Resources.layoutDefinition);
            var layoutDefinition = JsonConvert.DeserializeObject<IEnumerable<KeyTemplate>>(json) as List<KeyTemplate>;

            Debug.Assert(layoutDefinition != null, nameof(layoutDefinition) + " != null");

            // ReSharper disable once UseObjectOrCollectionInitializer
            CurrentLayoutTemplate = new ObservableCollection<KeyTemplate>(layoutDefinition);
            CurrentLayoutTemplate[0].EZKey = new EZKey {
                                                           Label = new KeyLabel("="),
                                                           Modifier = new KeyLabel("Left Shift"),
                                                           DisplayType = KeyDisplayType.ModifierOnTop,
                                                           KeyCategory = KeyCategory.DualFunction
                                                       };

            CurrentLayoutTemplate[1].EZKey = new EZKey {
                                                           Label = new KeyLabel("LT \u2192 1"),
                                                           DisplayType = KeyDisplayType.SimpleLabel,
                                                           KeyCategory = KeyCategory.DualFunction
                                                       };

            for (int i = 2 ; i < CurrentLayoutTemplate.Count ; i++)
            {
                CurrentLayoutTemplate[i].EZKey = new EZKey {
                                                               Label = new KeyLabel("E"),
                                                               Modifier = new KeyLabel("Left Shift"),
                                                               KeyCategory = KeyCategory.French,
                                                               InternationalHint = "fr"
                };
            }
        }

        private async Task<IEnumerable<KeyTemplate>> LoadLayoutDefinition()
        {
            var layoutDefinition = await _layoutService.GetLayoutTemplate();

            return layoutDefinition;
        }

        private async Task PopulateLayoutTemplates()
        {
            foreach (var t in _ezLayout.EZLayers)
            {
                if (!(await LoadLayoutDefinition() is List<KeyTemplate> layoutTemplate)) break;

                for (int j = 0 ; j < layoutTemplate.Count ; j++)
                {
                    layoutTemplate[j].EZKey = t.EZKeys[j];
                }

                _layoutTemplates.Add(layoutTemplate);
            }
        }

        private void SwitchLayer()
        {
            if (_layoutTemplates.Any())
            {
                CurrentLayoutTemplate = new ObservableCollection<KeyTemplate>(_layoutTemplates[CurrentLayerIndex]);
            }
        }


        #region Delegates

        private void LoadCompleteLayout(UpdatedLayoutMessage obj)
        {
            LoadCompleteLayout();
        }

        private void LostFocus()
        {
            _windowService.CloseWindow<DisplayLayoutWindow>();
        }

        private bool NextLayerCanExecute()
        {
            var canExecute = _layoutTemplates.Any();

            return canExecute;
        }

        private void NextLayer()
        {
            var maxLayerIndex = _ezLayout.EZLayers.Count - 1;

            switch (CurrentLayerIndex)
            {
                case var _ when maxLayerIndex == 0:
                case var _ when CurrentLayerIndex >= maxLayerIndex:
                    CurrentLayerIndex = 0;

                    break;
                case var _ when CurrentLayerIndex < maxLayerIndex:
                    CurrentLayerIndex++;

                    break;
            }

            SwitchLayer();
        }

        #endregion

        #endregion
    }
}