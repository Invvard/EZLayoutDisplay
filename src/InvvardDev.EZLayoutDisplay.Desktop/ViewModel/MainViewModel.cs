﻿using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using InvvardDev.EZLayoutDisplay.Desktop.Service.Interface;
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
        private readonly IApplicationService _applicationService;

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

        public string TrayMenuExitCommandLabel
        {
            get => _trayMenuExitCommandLabel;
            set => Set(ref _trayMenuExitCommandLabel, value);
        }

        #endregion

        #region Relay commands

        /// <summary>
        /// Shows the Layout window.
        /// </summary>
        public ICommand ShowLayoutCommand => _showLayoutCommand ?? (_showLayoutCommand = new RelayCommand(ShowLayoutWindow));

        /// <summary>
        /// Shows the Settings Window.
        /// </summary>
        public ICommand ShowSettingsCommand => _showSettingsCommand ?? (_showSettingsCommand = new RelayCommand(ShowSettingsWindow));

        /// <summary>
        /// Shuts down the application.
        /// </summary>
        public ICommand ExitApplicationCommand => _exitCommand ?? (_exitCommand = new RelayCommand(ShutdownApplication));

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IWindowService windowService, IApplicationService applicationService)
        {
            _windowService = windowService;
            _applicationService = applicationService;

            SetLabelUi();
        }

        private void SetLabelUi()
        {
            TrayMenuShowLayoutCommandLabel = "Show Layout";
            TrayMenuShowSettingsCommandLabel = "Settings";
            TrayMenuExitCommandLabel = "Exit";
        }

        #endregion

        #region Private methods

        private void ShowLayoutWindow()
        {
            _windowService.ShowWindow<DisplayLayoutWindow>();
        }

        private void ShowSettingsWindow()
        {
            _windowService.ShowWindow<SettingsWindow>();
        }

        private void ShutdownApplication()
        {
            _applicationService.ShutdownApplication();
        }

        #endregion

        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}