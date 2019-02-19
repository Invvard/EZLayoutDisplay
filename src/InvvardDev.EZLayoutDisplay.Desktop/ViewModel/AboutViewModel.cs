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
            WindowTitle = "About EZ Layout Display";
        }

        #endregion
    }
}