using System;
using System.Collections.Generic;

namespace Overlogger
{
	public interface IReporter
	{
		void RegisterUser(string userId, string userName, IDictionary<string, string> otherInformation = null);

		void UnregisterUser();

		void SetLogLevel(LogLevel logLevel);

		void Log(LogLevel logLevel, string message, string tag = null);

		void Log(LogLevel logLevel, string message, Exception exception, string tag = null);

		void TrackEvent(string eventName, IDictionary<string, string> properties = null, IDictionary<string, double> values = null);
	}
}
