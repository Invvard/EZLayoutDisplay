using System.Windows;
using GalaSoft.MvvmLight.Threading;
using Hardcodet.Wpf.TaskbarNotification;
using InvvardDev.ErgodoxLayoutDisplay.Desktop.Service.Interface;

namespace InvvardDev.ErgodoxLayoutDisplay.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private TaskbarIcon _notifyIcon;
        private IKeyboardListenerService _keyboardListenerService;

        public App()
        {
            DispatcherHelper.Initialize();
            //_keyboardListenerService = new KeyboardListenerService();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            //create the notifyicon (it's a resource declared in NotifyIconResources.xaml
            _notifyIcon = (TaskbarIcon)FindResource("NotifyIcon");
            
        }

        protected override void OnExit(ExitEventArgs e)
        {
            //_keyboardListenerService.Dispose();
            _notifyIcon.Dispose(); //the icon would clean up automatically, but this is cleaner
            base.OnExit(e);
        }
    }
}
