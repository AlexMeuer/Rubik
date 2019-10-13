using Core.IoC;
using Core.Logging;
using Game.Cube.Stickers;
using Game.Logging;
using SimpleInjector;

namespace Game
{
    public class ContainerRegistrations : ContainerRegistrationsBase
    {
        public override void RegisterAll(Container container)
        {
            Singleton<ILogger, UnityConsoleLogger>(container);
            Transient<IStickerDataFactory, StickerDataFactory>(container);
            Transient<IStickerFactory, StickerFactory>(container);
        }
    }
}