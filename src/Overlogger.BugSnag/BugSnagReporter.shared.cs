using System;
using System.Collections.Generic;

using Overlogger;
using Bugsnag;

namespace Overlogger.BugSnag
{
    public partial class BugSnagReporter
    {
        const string LOG_TAG = "OVERLOGGER_BUGSNAG";

        Client _bugsnagClient;

        LogLevel _logLevel;

        public BugSnagReporter(string apiKey)
        {
            _bugsnagClient = new Client(new Configuration(apiKey));
        }

        public void SetLogLevel(LogLevel logLevel) =>
            _logLevel = logLevel;

		public LogLevel GetLogLevel() =>
            _logLevel;

		public void Log(LogLevel logLevel, string message, string tag = null)
        {
            if(_logLevel >= logLevel)
            {
                
            }
        }

		void Log(LogLevel logLevel, string message, Exception exception, string tag = null);

		void TrackEvent(string eventName, IDictionary<string, string> properties = null, IDictionary<string, double> values = null);
    }
}