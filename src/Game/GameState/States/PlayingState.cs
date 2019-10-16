using Core.Command.Messages;
using Core.Logging;
using Core.Messages;
using Core.State;
using Core.Timer;
using Core.TinyMessenger;
using Game.Command;
using Game.Cube;
using Game.Messages;
using Game.UI;

namespace Game.GameState.States
{
    public partial class PlayingState : CubeGameStateBase
    {
        private readonly ICubeSolvedChecker cubeSolvedChecker;
        private readonly InGameScreen inGameScreen;
        private readonly ITimer timer;
        
        private TinyMessageSubscriptionToken dragEndSubscriptionToken;
        private TinyMessageSubscriptionToken cmdCompleteSubscriptionToken;
        private TinyMessageSubscriptionToken optionsRequestedSubscriptionToken;
        
        public PlayingState(StateContext context, ITinyMessengerHub messengerHub, ILogger logger, IRubiksCube cube, ITimer timer, ICubeSolvedChecker cubeSolvedChecker)
            : base(context, messengerHub, logger, cube)
        {
            this.timer = timer;
            this.cubeSolvedChecker = cubeSolvedChecker;
            
            inGameScreen = new InGameScreen(messengerHub, timer);
            
            inGameScreen.Build();
        }

        protected override void OnEnter()
        {
            cmdCompleteSubscriptionToken = MessengerHub.Subscribe<CommandCompleteMessage>(OnCommandCompleted);

            optionsRequestedSubscriptionToken = MessengerHub.Subscribe<OptionsRequestedMessage>(OnOptionsRequested);
            
            MessengerHub.Publish(new EnableCameraControlMessage(this));
            
            EnableInput();
            
            timer.Start();
            
            inGameScreen.AnimateIn();
        }

        protected override void OnExit()
        {
            cmdCompleteSubscriptionToken.Dispose();
            optionsRequestedSubscriptionToken.Dispose();
            
            MessengerHub.Publish(new DisableCameraControlMessage(this));
            MessengerHub.Publish(new ResetCameraMessage(this));
            
            timer.Stop();
            
            DisableInput();
                
            inGameScreen.AnimateOut(inGameScreen.Dispose);
        }

        private void OnDragEnd(DragEndMessage message)
        {
            if (!RubiksCube.AcceptDragInput(message, out var command))
                return;
            
            DisableInput();
                
            MessengerHub.Publish(new EnqueueCommandMessage(this, command));
        }

        private void OnCommandCompleted(CommandCompleteMessage message)
        {
            if (!(message.Command is RotateSliceCommand))
                return;

            if (cubeSolvedChecker.IsSolved(RubiksCube))
            {
                Context.TransitionTo(new SolvedState(Context, MessengerHub, Logger, RubiksCube, timer.Elapsed));
            }
            else
            {
                EnableInput();
            }
        }

        private void OnOptionsRequested(OptionsRequestedMessage message)
        {
            DisableInput();
            
            MessengerHub.Publish(new DisableCameraControlMessage(this));
            
            timer.Stop();
            
            optionsScreen = new OptionsScreen(this, inGameScreen.TimerIsVisible);
            
            optionsScreen.Build();
            
            optionsScreen.AnimateIn();
        }

        private void EnableInput()
        {
            if (dragEndSubscriptionToken != null)
                return;
            
            dragEndSubscriptionToken = MessengerHub.Subscribe<DragEndMessage>(OnDragEnd);
            
            inGameScreen.SetEnabled(true);
        }

        private void DisableInput()
        {
            if (dragEndSubscriptionToken == null)
                return;
            
            dragEndSubscriptionToken.Dispose();
            dragEndSubscriptionToken = null;
            
            inGameScreen.SetEnabled(true);
        }

        private void CloseOptionsScreen()
        {
            optionsScreen.AnimateOut(() =>
            {
                MessengerHub.Publish(new EnableCameraControlMessage(this));
                
                EnableInput();
                
                timer.Start();
                
                optionsScreen.Dispose();
            });
        }
//        protected override void OnEnter()
//        {
//            if (IsSolved)
//            {
//                screen.AnimateOut(screen.Dispose);
//                
//                timer.Stop();
//                
//                MessengerHub.Publish(new CubeSolvedMessage(this, timer.Elapsed));
//            }
//            else
//            {
//                Context.TransitionTo(new PlayingState(Context, MessengerHub, Logger, RubiksCube, screen, timer));
//            }
//        }
//
//        protected override void OnExit()
//        {
//        }
    }
}