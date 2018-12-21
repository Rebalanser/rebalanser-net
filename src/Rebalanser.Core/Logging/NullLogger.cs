using System;

namespace Rebalanser.Core.Logging
{
    public class NullLogger : ILogger
    {
        public void Debug(string clientId, string text)
        {
            
        }

        public void Warn(string clientId, string text)
        {
            
        }

        public void Error(string clientId, string text)
        {
            
        }

        public void Error(string clientId, Exception ex)
        {
            
        }

        public void Error(string clientId, string text, Exception ex)
        {
            
        }

        public void Info(string clientId, string text)
        {
        }

        public void SetMinimumLevel(LogLevel logLevel)
        {
        }
    }
}
