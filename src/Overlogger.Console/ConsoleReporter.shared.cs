using System;
using System.Collections.Generic;

using Overlogger;

namespace Overlogger.Console
{
    /// <summary>
    /// Console Reporter
    /// </summary>
    public class ConsoleReporter : IReporter
    {
        LogLevel _logLevel = LogLevel.Info;

        /// <summary>
        /// Set the log level
        /// </summary>
        /// <param name="logLevel">The log level</param>
		public void SetLogLevel(LogLevel logLevel) =>
            _logLevel = logLevel;

        /// <summary>
        /// Get the log level
        /// </summary>
        public LogLevel GetLogLevel() =>
            _logLevel;

        /// <summary>
        /// Log
        /// </summary>
        /// <param name="logLevel">The log level.</param>
        /// <param name="message">The message.</param>
        /// <param name="tag">The tag (optional).</param>
		public void Log(LogLevel logLevel, string message, string tag = null)
        {
			if (logLevel >= _logLevel)
			{
				SetConsoleBackgroundColorFromLogLevel(logLevel); 
				System.Console.WriteLine($"{logLevel.ToString().ToUpper()}{((!string.IsNullOrEmpty(tag)) ? $" [{tag}]" : "")} - {message}");
				System.Console.ResetColor();
			}
        }

        /// <summary>
        /// Log
        /// </summary>
        /// <param name="logLevel">The log level.</param>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="tag">The tag (optional).</param>
		public void Log(LogLevel logLevel, string message, Exception exception, string tag = null)
        {
			if (logLevel >= _logLevel)
			{
				SetConsoleBackgroundColorFromLogLevel(logLevel);
				System.Console.WriteLine($"{logLevel.ToString().ToUpper()}{((!string.IsNullOrEmpty(tag)) ? $" [{tag}]" : "")} - {message}: {exception?.GetStackTrace()}");
				System.Console.ResetColor();
				System.Console.WriteLine(); // Color does something weird with exceptions
			}
        }

        /// <summary>
        /// Track the event
        /// </summary>
        /// <param name="eventName">The event name.</param>
        /// <param name="properties">The properties.</param>
        /// <param name="values">The values.</param>
		public void TrackEvent(string eventName, IDictionary<string, string> properties = null, IDictionary<string, double> values = null) =>
            System.Console.WriteLine($"TRACK EVENT - {eventName} (Properties: {properties?.Count ?? 0}|Values: {values?.Count ?? 0}");

		void SetConsoleBackgroundColorFromLogLevel(LogLevel logLevel)
		{
			switch(logLevel)
			{
				case LogLevel.Warn:
					System.Console.BackgroundColor = ConsoleColor.Yellow;
					break;
				case LogLevel.Error:
					System.Console.BackgroundColor = ConsoleColor.Red;
					break;
				default:
					System.Console.ResetColor();
					break;
			}
		}
	}
}