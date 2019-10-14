using System;
using System.Collections;
using Core.Command;
using Core.IoC;
using Core.TinyMessenger;
using Game.Cube;
using Game.Cube.Stickers;
using Game.Messages;
using UnityEngine;
using ILogger = Core.Logging.ILogger;

namespace Game
{
    public partial class EntryPoint : ICommandExecutor
    {
        public int cubesPerRow = 3;
        public RubiksCube cube;
        private ITinyMessengerHub messengerHub;
        private IDragListener dragListener;
        private TinyMessageSubscriptionToken debugStartCoroutineSubscriptionToken;
        
        private void Initialise()
        {
            IoC.Initialize(
                new Core.ContainerRegistrations(),
                new Domain.ContainerRegistrations(),
                new ContainerRegistrations(commandExecutor: this));

            messengerHub = IoC.Resolve<ITinyMessengerHub>();
            
            dragListener = IoC.Resolve<IDragListener>();

            cube = new RubiksCube(cubesPerRow,IoC.Resolve<ILogger>(),  messengerHub, IoC.Resolve<IStickerDataFactory>(), IoC.Resolve<IStickerFactory>());

            debugStartCoroutineSubscriptionToken =
                messengerHub.Subscribe<DebugStartCoroutine>(dscr => StartCoroutine(dscr.Coroutine));
            StartCoroutine(Demo());
        }

        private IEnumerator Demo()
        {
            yield return new WaitForSeconds(3f);

            var handler = IoC.Resolve<ICommandHandler>();
            
            for (;;)
            {
                handler.UndoLast();
                
                yield return new WaitForSeconds(5f);
            }
        }

        public void HandleExecution(Func<IEnumerator> execution)
        {
            StartCoroutine(execution());
        }
    }
}