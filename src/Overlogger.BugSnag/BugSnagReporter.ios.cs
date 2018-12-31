using System;

namespace Overlogger.BugSnag
{
	/// <summary>
	/// iOS Bug snag reporter.
	/// </summary>
	public partial class BugSnagReporter : IReporter
	{
		/// <summary>
		/// Log the specified logLevel, message and tag.
		/// </summary>
		/// <param name="logLevel">Log level.</param>
		/// <param name="message">Message.</param>
		/// <param name="tag">Tag.</param>
		public void Log(LogLevel logLevel, string message, string tag = null)
		{
			if (_logLevel >= logLevel)
			{
				message = (string.IsNullOrEmpty(tag)) ? message : $"[{tag}] {message}";
				Console.WriteLine($"{LOG_TAG} {logLevel.ToString()} {message}");
			}
		}

		/// <summary>
		/// Log the specified logLevel, message, exception and tag.
		/// </summary>
		/// <param name="logLevel">Log level.</param>
		/// <param name="message">Message.</param>
		/// <param name="exception">Exception.</param>
		/// <param name="tag">Tag.</param>
		public void Log(LogLevel logLevel, string message, Exception exception, string tag = null)
		{
			if (_logLevel >= logLevel)
			{
				message = (string.IsNullOrEmpty(tag)) ? message : $"[{tag}] {message}";
				switch (logLevel)
				{
					case LogLevel.Verbose:
					case LogLevel.Debug:
						Console.WriteLine($"{LOG_TAG} {logLevel.ToString()} {message}");
						break;
					case LogLevel.Info:
						Console.WriteLine($"{LOG_TAG} {logLevel.ToString()} {message}");
						try
						{
							_bugsnagClient.Notify(exception, Bugsnag.Severity.Info);
						}
						catch (Exception ex)
						{
							Console.WriteLine($"{LOG_TAG} Error while reporting back to Bugsnag - {ex.Message}");
						}
						break;
					case LogLevel.Warn:
						Console.WriteLine($"{LOG_TAG} {logLevel.ToString()} {message}");
						try
						{
							_bugsnagClient.Notify(exception, Bugsnag.Severity.Warning);
						}
						catch (Exception ex)
						{
							Console.WriteLine($"{LOG_TAG} Error while reporting back to Bugsnag - {ex.Message}");
						}
						break;
					case LogLevel.Error:
						Console.WriteLine($"{LOG_TAG} {logLevel.ToString()} {message}");
						try
						{
							_bugsnagClient.Notify(exception, Bugsnag.Severity.Error);
						}
						catch (Exception ex)
						{
							Console.WriteLine($"{LOG_TAG} Error while reporting back to Bugsnag - {ex.Message}");
						}
						break;
				}
			}
		}
	}
}
