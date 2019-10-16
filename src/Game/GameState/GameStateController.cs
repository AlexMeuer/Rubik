using System;
using Core.IoC;
using Core.Logging;
using Core.State;
using Core.Store;
using Core.TinyMessenger;
using Game.Cube.Factory;
using Game.GameState.States;

namespace Game.GameState
{
    public class GameStateController : IDisposable
    {
        private readonly StateContext context;

        public GameStateController(ITinyMessengerHub messengerHub, ILogger logger, IStore store)
        {
            var prefixedLogger = PrefixedLogger.ForType<GameStateController>(logger);
            
            context = new StateContext(prefixedLogger, store);
            
            context.TransitionTo(new MainMenuState(context, messengerHub, logger, IoC.Resolve<IRubiksCubeFactory>()));
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}