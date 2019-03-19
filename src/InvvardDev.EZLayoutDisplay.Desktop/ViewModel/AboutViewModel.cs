using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using InvvardDev.EZLayoutDisplay.Desktop.Service.Interface;
using InvvardDev.EZLayoutDisplay.Desktop.View;
using NLog;

namespace InvvardDev.EZLayoutDisplay.Desktop.ViewModel
{
    public class AboutViewModel : ViewModelBase
    {
        #region Fields

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly IWindowService _windowService;

        private string _windowTitle;
        private string _appTitleLabel;
        private string _appVersionLabel;
        private string _createdTitleLabel;
        private string _basedOnTitleLabel;
        private string _projectHomeTitleLabel;
        private string _contactTitleLabel;
        private string _creatorInfoLabel;
        private string _basedOnInfoLabel;
        private string _projectHomeInfoLabel;
        private string _contactInfoLabel;
        private string _closeButtonLabel;

        private readonly string _basedOnUrl;
        private readonly string _projectHomeUrl;
        private readonly string _contactUrl;

        private ICommand _navigateBasedOnUrlCommand;
        private ICommand _navigateProjectHomeUrlCommand;
        private ICommand _navigateContactUrlCommand;
        private ICommand _closeAboutCommand;

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

        public string AppTitleLabel
        {
            get => _appTitleLabel;
            set => Set(ref _appTitleLabel, value);
        }

        public string AppVersionLabel
        {
            get => _appVersionLabel;
            set => Set(ref _appVersionLabel, value);
        }

        public string CreatedTitleLabel
        {
            get => _createdTitleLabel;
            set => Set(ref _createdTitleLabel, value);
        }

        public string BasedOnTitleLabel
        {
            get => _basedOnTitleLabel;
            set => Set(ref _basedOnTitleLabel, value);
        }

        public string ProjectHomeTitleLabel
        {
            get => _projectHomeTitleLabel;
            set => Set(ref _projectHomeTitleLabel, value);
        }

        public string ContactTitleLabel
        {
            get => _contactTitleLabel;
            set => Set(ref _contactTitleLabel, value);
        }

        public string CreatorInfoLabel
        {
            get => _creatorInfoLabel;
            set => Set(ref _creatorInfoLabel, value);
        }

        public string BasedOnInfoLabel
        {
            get => _basedOnInfoLabel;
            set => Set(ref _basedOnInfoLabel, value);
        }

        public string ProjectHomeInfoLabel
        {
            get => _projectHomeInfoLabel;
            set => Set(ref _projectHomeInfoLabel, value);
        }

        public string ContactInfoLabel
        {
            get => _contactInfoLabel;
            set => Set(ref _contactInfoLabel, value);
        }

        public string CloseButtonLabel
        {
            get => _closeButtonLabel;
            set => Set(ref _closeButtonLabel, value);
        }

        #endregion

        #region Relay commands

        /// <summary>
        /// Navigate to based on URL command.
        /// </summary>
        public ICommand NavigateBasedOnUrlCommand =>
            _navigateBasedOnUrlCommand
            ?? (_navigateBasedOnUrlCommand = new RelayCommand(NavigateBasedOnUrl));

        /// <summary>
        /// Navigate to project home URL command.
        /// </summary>
        public ICommand NavigateProjectHomeUrlCommand =>
            _navigateProjectHomeUrlCommand
            ?? (_navigateProjectHomeUrlCommand = new RelayCommand(NavigateProjectHomeUrl));

        /// <summary>
        /// Navigate to contact URL command.
        /// </summary>
        public ICommand NavigateContactUrlCommand =>
            _navigateContactUrlCommand
            ?? (_navigateContactUrlCommand = new RelayCommand(NavigateContactUrl));

        /// <summary>
        /// Close about window command.
        /// </summary>
        public ICommand CloseAboutCommand =>
            _closeAboutCommand
            ?? (_closeAboutCommand = new RelayCommand(CloseAboutWindow));

        private void CloseAboutWindow()
        {
            _windowService.CloseWindow<AboutWindow>();
        }

        #endregion

        #region Constructor

        public AboutViewModel(IWindowService windowService)
        {
            Logger.Trace("Instanciate {0}", GetType());

            _windowService = windowService;

            _basedOnUrl = "https://configure.ergodox-ez.com/layouts/default/latest/0";
            _projectHomeUrl = "https://github.com/Invvard/EZLayoutDisplay";
            _contactUrl = "https://twitter.com/invvard";

            SetLabelUi();
        }

        #endregion

        #region Private methods

        private void SetLabelUi()
        {
            var appTitle = GetAppTitle();
            WindowTitle = $"About {appTitle}";
            AppTitleLabel = appTitle;
            AppVersionLabel = $"v.{Assembly.GetExecutingAssembly().GetName().Version}";
            CreatedTitleLabel = "Created by";
            BasedOnTitleLabel = "Based on";
            ProjectHomeTitleLabel = "Project home";
            ContactTitleLabel = "Contact";
            CreatorInfoLabel = "Pierre CAVAROC";
            BasedOnInfoLabel = "ErgoDox EZ Configurator";
            ProjectHomeInfoLabel = appTitle;
            ContactInfoLabel = "@Invvard";
            CloseButtonLabel = "OK";
        }

        private static string GetAppTitle()
        {
            Logger.Trace("Call {0} method", nameof(GetAppTitle));

            var appTitle = "EZ Layout Display";

            var customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false).FirstOrDefault();
            
            if (customAttributes is AssemblyTitleAttribute attribute)
            {
                appTitle = attribute.Title;
            }

            return appTitle;
        }

        private void NavigateBasedOnUrl()
        {
            Logger.Trace("Call {0} relay command", nameof(NavigateBasedOnUrl));
            Process.Start(_basedOnUrl);
        }

        private void NavigateProjectHomeUrl()
        {
            Logger.Trace("Call {0} relay command", nameof(NavigateProjectHomeUrl));
            Process.Start(_projectHomeUrl);
        }

        private void NavigateContactUrl()
        {
            Logger.Trace("Call {0} relay command", nameof(NavigateContactUrl));
            Process.Start(_contactUrl);
        }

        #endregion
    }
}