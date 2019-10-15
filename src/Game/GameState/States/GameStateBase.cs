using Core.Logging;
using Core.State;
using Core.TinyMessenger;

namespace Game.GameState.States
{
    public abstract class GameStateBase : StateBase
    {
        protected GameStateBase(StateContext context, ITinyMessengerHub messengerHub, ILogger logger)
            : base(context, messengerHub, logger)
        {
        }
    }
}