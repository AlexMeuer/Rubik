using System.Collections;
using Core.Command;
using Core.Command.Messages;
using Core.State;
using Core.TinyMessenger;
using Game.Cube.Factory;
using UnityEngine;
using ILogger = Core.Logging.ILogger;

namespace Game.GameState.States
{
    public class MainMenuState : GameStateBase, IInvokableCommand
    {
        private readonly IRubiksCubeFactory rubiksCubeFactory;
        public MainMenuState(StateContext context, ITinyMessengerHub messengerHub, ILogger logger, IRubiksCubeFactory rubiksCubeFactory)
            : base(context, messengerHub, logger)
        {
            this.rubiksCubeFactory = rubiksCubeFactory;
        }

        protected override void OnEnter()
        {
            MessengerHub.Publish(new EnqueueCommandMessage(this, this, transient: true));
        }

        protected override void OnExit()
        {
            
        }
        
        public IEnumerator Execute()
        {
            yield return new WaitForSeconds(3f);
            
            Context.TransitionTo(new ScramblingState(Context, MessengerHub, Logger, rubiksCubeFactory.Create(3)));
        }

        public IEnumerator Undo()
        {
            yield return 1;
        }
    }
}