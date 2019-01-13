using System;
using System.Windows;
using System.Windows.Input;

namespace InvvardDev.ErgodoxLayoutDisplay.Desktop.ViewModel
{
    public class NotifyIconViewModel
    {

        /// <summary>
        /// Shows a window, if none is already open.
        /// </summary>
        public ICommand ShowWindowCommand => new DelegateCommand
                                             {
                                                 CanExecuteFunc = () => Application.Current.MainWindow == null,
                                                 CommandAction = () => {
                                                                     Application.Current.MainWindow = new MainWindow();
                                                                     Application.Current.MainWindow.Show();
                                                                 }
                                             };

        /// <summary>
        /// Hides the main window. This command is only enabled if a window is open.
        /// </summary>
        public ICommand HideWindowCommand => new DelegateCommand
                                             {
                                                 CommandAction = () => Application.Current.MainWindow.Close(),
                                                 CanExecuteFunc = () => Application.Current.MainWindow != null
                                             };

        /// <summary>
        /// Shuts down the application.
        /// </summary>
        public ICommand ExitApplicationCommand
        {
            get
            {
                return new DelegateCommand { CommandAction = () => Application.Current.Shutdown() };
            }
        }
    }

    /// <summary>
    /// Simplistic delegate command for the demo.
    /// </summary>
    public class DelegateCommand : ICommand
    {
        public Action CommandAction { get; set; }
        public Func<bool> CanExecuteFunc { get; set; }

        public void Execute(object parameter)
        {
            CommandAction();
        }

        public bool CanExecute(object parameter)
        {
            return CanExecuteFunc == null || CanExecuteFunc();
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}