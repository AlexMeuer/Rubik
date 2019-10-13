using Core.IoC;

namespace Domain
{
    public class ContainerRegistrations : ContainerRegistrationsBase
    {
        public override void RegisterAll(SimpleInjector.Container container)
        {
            Transient<IColorMap, ColorMap>(container);
        }
    }
}