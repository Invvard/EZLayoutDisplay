using NLog;

namespace InvvardDev.EZLayoutDisplay.Desktop.Helper
{
    public class LoggerConfguration
    {
        internal static LogLevel GetLogLevel(string value)
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

        internal static void AdjustLogLevel(LogLevel logLevel)
        {
            var target = LogManager.Configuration.FindTargetByName("logfile");

            if (target != null)
            {
                LogManager.Configuration.AddRule(logLevel, LogLevel.Fatal, target);
            }
        }
    }
}