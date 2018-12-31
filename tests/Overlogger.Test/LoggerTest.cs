using System;
using System.Threading.Tasks;
using Xunit;

using Overlogger;
using Overlogger.Console;

namespace Overlogger.Test
{
    public class LoggerTest
    {
        [Fact]
        public void LoggerAddAndRemoveReporter()
        {
            Assert.True(Logger.TryAddCrashReporter(new ConsoleReporter()));
            Assert.Equal(1, Logger.NumberOfReporters);
            Assert.True(Logger.TryRemoveCrashReporter<ConsoleReporter>());
            Assert.Equal(0, Logger.NumberOfReporters);
        }

        [Fact]
        public void LoggerAddSameReporterTwice()
        {
            Assert.True(Logger.TryAddCrashReporter(new ConsoleReporter()));
            Assert.Equal(1, Logger.NumberOfReporters);
            Assert.False(Logger.TryAddCrashReporter(new ConsoleReporter()));
            Assert.Equal(1, Logger.NumberOfReporters);
            Assert.True(Logger.TryRemoveCrashReporter<ConsoleReporter>());
            Assert.Equal(0, Logger.NumberOfReporters);
        }

        [Fact]
        public void LoggerRemoveSameReporterTwice()
        {
            Assert.True(Logger.TryAddCrashReporter(new ConsoleReporter()));
            Assert.Equal(1, Logger.NumberOfReporters);
            Assert.True(Logger.TryRemoveCrashReporter<ConsoleReporter>());
            Assert.Equal(0, Logger.NumberOfReporters);
            Assert.False(Logger.TryRemoveCrashReporter<ConsoleReporter>());
            Assert.Equal(0, Logger.NumberOfReporters);
        }

        [Fact]
        public async Task LoggerTestWithConsole()
        {
            try
            {
                Assert.True(Logger.TryAddCrashReporter(new ConsoleReporter()));
                Assert.Equal(1, Logger.NumberOfReporters);
                Logger.SetLogLevel(LogLevel.Verbose);
                Logger.Log(LogLevel.Info, "Test Message");
                Logger.Log(LogLevel.Info, "Test Message 2", "TAG");
                Logger.Log(LogLevel.Error, "Test Message 3", new Exception("My Exception"), "TAG");
                await Task.Delay(500).ConfigureAwait(false); // Let messages print
            }
            finally
            {
                Assert.True(Logger.TryRemoveCrashReporter<ConsoleReporter>());
                Assert.Equal(0, Logger.NumberOfReporters);
            }
        }

        [Theory]
        [InlineData(LogLevel.Verbose)]
        [InlineData(LogLevel.Debug)]
        [InlineData(LogLevel.Info)]
        [InlineData(LogLevel.Warn)]
        [InlineData(LogLevel.Error)]
        public void GetLogLevelFromType(LogLevel logLevel)
        {
            try
            {
                Logger.TryAddCrashReporter(new ConsoleReporter());
                Logger.SetLogLevel(logLevel);
                Assert.Equal(logLevel, Logger.GetLogLevel());
            }
            finally
            {
                Logger.TryRemoveCrashReporter<ConsoleReporter>();
            }
        }

        [Theory]
        [InlineData(LogLevel.Verbose)]
        [InlineData(LogLevel.Debug)]
        [InlineData(LogLevel.Info)]
        [InlineData(LogLevel.Warn)]
        [InlineData(LogLevel.Error)]
        public void SetAndGetLogLevelForType(LogLevel logLevel)
        {
            try
            {
                Logger.TryAddCrashReporter(new ConsoleReporter());
                Logger.SetLogLevelForReporter<ConsoleReporter>(logLevel);
                Assert.Equal(logLevel, Logger.GetLogLevelForReporter<ConsoleReporter>());
            }
            finally
            {
                Logger.TryRemoveCrashReporter<ConsoleReporter>();
            }
        }
    }
}
