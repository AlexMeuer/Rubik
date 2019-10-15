using Core.Command.Messages;
using Core.Logging;
using Core.State;
using Core.TinyMessenger;
using Game.Command;
using Game.Cube;
using Game.Messages;

namespace Game.GameState.States
{
    public class PlayingState : CubeGameStateBase
    {
        private TinyMessageSubscriptionToken rotateCommandSubscriptionToken;
        private TinyMessageSubscriptionToken dragEndSubscriptionToken;
        
        public PlayingState(StateContext context, ITinyMessengerHub messengerHub, ILogger logger, IRubiksCube cube)
            : base(context, messengerHub, logger, cube)
        {
        }

        protected override void OnEnter()
        {
            rotateCommandSubscriptionToken = MessengerHub.Subscribe<EnqueueCommandMessage>(OnEnqueueCommand);

            dragEndSubscriptionToken = MessengerHub.Subscribe<DragEndMessage>(RubiksCube.AcceptDragInput);
        }

        protected override void OnExit()
        {
            MessengerHub.Unsubscribe<EnqueueCommandMessage>(rotateCommandSubscriptionToken);
            
            MessengerHub.Unsubscribe<EnqueueCommandMessage>(dragEndSubscriptionToken);
        }

        private void OnEnqueueCommand(EnqueueCommandMessage message)
        {
            if (message.Command is RotateSliceCommand)
            {
                Context.TransitionTo(new ExecutingUserMoveState(Context, MessengerHub, Logger, RubiksCube));
            }
        }
    }
}