using System.Collections.Generic;

namespace Rebalanser.Core
{
    public class AssignedResources
    {
        public ClientState ClientState { get; set; }
        public IList<string> Resources { get; set; }
    }
}