using System;
using System.Collections.Generic;

namespace Overlogger
{
	/// <summary>
	/// Reporter.
	/// </summary>
	public interface IReporter
	{
		/// <summary>
		/// Sets the log level.
		/// </summary>
		/// <param name="logLevel">Log level.</param>
		void SetLogLevel(LogLevel logLevel);

		/// <summary>
		/// Gets the log level.
		/// </summary>
		/// <returns>The log level.</returns>
		LogLevel GetLogLevel();

		/// <summary>
		/// Log the specified logLevel, message and tag.
		/// </summary>
		/// <param name="logLevel">Log level.</param>
		/// <param name="message">Message.</param>
		/// <param name="tag">Tag.</param>
		void Log(LogLevel logLevel, string message, string tag = null);

		/// <summary>
		/// Log the specified logLevel, message, exception and tag.
		/// </summary>
		/// <param name="logLevel">Log level.</param>
		/// <param name="message">Message.</param>
		/// <param name="exception">Exception.</param>
		/// <param name="tag">Tag.</param>
		void Log(LogLevel logLevel, string message, Exception exception, string tag = null);

		/// <summary>
		/// Tracks the event.
		/// </summary>
		/// <param name="eventName">Event name.</param>
		/// <param name="properties">Properties.</param>
		/// <param name="values">Values.</param>
		void TrackEvent(string eventName, IDictionary<string, string> properties = null, IDictionary<string, double> values = null);
	}
}
