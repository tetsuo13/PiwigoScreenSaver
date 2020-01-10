using Microsoft.Extensions.Logging;
using PiwigoScreenSaver.Domain;
using System;
using Xunit;

namespace PiwigoScreenSaver.Tests.Domain
{
    public class LoggerTests
    {
        [Fact]
        public void DefaultLogLevels()
        {
            Environment.SetEnvironmentVariable(Logger.EnvName, null);

            var logger = new Logger();

            Assert.True(logger.IsEnabled(LogLevel.Critical));
            Assert.True(logger.IsEnabled(LogLevel.Error));
            Assert.True(logger.IsEnabled(LogLevel.Warning));
            Assert.True(logger.IsEnabled(LogLevel.Information));
            Assert.False(logger.IsEnabled(LogLevel.Debug));
            Assert.False(logger.IsEnabled(LogLevel.Trace));
        }

        [Fact]
        public void EnvironmentVariable_EnablesDebugLevel()
        {
            Environment.SetEnvironmentVariable(Logger.EnvName, "abc");

            var logger = new Logger();

            Assert.True(logger.IsEnabled(LogLevel.Critical));
            Assert.True(logger.IsEnabled(LogLevel.Error));
            Assert.True(logger.IsEnabled(LogLevel.Warning));
            Assert.True(logger.IsEnabled(LogLevel.Information));
            Assert.True(logger.IsEnabled(LogLevel.Debug));
            Assert.True(logger.IsEnabled(LogLevel.Trace));
        }
    }
}
