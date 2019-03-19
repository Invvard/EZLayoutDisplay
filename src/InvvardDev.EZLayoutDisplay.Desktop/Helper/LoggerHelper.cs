using System.Runtime.CompilerServices;
using NLog;

namespace InvvardDev.EZLayoutDisplay.Desktop.Helper
{
    public class LoggerHelper
    {
        internal static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        internal static void TraceMethod(string                      message          = "Method {0} called (line {1})",
                                         [ CallerMemberName ] string memberName       = "",
                                         [ CallerFilePath ]   string sourceFilePath   = "",
                                         [ CallerLineNumber ] int    sourceLineNumber = 0)
        {
            Logger.Trace(message, memberName, sourceLineNumber);
        }

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