using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using InvvardDev.EZLayoutDisplay.Desktop.Model;
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

        private ObservableCollection<KeyTemplate> _layoutTemplate;
        private ObservableCollection<EZKey> _currentLayerKeys;
        private int _currentLayerIndex;
        private EZLayout _ezLayout;

        private string _windowTitle;

        #endregion

        #region Properties

        public string WindowTitle
        {
            get => _windowTitle;
            set => Set(ref _windowTitle, value);
        }

        public ObservableCollection<KeyTemplate> LayoutTemplate
        {
            get => _layoutTemplate;
            set => Set(ref _layoutTemplate, value);
        }

        public ObservableCollection<EZKey> CurrentLayerKeys
        {
            get => _currentLayerKeys;
            set => Set(ref _currentLayerKeys, value);
        }

        #endregion

        #region Relay commands

        /// <summary>
        /// Lost focus command.
        /// </summary>
        public ICommand LostFocusCommand =>
            _lostFocusCommand
            ?? (_lostFocusCommand = new RelayCommand(LostFocus));

        #endregion

        public DisplayLayoutViewModel(IWindowService windowService, ILayoutService layoutService, ISettingsService settingsService)
        {
            _windowService = windowService;
            _layoutService = layoutService;
            _settingsService = settingsService;

            SetLabelUi();
            PopulateModel();
        }

        #region Private methods

        private void SetLabelUi()
        {
            WindowTitle = "Ergodox Layout";
        }

        private async void PopulateModel()
        {
            _currentLayerIndex = 0;

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

                _ezLayout = _settingsService.EZLayout;

                SwitchLayer();
            }
        }

        private void SwitchLayer()
        {
            var keys = _ezLayout.EZLayers.First(l => l.Index == _currentLayerIndex).EZKeys;

            for (int i = 0 ; i < LayoutTemplate.Count ; i++) { LayoutTemplate[i].EZKey = keys[i]; }
        }

        private void LostFocus()
        {
            _windowService.CloseWindow<DisplayLayoutWindow>();
        }

        #endregion
    }
}