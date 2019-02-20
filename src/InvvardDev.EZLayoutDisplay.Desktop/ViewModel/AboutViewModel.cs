using System.Linq;
using System.Reflection;
using GalaSoft.MvvmLight;

namespace InvvardDev.EZLayoutDisplay.Desktop.ViewModel
{
    public class AboutViewModel : ViewModelBase
    {
        #region Fields

        private string _windowTitle;
        private string _appTitleLabel;
        private string _appVersionLabel;
        private string _createdTitleLabel;
        private string _basedOnTitleLabel;
        private string _projectWebsiteTitleLabel;
        private string _contactTitleLabel;
        private string _closeButtonLabel;

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

        public string ProjectWebsiteTitleLabel
        {
            get => _projectWebsiteTitleLabel;
            set => Set(ref _projectWebsiteTitleLabel, value);
        }

        public string ContactTitleLabel
        {
            get => _contactTitleLabel;
            set => Set(ref _contactTitleLabel, value);
        }

        public string CloseButtonLabel
        {
            get => _closeButtonLabel;
            set => Set(ref _closeButtonLabel, value);
        }

        #endregion

        #region Relay commands

        #endregion

        #region Constructor

        public AboutViewModel()
        {
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
            ProjectWebsiteTitleLabel = "Project website";
            ContactTitleLabel = "Contact";
            CloseButtonLabel = "OK";
        }

        private static string GetAppTitle()
        {
            var appTitle = "EZ Layout Display";

            var customAttributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false).FirstOrDefault();
            
            if (customAttributes is AssemblyTitleAttribute attribute)
            {
                appTitle = attribute.Title;
            }

            return appTitle;
        }

        #endregion
    }
}