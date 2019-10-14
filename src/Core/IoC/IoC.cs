using SimpleInjector;

namespace Core.IoC
{
    public static class IoC
    {
        private static Container container;
        
        public static void Initialize(params IContainerRegistrations[] registrars)
        {
            container = new Container();

            foreach (var r in registrars)
            {
                r.RegisterAll(container);
            }
            
            container.Verify();
        }
        
        public static T Resolve<T>() where T : class
        {
            return container.GetInstance<T>();
        }

        public static void Dispose()
        {
            container.Dispose();
        }
    }
}