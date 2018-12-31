using System;

using Overlogger;

namespace Overlogger.Console
{
    public class ConsoleReporter : IReporter
    {
        LogLevel _logLevel = LogLevel.Info;

        public void RegisterUser(string userId, string userName, IDictionary<string, string> otherInformation = null)
        {

        }

		public void UnregisterUser();

		public void SetLogLevel(LogLevel logLevel) =>
            _logLevel = logLevel;

		public void Log(LogLevel logLevel, string message, string tag = null)
        {
            if(logLevel >= _logLevel)
                System.Console.WriteLine($"{logLevel.ToString().ToUpper()}{((!string.IsNullOrEmpty(tag)) ? $" [{tag}]" : "" )} - {message}");
        }

		public void Log(LogLevel logLevel, string message, Exception exception, string tag = null)
        {
            if(logLevel >= _logLevel)
                System.Console.WriteLine($"{logLevel.ToString().ToUpper()}{((!string.IsNullOrEmpty(tag)) ? $" [{tag}]" : "" )} - {message}: {exception?.GetStackTrace()}");
        }

		public void TrackEvent(string eventName, IDictionary<string, string> properties = null, IDictionary<string, double> values = null) =>
            System.Console.WriteLine($"TRACK EVENT - {eventName} (Properties: {properties?.Count ?? 0}|Values: {values?.Count ?? 0}");
    }
}