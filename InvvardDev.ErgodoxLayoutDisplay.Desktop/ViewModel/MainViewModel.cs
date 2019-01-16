using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using InvvardDev.ErgodoxLayoutDisplay.Desktop.Model.Service.Interface;
using InvvardDev.ErgodoxLayoutDisplay.Desktop.View;

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
        private ICommand _showLayoutWindowCommand;
        private ICommand _exitCommand;

        private IWindowService _windowService;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IWindowService windowService)
        {
            _windowService = windowService;
        }

        /// <summary>
        /// Shows a window, if none is already open.
        /// </summary>
        public ICommand ShowLayoutWindowCommand =>
            _showLayoutWindowCommand
            ?? (_showLayoutWindowCommand = new RelayCommand(() =>
                                                      {
                                                          _windowService.ShowWindow<DisplayLayoutWindow>();
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