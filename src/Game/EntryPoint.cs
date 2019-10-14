using System.Collections;
using Core.IoC;
using Core.TinyMessenger;
using Game.Cube;
using Game.Cube.Stickers;
using Game.Messages;
using UnityEngine;
using ILogger = Core.Logging.ILogger;

namespace Game
{
    public partial class EntryPoint
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
                new ContainerRegistrations());

            messengerHub = IoC.Resolve<ITinyMessengerHub>();
            
            dragListener = IoC.Resolve<IDragListener>();

            cube = new RubiksCube(cubesPerRow,IoC.Resolve<ILogger>(),  messengerHub, IoC.Resolve<IStickerDataFactory>(), IoC.Resolve<IStickerFactory>());

            debugStartCoroutineSubscriptionToken =
                messengerHub.Subscribe<DebugStartCoroutine>(dscr => StartCoroutine(dscr.Coroutine));
//            StartCoroutine(Demo());
        }

        private IEnumerator Demo()
        {
            yield return new WaitForSeconds(3f);
            
            for (;;)
            {
                var slice = cube.FindXAxisSlice(Vector3.one * 0.5f);
                yield return slice.Rotate90Degrees(false);
                slice = cube.FindYAxisSlice(Vector3.one * 0.5f);
                yield return slice.Rotate90Degrees(false);
                slice = cube.FindZAxisSlice(Vector3.one * 0.5f);
                yield return slice.Rotate90Degrees(false);
                slice = cube.FindXAxisSlice(Vector3.one * 1.5f);
                yield return slice.Rotate90Degrees(false);
                slice = cube.FindYAxisSlice(Vector3.one * -1.5f);
                yield return slice.Rotate90Degrees(false);
                slice = cube.FindZAxisSlice(Vector3.one * -2.5f);
                yield return slice.Rotate90Degrees(false);
            }
        }
    }
}