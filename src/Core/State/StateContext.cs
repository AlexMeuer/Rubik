using System;
using Core.Logging;

namespace Core.State
{
    public class StateContext : IDisposable
    {
        private readonly ILogger logger;
        private IState state;

        public bool IsDisabled => state == null || !state.IsActive;

        public StateContext(ILogger logger)
        {
            this.logger = logger;
        }


        public void TransitionTo(IState nextState)
        {
            logger.Info("Transitioning to {0}", nextState.GetType().Name);
            
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