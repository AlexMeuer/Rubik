using System.Collections;
using Core.Command;
using Core.IoC;
using Core.TinyMessenger;
using Game.Cube;

namespace Game
{
    public partial class EntryPoint : ICommandExecutor
    {
        public int cubesPerRow = 3;
        public RubiksCube cube;
        private ITinyMessengerHub messengerHub;
        private IDragListener dragListener;
        
        private void Initialise()
        {
            IoC.Initialize(
                new Core.ContainerRegistrations(),
                new Domain.ContainerRegistrations(),
                new ContainerRegistrations(commandExecutor: this));

            messengerHub = IoC.Resolve<ITinyMessengerHub>();
            
            dragListener = IoC.Resolve<IDragListener>();
        }

        public void HandleExecution(IEnumerator execution)
        {
            StartCoroutine(execution);
        }
    }
}