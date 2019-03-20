using System.Runtime.CompilerServices;
using NLog;

namespace InvvardDev.EZLayoutDisplay.Desktop.Helper
{
    public static class LoggerHelper
    {
        internal static void TraceMethod(this Logger                 logger,
                                         string                      message          = "[Method] {0} (line {1})",
                                         [ CallerMemberName ] string memberName       = "",
                                         [ CallerFilePath ]   string sourceFilePath   = "",
                                         [ CallerLineNumber ] int    sourceLineNumber = 0)
        {
            logger.Trace(message, memberName, sourceLineNumber);
        }

        internal static void TraceRelayCommand(this Logger                 logger,
                                               string                      message          = "[Relay Command] {0} (line {1})",
                                               [ CallerMemberName ] string memberName       = "",
                                               [ CallerFilePath ]   string sourceFilePath   = "",
                                               [ CallerLineNumber ] int    sourceLineNumber = 0)
        {
            logger.Trace(message, memberName, sourceLineNumber);
        }

        internal static void TraceConstructor(this Logger                 logger,
                                              string                      message          = "[Constructor] {0} (line {1})",
                                              [ CallerMemberName ] string memberName       = "",
                                              [ CallerFilePath ]   string sourceFilePath   = "",
                                              [ CallerLineNumber ] int    sourceLineNumber = 0)
        {
            logger.Trace(message, memberName, sourceLineNumber);
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