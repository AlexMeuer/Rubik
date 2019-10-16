using System;
using Core.IoC;
using Core.Messages;
using Core.State;
using Core.TinyMessenger;
using Game.Cube;
using Game.Cube.Factory;
using Game.UI;
using ILogger = Core.Logging.ILogger;

namespace Game.GameState.States
{
    public class SolvedState : CubeGameStateBase, IEndGameScreenCallbacks
    {
        private IScreen screen;

        public SolvedState(StateContext context, ITinyMessengerHub messengerHub, ILogger logger, IRubiksCube rubiksCube,
            TimeSpan timeToSolve) : base(context, messengerHub, logger, rubiksCube)
        {
            screen = new EndGameScreen(timeToSolve, this);
        }

        protected override void OnEnter()
        {
            MessengerHub.Publish(new CameraOrbitRequestMessage(this,
                                                                times: 3,
                                                                onComplete: () =>
                                                                {
                                                                    screen.Build();
                                                                    screen.AnimateIn();
                                                                }));
        }

        protected override void OnExit()
        {
            screen.AnimateOut(screen.Dispose);
            
            RubiksCube.AnimateOut(RubiksCube.Dispose);
            
            MessengerHub.Publish(new DisableCameraControlMessage(this));
            MessengerHub.Publish(new ResetCameraMessage(this));
        }

        public void OnReturnRequested()
        {
            Context.TransitionTo(new MainMenuState(Context, MessengerHub, Logger, IoC.Resolve<IRubiksCubeFactory>()));
        }

        public void OnStayRequested()
        {
            screen.AnimateOut(() =>
            {
                screen.Dispose();
                
                MessengerHub.Publish(new EnableCameraControlMessage(this));
                
                screen = new FreeViewScreen(this);
                
                screen.Build();
                
                screen.AnimateIn();
            });
        }
    }
}