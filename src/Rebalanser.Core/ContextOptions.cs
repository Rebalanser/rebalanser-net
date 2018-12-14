using System;

namespace Rebalanser.Core
{
    public class ContextOptions
    {
        public bool AutoRecoveryOnError { get; set; }
        public TimeSpan RestartDelay { get; set; }
    }
}
