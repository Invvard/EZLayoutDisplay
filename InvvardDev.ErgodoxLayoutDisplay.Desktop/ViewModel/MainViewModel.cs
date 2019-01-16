using GalaSoft.MvvmLight;
using InvvardDev.ErgodoxLayoutDisplay.Desktop.Service.Interface;

namespace InvvardDev.ErgodoxLayoutDisplay.Desktop.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private string _welcomeTitle = string.Empty;
        private IKeyboardListenerService _keyboardListenerService;

        /// <summary>
        /// Gets the WelcomeTitle property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string WelcomeTitle
        {
            get => _welcomeTitle;
            set => Set(ref _welcomeTitle, value);
        }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IKeyboardListenerService keyboardListenerService)
        {
            _keyboardListenerService = keyboardListenerService;
            WelcomeTitle = "Ergodox Layout Display";
        }
        
        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}