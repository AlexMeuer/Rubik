using Core.Command;
using Core.IoC;
using Core.TinyMessenger;
using SimpleInjector;

namespace Core
{
    public class ContainerRegistrations : ContainerRegistrationsBase
    {
        public override void RegisterAll(Container container)
        {
            Singleton<ITinyMessengerHub, TinyMessengerHub>(container);
            Singleton<ICommandHandler, CommandHandler>(container);
        }
    }
}