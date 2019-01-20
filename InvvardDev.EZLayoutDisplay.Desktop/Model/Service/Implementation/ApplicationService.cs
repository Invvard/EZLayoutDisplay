using System.Windows;
using InvvardDev.EZLayoutDisplay.Desktop.Model.Service.Interface;

namespace InvvardDev.EZLayoutDisplay.Desktop.Model.Service.Implementation
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