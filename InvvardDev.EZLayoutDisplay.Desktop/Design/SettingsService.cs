using System.Diagnostics;
using InvvardDev.EZLayoutDisplay.Desktop.Model.Service.Interface;

namespace InvvardDev.EZLayoutDisplay.Desktop.Design
{
    public class SettingsService : ISettingsService
    {
        public void UpdateSetting(string key, string value)
        {
            Debug.WriteLine($"{key} setting updated.");
        }

        public string GetSetting(string key)
        {
            string value = "";
            switch (key)
            {
                case "ErgodoxLayoutUrl":
                    value = "https://configure.ergodox-ez.com/layouts/default/latest/0";
                    break;
            }

            Debug.WriteLine($"Get {key} setting : {value}");

            return value;
        }
    }
}