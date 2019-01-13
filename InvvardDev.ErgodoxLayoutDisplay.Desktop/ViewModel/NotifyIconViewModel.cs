using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using InvvardDev.ErgodoxLayoutDisplay.Desktop.View;
using System.Windows;
using System.Windows.Input;

namespace InvvardDev.ErgodoxLayoutDisplay.Desktop.ViewModel
{
    public class NotifyIconViewModel : ViewModelBase
    {
        private ICommand _showWindowCommand;
        private ICommand _exitCommand;

        /// <summary>
        /// Shows a window, if none is already open.
        /// </summary>
        public ICommand ShowWindowCommand =>
            _showWindowCommand
            ?? (_showWindowCommand = new RelayCommand(() =>
            {
                if (Application.Current.MainWindow == null)
                {
                    Application.Current.MainWindow = new MainWindow();
                }
                Application.Current.MainWindow.Show();
            }));

        /// <summary>
        /// Shuts down the application.
        /// </summary>
        public ICommand ExitApplicationCommand =>
            _exitCommand
            ?? (_exitCommand = new RelayCommand(() => Application.Current.Shutdown()));
    }
}