using System.Diagnostics;
using InvvardDev.EZLayoutDisplay.Desktop.Model.Service.Interface;

namespace InvvardDev.EZLayoutDisplay.Desktop.Design
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