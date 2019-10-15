using Core.Logging;
using Core.State;
using Core.TinyMessenger;
using Game.Cube;

namespace Game.GameState.States
{
    public abstract class CubeGameStateBase : GameStateBase
    {
        protected readonly IRubiksCube RubiksCube;

        protected CubeGameStateBase(StateContext context, ITinyMessengerHub messengerHub, ILogger logger, IRubiksCube rubiksCube)
            : base(context, messengerHub, logger)
        {
            RubiksCube = rubiksCube;
        }
    }
}