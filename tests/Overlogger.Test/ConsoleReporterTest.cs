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
            consoleReporter.Log(LogLevel.Error, "Test Message", new Exception("My Expcetion"), "TAG");
        }
    }
}
