using Core.Logging;
using Core.State;
using Core.TinyMessenger;

namespace Game.GameState.States
{
    public class SolvedState : GameStateBase
    {
        public SolvedState(StateContext context, ITinyMessengerHub messengerHub, ILogger logger) : base(context, messengerHub, logger)
        {
        }

        protected override void OnEnter()
        {
            throw new System.NotImplementedException();
        }

        protected override void OnExit()
        {
            throw new System.NotImplementedException();
        }
    }
}