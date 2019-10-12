using SimpleInjector;

namespace Core.IoC
{
    public abstract class ContainerRegistrationsBase : IContainerRegistrations
    {
        public abstract void RegisterAll(Container container);
        
        protected void Singleton<TService, TImpl>(Container container) where TService : class where TImpl : class, TService
        {
            container.Register<TService, TImpl>(Lifestyle.Singleton);
        }
        
        protected void Scoped<TService, TImpl>(Container container) where TService : class where TImpl : class, TService
        {
            container.Register<TService, TImpl>(Lifestyle.Scoped);
        }
        
        protected void Transient<TService, TImpl>(Container container) where TService : class where TImpl : class, TService
        {
            container.Register<TService, TImpl>(Lifestyle.Transient);
        }
    }
}