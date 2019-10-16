using Core.IoC;
using Core.Lighting;
using Core.State;
using Core.TinyMessenger;
using Game.Cube.Factory;
using Game.UI;
using UnityEngine;
using ILogger = Core.Logging.ILogger;

namespace Game.GameState.States
{
    public class MainMenuState : GameStateBase
    {
        private readonly IRubiksCubeFactory rubiksCubeFactory;
        private readonly MainMenuScreen screen;
        
        public MainMenuState(StateContext context, ITinyMessengerHub messengerHub, ILogger logger, IRubiksCubeFactory rubiksCubeFactory)
            : base(context, messengerHub, logger)
        {
            this.rubiksCubeFactory = rubiksCubeFactory;
            
            screen = new MainMenuScreen(AdvanceToGame, QuitGame);
        }

        protected override void OnEnter()
        {
            screen.Build();

            screen.AnimateIn();
        }

        protected override void OnExit()
        {
            screen.AnimateOut(screen.Dispose);
        }

        private void AdvanceToGame()
        {
            var rubiksCube = rubiksCubeFactory.Create(screen.SelectedCubeSize);

            var state = new ScramblingState(Context, MessengerHub, Logger, rubiksCube, IoC.Resolve<ILightLevelController>());

            Context.TransitionTo(state);
        }

        private static void QuitGame() => Application.Quit();
    }
}