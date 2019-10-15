using System;
using Core.Logging;

namespace Core.State
{
    public class StateContext : IDisposable
    {
        private readonly ILogger logger;
        private IState state;

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
            if (state == null || state.IsActive)
                return;
            
            state.Exit();
        }

        public void Enable()
        {
            if (state == null || !state.IsActive)
                return;
            
            state.Enter();
        }

        public void Dispose()
        {
            state?.Exit();
        }
    }
}