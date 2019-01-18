using InvvardDev.EZLayoutDisplay.Desktop.Model;
using InvvardDev.EZLayoutDisplay.Desktop.Model.Service.Interface;

namespace InvvardDev.EZLayoutDisplay.Desktop.Design
{
    public class SettingsService : ISettingsService
    {
        public Hotkey GetHotKeyShowLayout()
        {
            var hotkey = new Hotkey(0x60);

            return hotkey;
        }

        public string GetErgodoxLayoutUrl()
        {
            var url = "https://configure.ergodox-ez.com/layouts/default/latest/0";

            return url;
        }
    }
}