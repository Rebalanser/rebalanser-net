using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Rebalanser.Core
{
    public interface IRebalanserProvider
    {
        Task StartAsync(string group, OnChangeActions onChangeActions, CancellationToken token, ClientOptions clientOptions);
        Task WaitForCompletionAsync();
        AssignedResources GetAssignedResources();
        ClientState GetState();
    }
}
