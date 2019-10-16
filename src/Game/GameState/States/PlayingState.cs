using Core.Command.Messages;
using Core.Logging;
using Core.Messages;
using Core.State;
using Core.TinyMessenger;
using Game.Command;
using Game.Cube;
using Game.Messages;
using Game.UI;

namespace Game.GameState.States
{
    public class PlayingState : CubeGameStateBase
    {
        private readonly IScreen screen;
        
        private TinyMessageSubscriptionToken rotateCommandSubscriptionToken;
        private TinyMessageSubscriptionToken dragEndSubscriptionToken;
        
        public PlayingState(StateContext context, ITinyMessengerHub messengerHub, ILogger logger, IRubiksCube cube, IScreen screen)
            : base(context, messengerHub, logger, cube)
        {
            this.screen = screen;
        }

        protected override void OnEnter()
        {
            rotateCommandSubscriptionToken = MessengerHub.Subscribe<EnqueueCommandMessage>(OnEnqueueCommand);

            dragEndSubscriptionToken = MessengerHub.Subscribe<DragEndMessage>(RubiksCube.AcceptDragInput);
            
            MessengerHub.Publish(new EnableCameraControlMessage(this));
            
            screen.SetEnabled(true);
        }

        protected override void OnExit()
        {
            rotateCommandSubscriptionToken.Dispose();
            
            dragEndSubscriptionToken.Dispose();
            
            screen.SetEnabled(false);
        }

        private void OnEnqueueCommand(EnqueueCommandMessage message)
        {
            if (message.Command is RotateSliceCommand)
            {
                Context.TransitionTo(new ExecutingUserMoveState(Context, MessengerHub, Logger, RubiksCube, screen));
            }
        }
    }
}