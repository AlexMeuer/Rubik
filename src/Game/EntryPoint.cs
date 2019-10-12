using Core.IoC;
using Core.Logging;

namespace Game
{
    public partial class EntryPoint
    {
        private void Initialise()
        {
            IoC.Initialize(new ContainerRegistrations(),
                            new Infrastructure.ContainerRegistrations());
        }

    }
}