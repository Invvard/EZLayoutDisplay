using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using InvvardDev.ErgodoxLayoutDisplay.Desktop.View;

namespace InvvardDev.ErgodoxLayoutDisplay.Desktop.ViewModel
{
    public class NotifyIconViewModel : ViewModelBase
    {
        private ICommand _showWindowCommand;
        private ICommand _hideWindowCommand;
        private ICommand _exitCommand;

        /// <summary>
        /// Shows a window, if none is already open.
        /// </summary>
        public ICommand ShowWindowCommand =>
            _showWindowCommand
            ?? (_showWindowCommand = new RelayCommand(() => {
                                                          Application.Current.MainWindow = new MainWindow();
                                                          Application.Current.MainWindow.Show();
                                                      },
                                                      () => Application.Current.MainWindow == null));

        /// <summary>
        /// Hides the main window. This command is only enabled if a window is open.
        /// </summary>
        public ICommand HideWindowCommand =>
            _hideWindowCommand
            ?? (_hideWindowCommand = new RelayCommand(() => Application.Current.MainWindow.Close(),
                                                      () => Application.Current.MainWindow != null));

        /// <summary>
        /// Shuts down the application.
        /// </summary>
        public ICommand ExitApplicationCommand =>
            _exitCommand
            ?? (_exitCommand = new RelayCommand(() => Application.Current.Shutdown()));
    }
}