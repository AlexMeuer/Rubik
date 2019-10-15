using Core.IoC;
using Core.Logging;
using Core.State;
using Core.TinyMessenger;
using Game.Cube.Factory;
using Game.GameState.States;

namespace Game.GameState
{
    public class GameStateController
    {
        private readonly ITinyMessengerHub messengerHub;
        private readonly ILogger logger;
        private readonly StateContext context;

        public GameStateController(ITinyMessengerHub messengerHub, ILogger logger)
        {
            this.messengerHub = messengerHub;
            this.logger = PrefixedLogger.ForType<GameStateController>(logger);
            
            context = new StateContext(this.logger);
            
            context.TransitionTo(new MainMenuState(context, messengerHub, logger, IoC.Resolve<IRubiksCubeFactory>()));
        }
    }
}