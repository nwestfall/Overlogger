using System;
using Xunit;

using Overlogger;
using Overlogger.Console;

namespace Overlogger.Test
{
    public class ConsoleReporterTest
    {
        [Fact]
        public void QuickTestReporter()
        {
            var consoleReporter = new ConsoleReporter();
            consoleReporter.SetLogLevel(LogLevel.Verbose);
            consoleReporter.Log(LogLevel.Info, "Test Message");
            consoleReporter.Log(LogLevel.Info, "Test Message", "TAG");
            consoleReporter.Log(LogLevel.Error, "Test Message", new Exception("My Exception", new Exception("My inner exception")), "TAG");
        }

        [Theory]
        [InlineData(LogLevel.Verbose)]
        [InlineData(LogLevel.Debug)]
        [InlineData(LogLevel.Info)]
        [InlineData(LogLevel.Warn)]
        [InlineData(LogLevel.Error)]
        public void SetAndGetLogLevel(LogLevel logLevel)
        {
            var consoleReporter = new ConsoleReporter();
            consoleReporter.SetLogLevel(logLevel);
            Assert.Equal(logLevel, consoleReporter.GetLogLevel());
        }
    }
}
