using System;

namespace Rebalanser.Core
{
    public class RebalanserException : Exception
    {
        public RebalanserException(string message)
            : base(message)
        {

        }

        public RebalanserException(string message, Exception ex)
            : base(message, ex)
        {

        }
    }
}
