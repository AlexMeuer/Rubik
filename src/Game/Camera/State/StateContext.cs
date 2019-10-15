using System;
using Core.Logging;

namespace Game.Camera.State
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
            logger.Info("Transitioning to: {0}", nextState.GetType().Name);
            
            state?.Exit();

            state = nextState;
            
            state.Enter();
        }

        public void Dispose()
        {
            state?.Exit();
        }
    }
}