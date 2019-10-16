using System.Collections;
using Core.Command;
using Core.IoC;
using Core.TinyMessenger;

namespace Game
{
    public partial class EntryPoint : ICommandExecutor
    {
        private ITinyMessengerHub messengerHub;
        
        private void Initialise()
        {
            IoC.Initialize(
                new Core.ContainerRegistrations(),
                new Domain.ContainerRegistrations(),
                new ContainerRegistrations(commandExecutor: this));
            
            messengerHub = IoC.Resolve<ITinyMessengerHub>();
        }

        public void HandleExecution(IEnumerator execution)
        {
            StartCoroutine(execution);
        }
    }
}