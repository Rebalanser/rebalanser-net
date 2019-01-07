using System;
using System.Collections.Generic;

namespace Rebalanser.Core
{
    /// <summary>
    /// EventArgs that contain the list of resources assigned to the client. An empty list is a valid value.
    /// </summary>
    public sealed class OnAssignmentArgs : EventArgs
    {
        public OnAssignmentArgs(IList<string> resources) : base()
        {
            Resources = resources;
        }

        /// <summary>
        /// The list of resources assigned
        /// </summary>
        public IList<string> Resources { get; }
    }
}