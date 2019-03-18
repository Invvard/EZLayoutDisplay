using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using GalaSoft.MvvmLight.Threading;
using NLog;
using NLog.Targets;
using NLog.Targets.Wrappers;

namespace InvvardDev.EZLayoutDisplay.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        static App()
        {
            DispatcherHelper.Initialize();
        }

        void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // Process unhandled exception
            _logger.Error(e.Exception, "Unhandled exception");

            // Prevent default unhandled exception processing
            e.Handled = true;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            ProcessArgs(e.Args);
            base.OnStartup(e);
        }

        private void ProcessArgs(string[] args)
        {
            foreach (var arg in args)
            {
                switch (arg)
                {
                    case var val when val.StartsWith("-loglevel=", true, CultureInfo.InvariantCulture):
                        (string key, string value) = SplitArg(arg);

                        LogLevel level = GetLogLevel(value);
                        AdjustLogLevel(level);

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

        private LogLevel GetLogLevel(string value)
        {
            LogLevel level;

            switch (value.ToLower())
            {
                case "debug":
                    level = LogLevel.Debug;

                    break;
                case "trace":
                    level = LogLevel.Trace;

                    break;
                default:
                    level = LogLevel.Warn;

                    break;
            }

            return level;
        }

        private void AdjustLogLevel(LogLevel logLevel)
        {
            var target = LogManager.Configuration.FindTargetByName("logfile");

            if (target != null)
            {
                LogManager.Configuration.AddRule(logLevel, LogLevel.Fatal, target);
            }
        }
    }
}