using System.Windows;
using InvvardDev.EZLayoutDisplay.Desktop.Service.Interface;

namespace InvvardDev.EZLayoutDisplay.Desktop.Service.Implementation
{
    public class ApplicationService : IApplicationService
    {
        /// <inheritdoc />
        public void ShutdownApplication()
        {
            Application.Current.Shutdown();
        }
    }
}