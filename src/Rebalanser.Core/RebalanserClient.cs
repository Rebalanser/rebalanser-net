using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Rebalanser.Core
{
    /// <summary>
    /// Creates a Rebalanser client node that participates in a resource group
    /// </summary>
    public class RebalanserClient : IDisposable
    {
        private IRebalanserProvider rebalanserProvider;
        private CancellationTokenSource cts;

        public RebalanserClient()
        {
            this.rebalanserProvider = Providers.GetProvider();
        }

        /// <summary>
        /// Called when a rebalancing is triggered
        /// </summary>
        public event EventHandler OnUnassignment;

        /// <summary>
        /// Called once the node has been assigned new resources
        /// </summary>
        public event EventHandler<OnAssignmentArgs> OnAssignment;

        /// <summary>
        /// Called when a non recoverable error occurs and the client is no longer participating in the resource group.
        /// </summary>
        public event EventHandler<OnAbortedArgs> OnAborted;

        /// <summary>
        /// Starts the node
        /// </summary>
        /// <param name="resourceGroup">The id of the resource group</param>
        /// <returns></returns>
        public async Task StartAsync(string resourceGroup, ClientOptions clientOptions)
        {
            this.cts = new CancellationTokenSource();
            var onChangeActions = new OnChangeActions();
            onChangeActions.AddOnStartAction(StartActivity);
            onChangeActions.AddOnStopAction(StopActivity);
            onChangeActions.AddOnAbortAction(Abort);
            await this.rebalanserProvider.StartAsync(resourceGroup, onChangeActions, this.cts.Token, clientOptions);
        }

        /// <summary>
        /// Blocks until the cancellation token is cancelled or the client stops or aborts.
        /// </summary>
        /// <param name="token">A CancellationToken that once cancelled will cause the client to stop participating in the resource group and terminate.</param>
        /// <param name="maxStopTime">If the cancellation token is cancelled, the maximum time to allow the client to safely shutdown.</param>
        /// <returns></returns>
        public async Task BlockAsync(CancellationToken token, TimeSpan maxStopTime)
        {
            while (!token.IsCancellationRequested)
            {
                var clientState = GetCurrentState();
                switch (clientState)
                {
                    case ClientState.PendingAssignment:
                    case ClientState.Assigned:
                        await Task.Delay(100);
                        break;
                    default:
                        return;
                }
            }
            
            await StopAsync(maxStopTime);
        }
        
        /// <summary>
        /// Returns the current state of the client and any assigned resources. If the client
        /// is pending assignment or not in an active state then the resources collection will be empty.
        /// </summary>
        /// <returns>The assigned resources and state of the client</returns>
        public AssignedResources GetAssignedResources()
        {
            return this.rebalanserProvider.GetAssignedResources();
        }

        /// <summary>
        /// Get the current state of the client
        /// </summary>
        /// <returns>The state of the client</returns>
        public ClientState GetCurrentState()
        {
            if (this.rebalanserProvider == null)
                return ClientState.NoProvider;
            
            return this.rebalanserProvider.GetState();
        }
        
        /// <summary>
        /// Shutsdown the client context, including invoking the OnCancelAssignment event handlers
        /// It will block until all handlers have finished executing
        /// </summary>
        /// <returns></returns>
        public async Task StopAsync()
        {
            if (!disposed)
            {
                this.cts.Cancel(); // signals provider to stop
                await this.rebalanserProvider.WaitForCompletionAsync();
                disposed = true;
            }
        }

        /// <summary>
        /// Shutsdown the client context, including invoking the OnCancelAssignment event handlers
        /// It will block until all handlers have finished executing or the timeout has been reached
        /// </summary>
        /// <returns></returns>
        public async Task StopAsync(TimeSpan timeout)
        {
            if (!disposed)
            {
                this.cts.Cancel(); // signals provider to stop
                var completionTask = this.rebalanserProvider.WaitForCompletionAsync();
                if (await Task.WhenAny(completionTask, Task.Delay(timeout)) == completionTask)
                    await completionTask;
                
                disposed = true;
            }
        }
        
        /// <summary>
        /// Shutsdown the client context, including invoking the OnCancelAssignment event handlers
        /// It will block until all handlers have finished executing, or the timeout has been reached or the cancellation token has been cancelled
        /// </summary>
        /// <returns></returns>
        public async Task StopAsync(TimeSpan timeout, CancellationToken token)
        {
            if (!disposed)
            {
                this.cts.Cancel(); // signals provider to stop
                var completionTask = this.rebalanserProvider.WaitForCompletionAsync();
                if (await Task.WhenAny(completionTask, Task.Delay(timeout, token)) == completionTask)
                    await completionTask;
               
                disposed = true;
            }
        }
        

        private bool disposed;
        /// <summary>
        /// If StopAsync has not previously been called it initiates a shutdown of the node,
        /// but leaves only 5 seconds for shutdown, which includes invoking your OnCancelAssignment event handlers
        /// For more control use the StopAsync method where you can specify a longer and safer shutdown timeout
        /// </summary>
        public void Dispose()
        {
            if (!disposed)
            {
                this.cts.Cancel(); // signals provider to stop
                var completionTask = Task.Run(async () => await this.rebalanserProvider.WaitForCompletionAsync());
                completionTask.Wait(5000); // waits for completion up to 5 seconds
                disposed = true;
            }
        }

        private void StopActivity()
        {
            RaiseOnUnassignment(EventArgs.Empty);
        }

        protected virtual void RaiseOnUnassignment(EventArgs e)
        {
            OnUnassignment?.Invoke(this, e);
        }

        private void Abort(string message, Exception ex)
        {
            RaiseOnAbort(new OnAbortedArgs(message, ex));
        }

        protected virtual void RaiseOnAbort(OnAbortedArgs e)
        {
            OnAborted?.Invoke(this, e);
        }

        private void StartActivity(IList<string> resources)
        {
            RaiseOnAssignments(new OnAssignmentArgs(resources));
        }

        protected virtual void RaiseOnAssignments(OnAssignmentArgs e)
        {
            OnAssignment?.Invoke(this, e);
        }
    }
}
