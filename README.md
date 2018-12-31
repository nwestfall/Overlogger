# Overlogger

[![Build Status](https://dev.azure.com/nathanwestfall/Overlogger/_apis/build/status/Overlogger-CI?branchName=master)](https://dev.azure.com/nathanwestfall/Overlogger/_build/latest?definitionId=16?branchName=master)
[![CodeFactor](https://www.codefactor.io/repository/github/nwestfall/overlogger/badge)](https://www.codefactor.io/repository/github/nwestfall/overlogger)

Logging is something that every application needs, but it shouldn't be a burden to change analytic/crash providers.  Write your app once with overlogger and then switch out providers on the fly.

## Why did I make this?
Being an Xamarin developer and going from Xamarin Insights, to HockeyApp, and then to App Center I got tired having to rewrite my logging logic each time I was forced to change.  This library lets me write it once and then log to one or more than one provider at a time!

## Supported Platforms
 - NETStandard 2.0
 - iOS (10+)
 - Android (4.4+)

## Available Nuget Packages
 - Overlogger [![NuGet](https://img.shields.io/nuget/v/Overlogger.svg?label=NuGet)](https://www.nuget.org/packages/Overlogger/)
 - Overlogger.AppCenter [![NuGet](https://img.shields.io/nuget/v/Overlogger.AppCenter.svg?label=NuGet)](https://www.nuget.org/packages/Overlogger.AppCenter/)
 - Overlogger.BugSnag [![NuGet](https://img.shields.io/nuget/v/Overlogger.BugSnag.svg?label=NuGet)](https://www.nuget.org/packages/Overlogger.BugSnag/)
 - Overlogger.Console [![NuGet](https://img.shields.io/nuget/v/Overlogger.Console.svg?label=NuGet)](https://www.nuget.org/packages/Overlogger.Console/)


## How does it work

Simple.  First, pick which provider you want to use.  We currently support
 - [App Center](https://appcenter.ms)
 - [Bugsnag](https://www.bugsnag.com)
 - Console
 - More to come (create an issue!)
  
If you don't want to wait for me to create one, just implement `IReporter` on your own class.

#### Add a Reporter
```c#
var appCenterReporter = new AppCenterReporter("MY-KEY");
Logger.AddCrashReporter(appCenterReporter);
```
That's it.  If you want to add another, just do the it twice!
```c#
var appCenterReporter = new AppCenterReporter("MY-KEY");
var bugSnagReporter = new BugSnagReporter("MY-KEY");
Logger.AddCrashReporter(appCenterReporter);
Logger.AddCrashReporter(bugSnagReporter);
```
#### Start Logging

Once a reporter is added, start logging away!  Or if you start logging before a reporter is added, we'll keep track of that and then send the logs once a reporter is added.
```c#
Logger.Log(LogLevel.Info, "My Message");
Logger.Log(LogLevel.Info, "My Message", "MY-TAG");
Logger.Log(LogLevel.Error, "My Error", new Exception("My Exception"));
Logger.Log(LogLevel.Error, "My Error", new Exception("My Exception"), "MY-TAG");
```
You can even track events if the reporter supports it (most do)
```c#
Logger.TrackEvent("My Custom Event");
Logger.TrackEvent("My Custom Event", new Dictionary<string, string>() { "My Property", "My Value" });
```

#### Change the minimum log level

If you don't want logs below a certain level to be reported, we'll take care of that for you too!  Just pass the log level to either all reporters or just a specific one.

```c#
Logger.SetLogLevel(LogLevel.Warn); // For all reporters
Logger.SetLogLevelForReporter<AppCenterReporter>(LogLevel.Error); // Just the App Center Reporter
```

#### Remove a Reporter

If you want to remove a reporter, just pass the `Type`
```c#
Logger.RemoveCrashReporter<AppCenterReporter>();
```
