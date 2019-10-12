using SimpleInjector;

namespace Core.IoC
{
    public interface IContainerRegistrations
    {
        void RegisterAll(Container container);
    }
}