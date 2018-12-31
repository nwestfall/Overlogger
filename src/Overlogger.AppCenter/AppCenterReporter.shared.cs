using System;
using System.Collections.Generic;

using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace Overlogger.AppCenter
{
	/// <summary>
	/// Shared App center reporter.
	/// </summary>
    public partial class AppCenterReporter
    {
        const string LOG_TAG = "OVERLOGGER_APPCENTER";

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Overlogger.AppCenter.AppCenterReporter"/> class.
		/// </summary>
		/// <param name="appId">App identifier.</param>
        public AppCenterReporter(string appId) =>
            Microsoft.AppCenter.AppCenter.Start(appId, typeof(Analytics), typeof(Crashes)); 

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Overlogger.AppCenter.AppCenterReporter"/> class.
		/// </summary>
		/// <param name="androidAppId">Android app identifier.</param>
		/// <param name="iosAppId">Ios app identifier.</param>
        public AppCenterReporter(string androidAppId, string iosAppId)
        {
            if(string.IsNullOrEmpty(androidAppId) && string.IsNullOrEmpty(iosAppId))
                throw new ArgumentException("You must pass at least 1 app id");
            
            Microsoft.AppCenter.AppCenter.Start($"{(string.IsNullOrEmpty(iosAppId) ? "" : $"ios={iosAppId};")}{(string.IsNullOrEmpty(androidAppId) ? "" : $"android={androidAppId};")}", typeof(Analytics), typeof(Crashes));
        }

		/// <summary>
		/// Gets the log level.
		/// </summary>
		/// <returns>The log level.</returns>
        public LogLevel GetLogLevel()
        {
            switch(Microsoft.AppCenter.AppCenter.LogLevel)
            {
                case Microsoft.AppCenter.LogLevel.Verbose:
                    return LogLevel.Verbose;
                case Microsoft.AppCenter.LogLevel.Debug:
                    return LogLevel.Debug;
                case Microsoft.AppCenter.LogLevel.Info:
                    return LogLevel.Info;
                case Microsoft.AppCenter.LogLevel.Warn:
                    return LogLevel.Warn;
                case Microsoft.AppCenter.LogLevel.Error:
                    return LogLevel.Error;
                default:
                    throw new KeyNotFoundException("Unable to map AppCenter.LogLevel to Overlogger.LogLevel");
            }
        }
    }
}