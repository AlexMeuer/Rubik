using Core.Command.Messages;
using Core.Logging;
using Core.State;
using Core.TinyMessenger;
using Game.Command;
using Game.Cube;
using Game.UI;

namespace Game.GameState.States
{
    public class ExecutingUserMoveState : CubeGameStateBase
    {
        private readonly IScreen screen;
        
        private TinyMessageSubscriptionToken commandFinishedSubscriptionToken;
        
        public ExecutingUserMoveState(StateContext context, ITinyMessengerHub messengerHub, ILogger logger, IRubiksCube cube, IScreen screen)
            : base(context, messengerHub, logger, cube)
        {
            this.screen = screen;
        }

        protected override void OnEnter()
        {
            commandFinishedSubscriptionToken = MessengerHub.Subscribe<CommandCompleteMessage>(OnCommandFinished);
        }

        protected override void OnExit()
        {
            commandFinishedSubscriptionToken.Dispose();
        }

        private void OnCommandFinished(CommandCompleteMessage message)
        {
            if (message.Command is RotateSliceCommand)
            {
                Context.TransitionTo(new CheckingSolvedState(Context, MessengerHub, Logger, RubiksCube, screen));
            }
        }
    }
}