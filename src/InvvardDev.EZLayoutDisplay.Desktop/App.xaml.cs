using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using GalaSoft.MvvmLight.Threading;
using InvvardDev.EZLayoutDisplay.Desktop.Helper;
using NLog;

namespace InvvardDev.EZLayoutDisplay.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private Mutex _mutex;

        public App()
        {
            DispatcherHelper.Initialize();
            DispatcherUnhandledException += OnDispatcherUnhandledException;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            EnforceSingleInstance();
            ProcessArgs(e.Args);
            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _mutex?.ReleaseMutex();

            base.OnExit(e);
        }

        protected void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Logger.Error(e.Exception, "Unhandled exception", sender);
            MessageBox.Show("Something went horribly wrong...\nBut I landed on my feet like a cat !\n\nCheck logs to get more details.",
                            "Almost crashed...",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);

            e.Handled = true;
        }

        private void EnforceSingleInstance()
        {
            _mutex = new Mutex(true, "{InvvardDev.EZLayoutDisplay.Desktop-7F8CC1C9-0D4B-4F75-828A-0F2F29925C06}", out var singleInstance);

            if (singleInstance) return;

            MessageBox.Show("EZ Layout Display is already running :)");
            _mutex = null;

            Current.Shutdown();
        }

        private void ProcessArgs(string[] args)
        {
            foreach (var arg in args)
            {
                switch (arg)
                {
                    case var val when val.StartsWith("-loglevel=", true, CultureInfo.InvariantCulture):
                        (string key, string value) = SplitArg(arg);

                        LogLevel level = LoggerHelper.GetLogLevel(value);
                        LoggerHelper.AdjustLogLevel(level);

                        break;
                }
            }
        }

        private (string, string) SplitArg(string arg)
        {
            var (key, value) = ("", "");
            var splitted = arg.Split('=');

            if (splitted.Length <= 1) return (key, value);

            key = splitted[0];
            value = splitted[1];

            return (key, value);
        }
    }
}