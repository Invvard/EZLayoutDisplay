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
            ?? (_nextLayerCommand = new RelayCommand(NextLayer));

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
            WindowTitle = "Ergodox Layout";
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

            var layoutDefinition = await LoadLayoutDefinition() as List<KeyTemplate>;
            _layoutTemplates = new List<List<KeyTemplate>>();

            if (IsInDesignModeStatic // in DesignMode, every thing is already set
                || layoutDefinition == null
                || _ezLayout?.EZLayers == null
                || !_ezLayout.EZLayers.Any()
                || !_ezLayout.EZLayers.SelectMany(l => l.EZKeys).Any())
            {
                return;
            }

            await PopulateLayoutTemplates();

            SwitchLayer();
        }

        private void LoadDesignTimeModel()
        {
            var json = Encoding.Default.GetString(Resources.layoutDefinition);
            var layoutDefinition = JsonConvert.DeserializeObject<IEnumerable<KeyTemplate>>(json) as List<KeyTemplate>;

            Debug.Assert(layoutDefinition != null, nameof(layoutDefinition) + " != null");

            // ReSharper disable once UseObjectOrCollectionInitializer
            CurrentLayoutTemplate = new ObservableCollection<KeyTemplate>(layoutDefinition);
            CurrentLayoutTemplate[0].EZKey = new EZKey
            {
                Label = new KeyLabel("="),
                Modifier = new KeyLabel("Left Shift"),
                DisplayType = KeyDisplayType.ModifierOnTop,
                KeyCategory = KeyCategory.DualFunction
            };

            for (int i = 1; i < CurrentLayoutTemplate.Count; i++)
            {
                CurrentLayoutTemplate[i].EZKey = new EZKey
                {
                    Label = new KeyLabel("A \u2192"),
                    Modifier = new KeyLabel("Left Shift")
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
                var layoutTemplate = await LoadLayoutDefinition() as List<KeyTemplate>;

                if (layoutTemplate != null)
                {
                    for (int j = 0 ; j < layoutTemplate.Count ; j++)
                    {
                        layoutTemplate[j].EZKey = t.EZKeys[j];
                    }
                }

                _layoutTemplates.Add(layoutTemplate);
            }
        }

        private void SwitchLayer()
        {
            CurrentLayoutTemplate.Clear();
            CurrentLayoutTemplate = new ObservableCollection<KeyTemplate>(_layoutTemplates[CurrentLayerIndex]);
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