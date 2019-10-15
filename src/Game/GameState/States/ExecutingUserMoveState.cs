using Core.Command.Messages;
using Core.Logging;
using Core.State;
using Core.TinyMessenger;
using Game.Command;
using Game.Cube;

namespace Game.GameState.States
{
    public class ExecutingUserMoveState : CubeGameStateBase
    {
        private TinyMessageSubscriptionToken commandFinishedSubscriptionToken;
        
        public ExecutingUserMoveState(StateContext context, ITinyMessengerHub messengerHub, ILogger logger, IRubiksCube cube)
            : base(context, messengerHub, logger, cube)
        {
        }

        protected override void OnEnter()
        {
            commandFinishedSubscriptionToken = MessengerHub.Subscribe<CommandCompleteMessage>(OnCommandFinished);
        }

        protected override void OnExit()
        {
            MessengerHub.Unsubscribe<CommandCompleteMessage>(commandFinishedSubscriptionToken);
        }

        private void OnCommandFinished(CommandCompleteMessage message)
        {
            if (message.Command is RotateSliceCommand)
            {
                Context.TransitionTo(new CheckingSolvedState(Context, MessengerHub, Logger, RubiksCube));
            }
        }
    }
}