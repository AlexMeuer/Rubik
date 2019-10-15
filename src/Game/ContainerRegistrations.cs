using Core.Command;
using Core.IoC;
using Game.Camera;
using Game.Cube.Factory;
using Game.GameState;
using Game.Logging;
using SimpleInjector;
using ILogger = Core.Logging.ILogger;

namespace Game
{
    public class ContainerRegistrations : ContainerRegistrationsBase
    {
        private readonly ICommandExecutor commandExecutor;

        public ContainerRegistrations(ICommandExecutor commandExecutor)
        {
            this.commandExecutor = commandExecutor;
        }

        public override void RegisterAll(Container container)
        {
            Singleton<ILogger, UnityConsoleLogger>(container);
            Singleton<CameraController, CameraController>(container);
            Singleton<IDragListener, MouseDragListener>(container);
            Singleton<GameStateController, GameStateController>(container);
            
            Transient<IStickerDataFactory, StickerDataFactory>(container);
            Transient<IStickerFactory, StickerFactory>(container);
            Transient<IPieceFactory, PieceFactory>(container);
            Transient<IRubiksCubeFactory, RubiksCubeFactory>(container);
            
            container.RegisterInstance(commandExecutor);
            
            container.RegisterInstance(UnityEngine.Camera.main);
        }
    }
}