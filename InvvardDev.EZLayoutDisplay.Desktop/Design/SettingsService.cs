using System.Diagnostics;
using InvvardDev.EZLayoutDisplay.Desktop.Model.Service.Interface;

namespace InvvardDev.EZLayoutDisplay.Desktop.Design
{
    public class SettingsService : ISettingsService
    {
        public void UpdateKey(string key, string value)
        {
            Debug.WriteLine($"{key} key updated.");
        }
    }
}