using System.Diagnostics;
using InvvardDev.EZLayoutDisplay.Desktop.Model;
using InvvardDev.EZLayoutDisplay.Desktop.Service.Interface;
using NonInvasiveKeyboardHookLibrary;

namespace InvvardDev.EZLayoutDisplay.Desktop.Service.Design
{
    public class SettingsService : ISettingsService
    {
        public Hotkey HotkeyShowLayout { get; set; }
        public string ErgodoxLayoutUrl { get; set; }
        public EZLayout EZLayout { get; set; }

        public SettingsService()
        {
            HotkeyShowLayout = new Hotkey(0x60, ModifierKeys.Alt);
            ErgodoxLayoutUrl = "https://configure.zsa.io/ergodox-ez/layouts/default/latest/0";
        }

        public void Save()
        {
            Debug.WriteLine("Settings saved.");
        }

        public void Cancel()
        {
            Debug.WriteLine("Settings canceled.");
        }
    }
}