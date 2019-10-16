using System;
using Core.Logging;
using Core.Store;

namespace Core.State
{
    public class StateContext : IDisposable
    {
        private IState state;

        public ILogger Logger { get; }
        public IStore Store { get; }
        public bool IsDisabled => state == null || !state.IsActive;

        public StateContext(ILogger logger, IStore store)
        {
            Logger = logger;
            Store = store;
        }


        public void TransitionTo(IState nextState)
        {
            Logger.Info("Transitioning to {0}", nextState.GetType().Name);
            
            state?.Exit();

            state = nextState;
            
            state.Enter();
        }

        public void Disable()
        {
            if (IsDisabled)
                return;
            
            state.Exit();
        }

        public void Enable()
        {
            if (!IsDisabled)
                return;
            
            state.Enter();
        }

        public void Dispose()
        {
            state?.Exit();
        }
    }
}