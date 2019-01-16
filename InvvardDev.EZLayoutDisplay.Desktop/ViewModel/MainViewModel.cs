using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using InvvardDev.EZLayoutDisplay.Desktop.Model.Service.Interface;
using InvvardDev.EZLayoutDisplay.Desktop.View;

namespace InvvardDev.EZLayoutDisplay.Desktop.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        #region Fields

        private ICommand _showLayoutCommand;
        private ICommand _showSettingsCommand;
        private ICommand _exitCommand;

        private readonly IWindowService _windowService;

        private string _trayMenuShowLayoutCommandLabel;
        private string _trayMenuShowSettingsCommandLabel;
        private string _trayMenuExitCommandLabel;

        #endregion

        #region Public properties

        public string TrayMenuShowLayoutCommandLabel
        {
            get => _trayMenuShowLayoutCommandLabel;
            set => Set(ref _trayMenuShowLayoutCommandLabel, value);
        }

        public string TrayMenuShowSettingsCommandLabel
        {
            get => _trayMenuShowSettingsCommandLabel;
            set => Set(ref _trayMenuShowSettingsCommandLabel, value);
        }

        public string TrayMenuShowExitCommandLabel
        {
            get => _trayMenuExitCommandLabel;
            set => Set(ref _trayMenuExitCommandLabel, value);
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IWindowService windowService)
        {
            _windowService = windowService;

            SetLabelUi();
        }

        private void SetLabelUi()
        {
            TrayMenuShowLayoutCommandLabel = "Show Layout";
            TrayMenuShowSettingsCommandLabel = "Settings";
            TrayMenuShowExitCommandLabel = "Exit";
        }

        /// <summary>
        /// Shows the Layout window.
        /// </summary>
        public ICommand ShowLayoutCommand =>
            _showLayoutCommand
            ?? (_showLayoutCommand = new RelayCommand(() =>
                                                      {
                                                          _windowService.ShowWindow<DisplayLayoutWindow>();
                                                      }));

        /// <summary>
        /// Shows the Settings Window.
        /// </summary>
        public ICommand ShowSettingsCommand =>
            _showSettingsCommand
            ?? (_showSettingsCommand = new RelayCommand(() =>
                                                      {
                                                          _windowService.ShowWindow<SettingsWindow>();
                                                      }));

        /// <summary>
        /// Shuts down the application.
        /// </summary>
        public ICommand ExitApplicationCommand =>
            _exitCommand
            ?? (_exitCommand = new RelayCommand(() => Application.Current.Shutdown()));

        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}