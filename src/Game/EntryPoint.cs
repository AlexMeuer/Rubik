using System.Collections;
using Core.IoC;
using Game.Cube;
using Game.Cube.Stickers;
using UnityEngine;

namespace Game
{
    public partial class EntryPoint
    {
        public int cubesPerRow = 3;
        public RubiksCube cube;
        
        private void Initialise()
        {
            IoC.Initialize(new ContainerRegistrations(), new Domain.ContainerRegistrations());

            cube = new RubiksCube(cubesPerRow, IoC.Resolve<IStickerDataFactory>(), IoC.Resolve<IStickerFactory>());

            StartCoroutine(Demo());
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