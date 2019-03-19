using System.Globalization;
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

        public App()
        {
            DispatcherHelper.Initialize();
            DispatcherUnhandledException += OnDispatcherUnhandledException;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            ProcessArgs(e.Args);
            base.OnStartup(e);
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

        private void ProcessArgs(string[] args)
        {
            foreach (var arg in args)
            {
                switch (arg)
                {
                    case var val when val.StartsWith("-loglevel=", true, CultureInfo.InvariantCulture):
                        (string key, string value) = SplitArg(arg);

                        LogLevel level = LoggerConfguration.GetLogLevel(value);
                        LoggerConfguration.AdjustLogLevel(level);

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