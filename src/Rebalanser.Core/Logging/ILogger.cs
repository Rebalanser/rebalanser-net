using System;

namespace Rebalanser.Core.Logging
{
    public enum LogLevel
    {
        DEBUG=0,
        INFO=1,
        WARN=2,
        ERROR=3
    }

    public interface ILogger
    {
        void SetMinimumLevel(LogLevel logLevel);
        void Debug(string clientId, string text);
        void Info(string clientId, string text);
        void Warn(string clientId, string text);
        void Error(string clientId, string text);
        void Error(string clientId, Exception ex);
        void Error(string clientId, string text, Exception ex);
    }
}
