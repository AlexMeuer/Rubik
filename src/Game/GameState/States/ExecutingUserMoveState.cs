using Core.Command.Messages;
using Core.Logging;
using Core.State;
using Core.Timer;
using Core.TinyMessenger;
using Game.Command;
using Game.Cube;
using Game.UI;

namespace Game.GameState.States
{
    public class ExecutingUserMoveState : CubeGameStateBase
    {
        private readonly IScreen screen;
        private readonly ITimer timer;

        private TinyMessageSubscriptionToken commandFinishedSubscriptionToken;

        public ExecutingUserMoveState(StateContext context, ITinyMessengerHub messengerHub, ILogger logger,
            IRubiksCube cube, IScreen screen, ITimer timer)
            : base(context, messengerHub, logger, cube)
        {
            this.screen = screen;
            this.timer = timer;
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
                Context.TransitionTo(new CheckingSolvedState(Context, MessengerHub, Logger, RubiksCube, screen, timer));
            }
        }
    }
}