using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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

        private ObservableCollection<KeyTemplate> _layoutTemplate;
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
        public ObservableCollection<KeyTemplate> LayoutTemplate
        {
            get => _layoutTemplate;
            set => Set(ref _layoutTemplate, value);
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

            Messenger.Default.Register<UpdatedLayoutMessage>(this, LoadLayout);

            SetLabelUi();
            LoadLayout(null);
        }

        #region Private methods

        private void SetLabelUi()
        {
            WindowTitle = "Ergodox Layout";
        }

        private async void PopulateModel()
        {
            CurrentLayerIndex = 0;

            if (IsInDesignModeStatic)
            {
                var json = Encoding.Default.GetString(Resources.layoutDefinition);

                var layoutDefinition = JsonConvert.DeserializeObject<IEnumerable<KeyTemplate>>(json);
                LayoutTemplate = new ObservableCollection<KeyTemplate>(layoutDefinition);
            }
            else
            {
                var definition = await _layoutService.GetLayoutTemplate();
                LayoutTemplate = new ObservableCollection<KeyTemplate>(definition);
            }

            _ezLayout = _settingsService.EZLayout;

            if (IsInDesignModeStatic || _ezLayout?.EZLayers != null && _ezLayout.EZLayers.Any() && _ezLayout.EZLayers.SelectMany(l => l.EZKeys).Any())
            {
                SwitchLayer();
            }
        }

        private void SwitchLayer()
        {
            List<EZKey> keys;

            if (IsInDesignModeStatic)
            {
                keys = new List<EZKey> {
                                           new EZKey {
                                                         Label = new KeyLabel("="),
                                                         Modifier = new KeyLabel("Left Shift"),
                                                         DisplayType = KeyDisplayType.ModifierOnTop,
                                                         KeyCategory = KeyCategory.DualFunction
                                                     }
                                       };

                for (int i = 0 ; i < LayoutTemplate.Count - 1 ; i++)
                {
                    keys.Add(new EZKey {
                                           Label = new KeyLabel("A \u2192"),
                                           Modifier = new KeyLabel("Left Shift")
                                       });
                }
            }
            else
            {
                keys = _ezLayout.EZLayers.First(l => l.Index == CurrentLayerIndex).EZKeys;
            }

            if (keys.Count == LayoutTemplate.Count)
            {
                for (int i = 0 ; i < LayoutTemplate.Count ; i++)
                {
                    LayoutTemplate[i].EZKey = keys[i];
                }
            }
        }

        private void LoadLayout(UpdatedLayoutMessage obj)
        {
            PopulateModel();
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
                case var exp when maxLayerIndex == 0:
                case var _ when CurrentLayerIndex >= maxLayerIndex:
                    CurrentLayerIndex = 0;

                    break;
                case var exp when CurrentLayerIndex < maxLayerIndex:
                    CurrentLayerIndex++;

                    break;
            }

            SwitchLayer();
        }

        #endregion
    }
}