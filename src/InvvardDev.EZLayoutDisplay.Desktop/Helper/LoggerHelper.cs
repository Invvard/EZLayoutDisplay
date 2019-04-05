using System.Runtime.CompilerServices;
using NLog;

namespace InvvardDev.EZLayoutDisplay.Desktop.Helper
{
    public static class LoggerHelper
    {
        internal static void TraceMethod(this Logger                 logger,
                                         string                      message          = "[Method] {0} (line {1})",
                                         [ CallerMemberName ] string memberName       = "",
                                         [ CallerLineNumber ] int    sourceLineNumber = 0)
        {
            logger.Trace(message, memberName, sourceLineNumber);
        }

        internal static void TraceRelayCommand(this Logger                 logger,
                                               string                      message          = "[Relay Command] {0} (line {1})",
                                               [ CallerMemberName ] string memberName       = "",
                                               [ CallerLineNumber ] int    sourceLineNumber = 0)
        {
            logger.Trace(message, memberName, sourceLineNumber);
        }

        internal static void TraceConstructor(this Logger                 logger,
                                              string                      message          = "[Constructor] {0} (line {1})",
                                              [ CallerMemberName ] string memberName       = "",
                                              [ CallerLineNumber ] int    sourceLineNumber = 0)
        {
            logger.Trace(message, memberName, sourceLineNumber);
        }

        internal static void DebugInputParam(this Logger                 logger,
                                             string                      parameterName,
                                             object                      parameter,
                                             string                      message    = "[Input {method}] {parameterName} = {@parameterValue})",
                                             [ CallerMemberName ] string memberName = "")
        {
            DebugParam(logger, parameterName, parameter, message, memberName);
        }

        internal static void DebugOutputParam(this Logger                 logger,
                                              string                      parameterName,
                                              object                      parameter,
                                              string                      message    = "[Output {method}] {parameterName} = {@parameterValue})",
                                              [ CallerMemberName ] string memberName = "")
        {
            DebugParam(logger, parameterName, parameter, message, memberName);
        }

        private static void DebugParam(Logger logger,
                                       string parameterName,
                                       object parameter,
                                       string message,
                                       string memberName)
        {
            logger.Debug(message, memberName, parameterName, parameter);
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