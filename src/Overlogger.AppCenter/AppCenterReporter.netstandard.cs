using System;
using System.Collections.Generic;

namespace Overlogger.AppCenter
{
	/// <summary>
	/// NETStandard App center reporter.
	/// </summary>
	public partial class AppCenterReporter : IReporter
	{
		/// <summary>
		/// Sets the log level.
		/// </summary>
		/// <param name="logLevel">Log level.</param>
	    public void SetLogLevel(LogLevel logLevel) =>
	          throw new NotImplementedInReferenceAssemblyException();

		/// <summary>
		/// Log the specified logLevel, message and tag.
		/// </summary>
		/// <param name="logLevel">Log level.</param>
		/// <param name="message">Message.</param>
		/// <param name="tag">Tag.</param>
	    public void Log(LogLevel logLevel, string message, string tag = null) =>
	          throw new NotImplementedInReferenceAssemblyException();

		/// <summary>
		/// Log the specified logLevel, message, exception and tag.
		/// </summary>
		/// <param name="logLevel">Log level.</param>
		/// <param name="message">Message.</param>
		/// <param name="exception">Exception.</param>
		/// <param name="tag">Tag.</param>
	    public void Log(LogLevel logLevel, string message, Exception exception, string tag = null) =>
	          throw new NotImplementedInReferenceAssemblyException();

		/// <summary>
		/// Tracks the event.
		/// </summary>
		/// <param name="eventName">Event name.</param>
		/// <param name="properties">Properties.</param>
		/// <param name="values">Values.</param>
	    public void TrackEvent(string eventName, IDictionary<string, string> properties = null, IDictionary<string, double> values = null) =>
	          throw new NotImplementedInReferenceAssemblyException();
	}
}