using System;

namespace Rebalanser.Core
{
    public class ClientOptions
    {
        public bool AutoRecoveryOnError { get; set; }
        public TimeSpan RestartDelay { get; set; }
    }
}
