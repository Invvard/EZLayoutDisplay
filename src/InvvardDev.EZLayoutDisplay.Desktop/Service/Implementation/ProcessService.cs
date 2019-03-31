using System.Diagnostics;
using InvvardDev.EZLayoutDisplay.Desktop.Service.Interface;

namespace InvvardDev.EZLayoutDisplay.Desktop.Service.Implementation
{
    public class ProcessService : IProcessService
    {
        public void StartWebUrl(string url)
        {
            Process.Start(url);
        }
    }
}