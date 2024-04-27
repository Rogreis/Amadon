using AmadonStandardLib.Classes;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace AmadonStandardLib.Helpers
{

    public class FileLogger : ILogger
    {
        private readonly string _filePath;
        private readonly LogLevel _minLogLevel;

        public FileLogger(string filePath, LogLevel minLogLevel)
        {
            _filePath = filePath;
            _minLogLevel = minLogLevel;
        }

        public IDisposable? BeginScope<TState>(TState state) => default;

        public bool IsEnabled(LogLevel logLevel) => logLevel >= _minLogLevel;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
                return;

            var logMessage = formatter(state, exception);
            File.AppendAllText(_filePath, $"{DateTime.Now} [{logLevel}] {logMessage}\n");
        }
    }

    // FileLoggerProvider.cs

    public class FileLoggerProvider : ILoggerProvider
    {
        private readonly string _filePath;
        private readonly LogLevel _minLogLevel;

        public FileLoggerProvider(string filePath, LogLevel minLogLevel)
        {
            _filePath = filePath;
            _minLogLevel = minLogLevel;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(_filePath, _minLogLevel);
        }

        public void Dispose() { }
    }

    public class Logger
    {
        private bool _logIniciado = false;

        public const string FileName = "Amadon.log";

        public static string? PathLog { get; set; }

        private readonly ILogger _logger;

        public Logger(ILogger logger)
        {
            _logger = logger;
        }



        private void FatalError(string message)
        {
            // https://learn.microsoft.com/en-us/dotnet/maui/user-interface/pop-ups?view=net-maui-7.0
            _logger.LogError(message);
            LibraryEventsControl.FireFatalError(message);
        }


        public void NonFatalError(string message)
        {
            _logger.LogError(message);
            LibraryEventsControl.FireError(message);
        }

        public void Warn(string message)
        {
            _logger.LogWarning(message);
        }

        public void Info(string message)
        {
            _logger.LogInformation(message);
        }

        public void Error(string message)
        {
            _logger.LogError(message);
        }

        public void Error(string message, Exception ex)
        {
            _logger.LogError(ex, message);
        }


        /// <summary>
        /// Returns all current lines on log
        /// </summary>
        public string GetLog()
        {
            Close();
            return File.ReadAllText(PathLog);
        }


        public void Close()
        {
            //LogManager.GetRepository().Shutdown();
            _logIniciado = false;
        }
    }
}
