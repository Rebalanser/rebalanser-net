using System;
using System.Collections.Generic;

namespace Rebalanser.Core
{
    public class OnAssignmentArgs : EventArgs
    {
        public OnAssignmentArgs(IList<string> resources) : base()
        {
            Resources = resources;
        }

        public IList<string> Resources { get; }
    }
}