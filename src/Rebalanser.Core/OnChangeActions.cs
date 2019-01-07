using System;
using System.Collections.Generic;

namespace Rebalanser.Core
{
    /// <summary>
    /// A wrapper class for all start, stop and abort actions.
    /// This is only of note for provider library implementers.
    /// </summary>
    public sealed class OnChangeActions
    {
        public OnChangeActions()
        {
            OnStartActions = new List<Action<IList<string>>>();
            OnStopActions = new List<Action>();
            OnAbortActions = new List<Action<string, Exception>>();
        }

        public IList<Action<IList<string>>> OnStartActions { get; set; }
        public IList<Action> OnStopActions { get; set; }
        public IList<Action<string, Exception>> OnAbortActions { get; set; }

        public void AddOnStartAction(Action<IList<string>> action)
        {
            OnStartActions.Add(action);
        }

        public void AddOnStopAction(Action action)
        {
            OnStopActions.Add(action);
        }

        public void AddOnAbortAction(Action<string, Exception> action)
        {
            OnAbortActions.Add(action);
        }
    }
}
