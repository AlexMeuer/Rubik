using System;
using Core.Logging;
using Core.State;
using Core.TinyMessenger;
using Game.Cube;

namespace Game.GameState.States
{
    public class SolvedState : CubeGameStateBase
    {
        private readonly TimeSpan timeToSolve;

        public SolvedState(StateContext context, ITinyMessengerHub messengerHub, ILogger logger, IRubiksCube rubiksCube,
            TimeSpan timeToSolve) : base(context, messengerHub, logger, rubiksCube)
        {
            this.timeToSolve = timeToSolve;
        }

        protected override void OnEnter()
        {
            Logger.Error("TIME TO SOLVE: {0} seconds", timeToSolve.TotalSeconds);
        }

        protected override void OnExit()
        {
            
        }
    }
}