using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace PiwigoScreenSaver.Domain
{
    /// <summary>
    /// Simple logger that writes to a file in the user's temporary folder.
    /// </summary>
    public class Logger : ILogger
    {
        private readonly string logFilePath;

        public Logger()
        {
            logFilePath = Path.Combine(Path.GetTempPath(), "PiwigoScreenSaver.log");
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
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

            File.AppendAllText(logFilePath,
                $"{DateTime.Now.ToString("O")} [{logLevel}] - {eventId.Id} - {message}\n");
        }
    }
}
