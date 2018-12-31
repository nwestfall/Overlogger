using System;
using System.Collections.Generic;

using Overlogger;
using Bugsnag;

namespace Overlogger.BugSnag
{
	/// <summary>
	/// Bug snag reporter.
	/// </summary>
    public partial class BugSnagReporter
    {
        const string LOG_TAG = "OVERLOGGER_BUGSNAG";

        IClient _bugsnagClient;

        LogLevel _logLevel;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Overlogger.BugSnag.BugSnagReporter"/> class.
		/// </summary>
		/// <param name="apiKey">API key.</param>
        public BugSnagReporter(string apiKey) =>
        	_bugsnagClient = new Client(new Configuration(apiKey));

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Overlogger.BugSnag.BugSnagReporter"/> class.
		/// </summary>
		/// <param name="configuration">Configuration.</param>
		public BugSnagReporter(IConfiguration configuration) =>
			_bugsnagClient = new Client(configuration);

		/// <summary>
		/// Sets the log level.
		/// </summary>
		/// <param name="logLevel">Log level.</param>
        public void SetLogLevel(LogLevel logLevel) =>
            _logLevel = logLevel;

		/// <summary>
		/// Gets the log level.
		/// </summary>
		/// <returns>The log level.</returns>
		public LogLevel GetLogLevel() =>
            _logLevel;

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
					if (!combinedProperties.ContainsKey(prop.Key))
						combinedProperties.Add(prop.Key, prop.Value);
				}
			}
			if(values != null)
			{
				foreach(var val in values)
				{
					if (!combinedProperties.ContainsKey(val.Key))
						combinedProperties.Add(val.Key, val.Value.ToString());
				}
			}

			_bugsnagClient.Breadcrumbs.Leave(eventName, BreadcrumbType.Manual, combinedProperties);
		}
	}
}