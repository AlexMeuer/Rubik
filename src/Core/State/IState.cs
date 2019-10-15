using Core.Logging;
using Core.TinyMessenger;

namespace Core.State
{
    public interface IState
    {
        bool IsActive { get; }
        void Enter();
        void Exit();
    }

    public abstract class StateBase : IState
    {
        protected readonly StateContext Context;
        protected readonly ITinyMessengerHub MessengerHub;
        protected readonly ILogger Logger;
        
        public bool IsActive { get; private set; }

        protected StateBase(StateContext context, ITinyMessengerHub messengerHub, ILogger logger)
        {
            Context = context;
            MessengerHub = messengerHub;
            Logger = logger;
        }
        public void Enter()
        {
            IsActive = true;
            
            OnEnter();
        }

        public void Exit()
        {
            IsActive = false;
            
            OnExit();
        }
        
        protected abstract void OnEnter();
        protected abstract void OnExit();
    }
}