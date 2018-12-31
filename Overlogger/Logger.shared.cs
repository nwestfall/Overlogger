using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;

namespace Overlogger
{
	public static class Logger
	{
		static ConcurrentQueue<(LogLevel logLevel, string message, Exception ex, string tag)> _pending = new ConcurrentQueue<(LogLevel logLevel, string message, Exception ex, string tag)>();

		static IList<IReporter> _reporters = new List<IReporter>();

		static readonly object _reporterLock = new object();

		public static bool IsDebug { get; set; }

		public static int NumberOfReporters => _reporters?.Count ?? 0;

		#region Manage Crash Reporters
		public static void AddCrashReporter<T>(T reporter) where T : IReporter
		{
			lock(_reporterLock)
			{
				if (_reporters.Any(r => r.GetType() == reporter.GetType()))
					throw new ArgumentException($"Reporter of type {reporter.GetType().Name} is already added", nameof(reporter));

				_reporters.Add(reporter);
			}
		}

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

		public static void RemoveCrashReporter<T>() where T : IReporter
		{
			lock(_reporterLock)
			{
				if (!_reporters.Any(r => r.GetType() == typeof(T)))
					throw new ArgumentException($"Reporter of type {typeof(T).Name} has not been added");

				_reporters.Remove(_reporters.FirstOrDefault(r => r.GetType() == typeof(T)));
			}
		}

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

		static void ProcessPending()
		{
			if(!_pending.IsEmpty && NumberOfReporters > 0)
			{
				// TODO: Send to reporter
			}
		}
	}
}
