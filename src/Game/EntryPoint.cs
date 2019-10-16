using System.Collections;
using Core.Command;
using Core.IoC;

namespace Game
{
    public partial class EntryPoint : ICommandExecutor
    {
        private void Initialise()
        {
            IoC.Initialize(
                new Core.ContainerRegistrations(),
                new Domain.ContainerRegistrations(),
                new ContainerRegistrations(commandExecutor: this));
        }

        public void HandleExecution(IEnumerator execution)
        {
            StartCoroutine(execution);
        }
    }
}