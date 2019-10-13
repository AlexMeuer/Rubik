using System.Collections;
using Core.IoC;

namespace Game
{
    public partial class EntryPoint
    {
        public int cubesPerRow = 3;
        
        private void Initialise()
        {
            IoC.Initialize(new ContainerRegistrations());

            new Rubik(cubesPerRow);
        }
    }
}