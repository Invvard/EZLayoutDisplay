using System.Windows.Threading;
using GalaSoft.MvvmLight.Threading;
using NLog;

namespace InvvardDev.EZLayoutDisplay.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        static App()
        {
            DispatcherHelper.Initialize();
        }

        void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // Process unhandled exception
            _logger.Error(e.Exception, "Unhandled exception");

            // Prevent default unhandled exception processing
            e.Handled = true;
        }
    }
}
