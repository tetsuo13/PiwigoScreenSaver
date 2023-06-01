using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace PiwigoScreenSaver.Domain;

/// <summary>
/// Simple logger that writes to a file in the user's temporary folder.
/// </summary>
public class Logger : ILogger
{
    public const string EnvName = "PiwigoScreenSaverDebug";

    private readonly string _logFilePath;
    private readonly bool _enableDebugLevel;

    public Logger()
    {
        _enableDebugLevel = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable(EnvName));
        _logFilePath = Path.Combine(Path.GetTempPath(), "PiwigoScreenSaver.log");
    }

    public IDisposable BeginScope<TState>(TState state)
    {
        throw new NotImplementedException();
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        if (logLevel == LogLevel.Debug || logLevel == LogLevel.Trace)
        {
            return _enableDebugLevel;
        }
        return true;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state,
        Exception exception, Func<TState, Exception, string> formatter)
    {
        string message = string.Empty;

        if (formatter != null)
        {
            message += formatter(state, exception);
        }

        File.AppendAllText(_logFilePath,
            $"{DateTime.Now:O} [{logLevel}] - {message}\n");
    }
}
