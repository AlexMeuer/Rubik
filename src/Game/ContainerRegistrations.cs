using Core.IoC;
using Core.Logging;
using Game.Logging;
using SimpleInjector;

namespace Game
{
    public class ContainerRegistrations : ContainerRegistrationsBase
    {
        public override void RegisterAll(Container container)
        {
            Singleton<ILogger, UnityConsoleLogger>(container);
        }
    }
}