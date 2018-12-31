using System;
using System.Collections.Generic;

using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace Overlogger.AppCenter
{
	/// <summary>
	/// Android App center reporter.
	/// </summary>
    public partial class AppCenterReporter : IReporter
    {
		/// <summary>
		/// Sets the log level.
		/// </summary>
		/// <param name="logLevel">Log level.</param>
        public void SetLogLevel(LogLevel logLevel)
        {
            switch (logLevel)
            {
				case LogLevel.Verbose:
					Microsoft.AppCenter.AppCenter.LogLevel = Microsoft.AppCenter.LogLevel.Verbose;
					break;
				case LogLevel.Debug:
                    Microsoft.AppCenter.AppCenter.LogLevel = Microsoft.AppCenter.LogLevel.Debug;
                    break;
                case LogLevel.Info:
                    Microsoft.AppCenter.AppCenter.LogLevel = Microsoft.AppCenter.LogLevel.Info;
                    break;
                case LogLevel.Warn:
                    Microsoft.AppCenter.AppCenter.LogLevel = Microsoft.AppCenter.LogLevel.Warn;
                    break;
				case LogLevel.Error:
					Microsoft.AppCenter.AppCenter.LogLevel = Microsoft.AppCenter.LogLevel.Error;
					break;
				default:
                    throw new ArgumentException("Log Level not mapped to App Center");
            }
        }

		/// <summary>
		/// Log the specified logLevel, message and tag.
		/// </summary>
		/// <param name="logLevel">Log level.</param>
		/// <param name="message">Message.</param>
		/// <param name="tag">Tag.</param>
		public void Log(LogLevel logLevel, string message, string tag = null)
        {
            message = (string.IsNullOrEmpty(tag)) ? message : $"[{tag}] {message}";

            switch(logLevel)
            {
				case LogLevel.Verbose:
					Android.Util.Log.Verbose(LOG_TAG, message);
					break;
				case LogLevel.Debug:
                    Android.Util.Log.Debug(LOG_TAG, message);
                    break;
                case LogLevel.Info:
                    Android.Util.Log.Info(LOG_TAG, message);
                    break;
                case LogLevel.Warn:
                    Android.Util.Log.Warn(LOG_TAG, message);
                    break;
				case LogLevel.Error:
					Android.Util.Log.Error(LOG_TAG, message);
					break;
			}
        }

		/// <summary>
		/// Log the specified logLevel, message, exception and tag.
		/// </summary>
		/// <param name="logLevel">Log level.</param>
		/// <param name="message">Message.</param>
		/// <param name="exception">Exception.</param>
		/// <param name="tag">Tag.</param>
		public void Log(LogLevel logLevel, string message, System.Exception exception, string tag = null)
        {
            if(exception != null)
                message += string.Format(": {0}", exception.GetStackTrace());
            message = (string.IsNullOrEmpty(tag)) ? message : $"[{tag}] {message}";

            switch(logLevel)
            {
				case LogLevel.Verbose:
					Android.Util.Log.Verbose(LOG_TAG, message);
					break;
				case LogLevel.Debug:
                    Android.Util.Log.Debug(LOG_TAG, message);
                    break;
                case LogLevel.Info:
                    Android.Util.Log.Info(LOG_TAG, message);
                    break;
                case LogLevel.Warn:
                    Android.Util.Log.Warn(LOG_TAG, message);
                    break;
				case LogLevel.Error:
					Android.Util.Log.Error(LOG_TAG, message);
					try
					{
						Crashes.TrackError(exception, null); // TODO: Other properties
					}
					catch (System.Exception ex)
					{
						Android.Util.Log.Error(LOG_TAG, $"Error while reporting back to App Center - {ex.Message}");
						// TODO: What to do?
					}
					break;
			}
        }

		/// <summary>
		/// Tracks the event.
		/// </summary>
		/// <param name="eventName">Event name.</param>
		/// <param name="properties">Properties.</param>
		/// <param name="values">Values.</param>
		public void TrackEvent(string eventName, IDictionary<string, string> properties = null, IDictionary<string, double> values = null)
        {
            IDictionary<string, string> combinedProperties = new Dictionary<string, string>();
            if(properties != null)
            {
                foreach(var prop in properties)
                {
                    if(!combinedProperties.ContainsKey(prop.Key))
                        combinedProperties.Add(prop.Key, prop.Value);
                }
            }
            if(values != null)
            {
                foreach(var val in values)
                {
                    if(!combinedProperties.ContainsKey(val.Key))
                        combinedProperties.Add(val.Key, val.Value.ToString());
                }
            }

            Analytics.TrackEvent(eventName, combinedProperties);
        }
    }
}