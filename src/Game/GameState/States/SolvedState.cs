using Core.Logging;
using Core.State;
using Core.TinyMessenger;
using Game.Cube;

namespace Game.GameState.States
{
    public class SolvedState : CubeGameStateBase
    {
        public SolvedState(StateContext context, ITinyMessengerHub messengerHub, ILogger logger, IRubiksCube rubiksCube) : base(context, messengerHub, logger, rubiksCube)
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