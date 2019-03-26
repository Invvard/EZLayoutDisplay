using System.Windows;
using InvvardDev.EZLayoutDisplay.Desktop.Helper;
using InvvardDev.EZLayoutDisplay.Desktop.Service.Interface;
using NLog;

namespace InvvardDev.EZLayoutDisplay.Desktop.Service.Implementation
{
    public class ApplicationService : IApplicationService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <inheritdoc />
        public void ShutdownApplication()
        { 
            Logger.TraceMethod();
            Application.Current.Shutdown();
        }
    }
}