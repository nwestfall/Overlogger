using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Linq;

namespace Overlogger
{
	/// <summary>
	/// Logger.
	/// </summary>
	public static class Logger
	{
		static ConcurrentQueue<(LogLevel logLevel, string message, Exception ex, string tag)> _pending = new ConcurrentQueue<(LogLevel logLevel, string message, Exception ex, string tag)>();

		static bool _logLevelSet = false;

		static LogLevel _logLevel;

		static IList<IReporter> _reporters = new List<IReporter>();

		static readonly object _reporterLock = new object();

		/// <summary>
		/// Gets the number of reporters.
		/// </summary>
		/// <value>The number of reporters.</value>
		public static int NumberOfReporters => _reporters?.Count ?? 0;

		#region Manage Crash Reporters
		/// <summary>
		/// Adds the crash reporter.
		/// </summary>
		/// <param name="reporter">Reporter.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static void AddCrashReporter<T>(T reporter) where T : IReporter
		{
			lock(_reporterLock)
			{
				if (_reporters.Any(r => r.GetType() == reporter.GetType()))
					throw new ArgumentException($"Reporter of type {reporter.GetType().Name} is already added", nameof(reporter));

				// Set log level if it was already set
				if(_logLevelSet)
					reporter.SetLogLevel(_logLevel);

				_reporters.Add(reporter);
			}
		}

		/// <summary>
		/// Tries the add crash reporter.
		/// </summary>
		/// <returns><c>true</c>, if add crash reporter was tryed, <c>false</c> otherwise.</returns>
		/// <param name="reporter">Reporter.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static bool TryAddCrashReporter<T>(T reporter) where T : IReporter
		{
			try
			{
				AddCrashReporter<T>(reporter);
				return true;
			}
			catch(ArgumentException)
			{
				return false;
			}
		}

		/// <summary>
		/// Removes the crash reporter.
		/// </summary>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static void RemoveCrashReporter<T>() where T : IReporter
		{
			lock(_reporterLock)
			{
				if (!_reporters.Any(r => r.GetType() == typeof(T)))
					throw new ArgumentException($"Reporter of type {typeof(T).Name} has not been added");

				_reporters.Remove(_reporters.FirstOrDefault(r => r.GetType() == typeof(T)));
			}
		}

		/// <summary>
		/// Tries the remove crash reporter.
		/// </summary>
		/// <returns><c>true</c>, if remove crash reporter was tryed, <c>false</c> otherwise.</returns>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static bool TryRemoveCrashReporter<T>() where T : IReporter
		{
			try
			{
				RemoveCrashReporter<T>();
				return true;
			}
			catch(ArgumentException)
			{
				return false;
			}
		}
		#endregion

		#region Logging
		/// <summary>
		/// Gets the log level for reporter.
		/// </summary>
		/// <returns>The log level for reporter.</returns>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static LogLevel GetLogLevelForReporter<T>()
		{
			lock(_reporterLock)
			{
				if(!_reporters.Any(r => r.GetType() == typeof(T)))
					throw new ArgumentException($"Reporter of type {typeof(T).Name} has not been added");
				
				return _reporters.FirstOrDefault(r => r.GetType() == typeof(T)).GetLogLevel();
			}
		}

		/// <summary>
		/// Sets the log level for reporter.
		/// </summary>
		/// <param name="logLevel">Log level.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static void SetLogLevelForReporter<T>(LogLevel logLevel)
		{
			lock(_reporterLock)
			{
				if(!_reporters.Any(r => r.GetType() == typeof(T)))
					throw new ArgumentException($"Reporter of type {typeof(T).Name} has not been added");
				
				_reporters.FirstOrDefault(r => r.GetType() == typeof(T)).SetLogLevel(logLevel);
			}
		}

		/// <summary>
		/// Gets the log level.
		/// </summary>
		/// <returns>The log level.</returns>
		public static LogLevel GetLogLevel() =>
			_logLevel;

		/// <summary>
		/// Sets the log level.
		/// </summary>
		/// <param name="logLevel">Log level.</param>
		public static void SetLogLevel(LogLevel logLevel)
		{
			lock(_reporterLock)
			{
				if(_reporters.Any())
				{
					foreach(var reporter in _reporters)
					{
						try
						{
							reporter.SetLogLevel(logLevel);
						}
						catch(NotImplementedException ex)
						{
							Console.WriteLine($"The {reporter.GetType().Name} does not support SetLogLevel(LogLevel) - {ex.Message}");
						}
					}
				}
				_logLevel = logLevel;
				_logLevelSet = true;
			}
		}

		/// <summary>
		/// Log the specified logLevel, message and tag.
		/// </summary>
		/// <param name="logLevel">Log level.</param>
		/// <param name="message">Message.</param>
		/// <param name="tag">Tag.</param>
		public static void Log(LogLevel logLevel, string message, string tag = null)
        {
			if(!_logLevelSet)
			{
				_pending.Enqueue((logLevel, message, null, tag));
				return;
			}

			Task.Factory.StartNew(() =>
			{
				lock(_reporterLock)
				{
					if(_reporters.Any())
					{
						ProcessPending();

						foreach(var reporter in _reporters)
						{
							try
							{
								reporter.Log(logLevel, message, tag);
							}
							catch(NotImplementedException ex)
							{
								Console.WriteLine($"The {reporter.GetType().Name} does not support Log(LogLevel, string, string) - {ex.Message}");
							}
						}
					}
				}
			});
        }

		/// <summary>
		/// Log the specified logLevel, message, exception and tag.
		/// </summary>
		/// <param name="logLevel">Log level.</param>
		/// <param name="message">Message.</param>
		/// <param name="exception">Exception.</param>
		/// <param name="tag">Tag.</param>
		public static void Log(LogLevel logLevel, string message, Exception exception, string tag = null)
        {
			if(!_logLevelSet)
			{
				_pending.Enqueue((logLevel, message, exception, tag));
				return;
			}

            Task.Factory.StartNew(() =>
			{
				lock(_reporterLock)
				{
					if(_reporters.Any())
					{
						ProcessPending();

						foreach(var reporter in _reporters)
						{
							try
							{
								reporter.Log(logLevel, message, exception, tag);
							}
							catch(NotImplementedException ex)
							{
								Console.WriteLine($"The {reporter.GetType().Name} does not support Log(LogLevel, string, Exception, string) - {ex.Message}");
							}
						}
					}
				}
			});
        }
		#endregion

		static void ProcessPending()
		{
			if(!_pending.IsEmpty)
			{
				if(_reporters.Any()) // we assume we are already in a lock
				{
					foreach(var log in _pending)
					{
						foreach(var reporter in _reporters)
						{
							try
							{
								if(log.ex == null)
								reporter.Log(log.logLevel, log.message, log.tag);
								else
									reporter.Log(log.logLevel, log.message, log.ex, log.tag);
							}
							catch(NotImplementedException ex)
							{
								if(log.ex == null)
									Console.WriteLine($"The {reporter.GetType().Name} does not support Log(LogLevel, string, tag) - {ex.Message}");
								else
									Console.WriteLine($"The {reporter.GetType().Name} does not support Log(LogLevel, string, Exception, string) - {ex.Message}");
							}
						}
					}
				}
			}
		}
	}
}
