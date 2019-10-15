using Core.Logging;
using Core.State;
using Core.TinyMessenger;

namespace Game.GameState.States
{
    public class PausedState : GameStateBase
    {
        public PausedState(StateContext context, ITinyMessengerHub messengerHub, ILogger logger) : base(context, messengerHub, logger)
        {
        }

        protected override void OnEnter()
        {
            
        }

        protected override void OnExit()
        {
            
        }
    }
}