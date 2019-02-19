using GalaSoft.MvvmLight;

namespace InvvardDev.EZLayoutDisplay.Desktop.ViewModel
{
    public class AboutViewModel : ViewModelBase
    {
        #region Fields

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