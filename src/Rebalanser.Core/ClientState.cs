namespace Rebalanser.Core
{
    /// <summary>
    /// Describes the current state of the client
    /// </summary>
    public enum ClientState
    {
        /// <summary>
        /// No provider was supplied to the client. Check your code.
        /// </summary>
        NoProvider,
        /// <summary>
        /// Created but not started
        /// </summary>
        NotStarted,
        /// <summary>
        /// Started and wait for an assignment of resources
        /// </summary>
        PendingAssignment,
        /// <summary>
        /// The client has started and has been assigned resources
        /// </summary>
        Assigned,
        /// <summary>
        /// The client has been stopped in a controlled way.
        /// </summary>
        Stopped,
        /// <summary>
        /// The client aborted due to an unrecoverable error. It is no longer participating in the resource group.
        /// </summary>
        Aborted
    }
}