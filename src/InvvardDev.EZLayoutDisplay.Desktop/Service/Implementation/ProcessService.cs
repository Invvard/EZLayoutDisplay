using System.Diagnostics;
using InvvardDev.EZLayoutDisplay.Desktop.Service.Interface;

namespace InvvardDev.EZLayoutDisplay.Desktop.Service.Implementation
{
    // ReSharper disable once ClassNeverInstantiated.Global : Instantiated in ViewModelLocator
    public class ProcessService : IProcessService
    {
        public void StartWebUrl(string url)
        {
            Process.Start(url);
        }
    }
}