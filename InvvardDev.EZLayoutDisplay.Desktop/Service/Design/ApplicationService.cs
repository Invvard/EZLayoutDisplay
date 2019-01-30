using System.Diagnostics;
using InvvardDev.EZLayoutDisplay.Desktop.Service.Interface;

namespace InvvardDev.EZLayoutDisplay.Desktop.Service.Design
{
    public class ApplicationService : IApplicationService
    {
        /// <inheritdoc />
        public void ShutdownApplication()
        {
            Debug.WriteLine("Application is closing.");
        }
    }
}