using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using InvvardDev.EZLayoutDisplay.Desktop.Helper;
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
        private readonly IProcessService _processService;

        private string _windowTitle;
        private string _appTitleLabel;
        private string _appVersionLabel;
        private string _createdTitleLabel;
        private string _basedOnTitleLabel;
        private string _projectHomeTitleLabel;
        private string _contactTitleLabel;
        private string _creatorInfoLabel;
        private string _contributorsLabel;
        private string _basedOnInfoLabel;
        private string _projectHomeInfoLabel;
        private string _twitterInfoLabel;
        private string _redditInfoLabel;
        private string _vladZahariaLabel;
        private string _closeButtonLabel;

        private readonly string _basedOnUrl;
        private readonly string _projectHomeUrl;
        private readonly string _twitterUrl;
        private readonly string _redditUrl;
        private readonly string _githubVZUrl;

        private ICommand _navigateBasedOnUrlCommand;
        private ICommand _navigateProjectHomeUrlCommand;
        private ICommand _navigateTwitterUrlCommand;
        private ICommand _navigateRedditUrlCommand;
        private ICommand _navigateGithubVZUrlCommand;
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
            get => $"{_appVersionLabel}-Beta";
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

        public string ContributorsTitleLabel
        {
            get => _contributorsLabel;
            set => Set(ref _contributorsLabel, value);
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

        public string TwitterInfoLabel
        {
            get => _twitterInfoLabel;
            set => Set(ref _twitterInfoLabel, value);
        }

        public string RedditInfoLabel
        {
            get => _redditInfoLabel;
            set => Set(ref _redditInfoLabel, value);
        }

        public string VladZahariaLabel
        {
            get => _vladZahariaLabel;
            set => Set(ref _vladZahariaLabel, value);
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
        /// Navigate to Twitter URL command.
        /// </summary>
        public ICommand NavigateTwitterUrlCommand =>
            _navigateTwitterUrlCommand
            ?? (_navigateTwitterUrlCommand = new RelayCommand(NavigateTwitterUrl));

        /// <summary>
        /// Navigate to Reddit URL command.
        /// </summary>
        public ICommand NavigateRedditUrlCommand =>
            _navigateRedditUrlCommand
            ?? (_navigateRedditUrlCommand = new RelayCommand(NavigateRedditUrl));

        /// <summary>
        /// Navigate to Vlad Zaharia Github URL command.
        /// </summary>
        public ICommand NavigateGithubVZUrlCommand =>
            _navigateGithubVZUrlCommand
            ?? (_navigateGithubVZUrlCommand = new RelayCommand(NavigateGithubVZUrl));

        /// <summary>
        /// Close about window command.
        /// </summary>
        public ICommand CloseAboutCommand =>
            _closeAboutCommand
            ?? (_closeAboutCommand = new RelayCommand(CloseAboutWindow));

        #endregion

        #region Constructor

        public AboutViewModel(IWindowService windowService, IProcessService processService)
        {
            Logger.TraceConstructor();

            _windowService = windowService;
            _processService = processService;

            _basedOnUrl = "https://configure.zsa.io/ergodox-ez/";
            _projectHomeUrl = "https://github.com/Invvard/EZLayoutDisplay";
            _twitterUrl = "https://twitter.com/invvard";
            _redditUrl = "https://www.reddit.com/r/EZLayoutDisplay/";
            _githubVZUrl = "https://github.com/vladzaharia";

            SetLabelUi();
        }

        #endregion

        #region Private methods

        private void SetLabelUi()
        {
            var appTitle = GetAppTitle();
            var appVersion = GetAppVersion();
            WindowTitle = $"About {appTitle}";
            AppTitleLabel = appTitle;
            AppVersionLabel = $"v{appVersion}";
            CreatedTitleLabel = "Created by";
            BasedOnTitleLabel = "Based on";
            ProjectHomeTitleLabel = "Project home";
            ContactTitleLabel = "Contact";
            CreatorInfoLabel = "Pierre CAVAROC";
            BasedOnInfoLabel = "ErgoDox EZ Configurator";
            ProjectHomeInfoLabel = appTitle;
            TwitterInfoLabel = "@Invvard";
            RedditInfoLabel = "r/EZLayoutDisplay";
            ContributorsTitleLabel = "Contributors";
            VladZahariaLabel = "Vlad Zaharia";
            CloseButtonLabel = "OK";
        }

        private string GetAppVersion()
        {
            var assembly = Assembly.GetExecutingAssembly();
            string version = FileVersionInfo.GetVersionInfo(assembly.Location).FileVersion;

            return version;
        }

        private static string GetAppTitle()
        {
            Logger.TraceMethod();

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
            Logger.TraceRelayCommand();
            _processService.StartWebUrl(_basedOnUrl);
        }

        private void NavigateProjectHomeUrl()
        {
            Logger.TraceRelayCommand();
            _processService.StartWebUrl(_projectHomeUrl);
        }

        private void NavigateTwitterUrl()
        {
            Logger.TraceRelayCommand();
            _processService.StartWebUrl(_twitterUrl);
        }

        private void NavigateRedditUrl()
        {
            Logger.TraceRelayCommand();
            _processService.StartWebUrl(_redditUrl);
        }

        private void NavigateGithubVZUrl()
        {
            Logger.TraceRelayCommand();
            _processService.StartWebUrl(_githubVZUrl);
        }

        private void CloseAboutWindow()
        {
            Logger.TraceRelayCommand();
            _windowService.CloseWindow<AboutWindow>();
        }

        #endregion
    }
}