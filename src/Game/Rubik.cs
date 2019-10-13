using Core.IoC;
using Game.Extensions;
using UnityEngine;
using ILogger = Core.Logging.ILogger;

namespace Game
{
    public class Rubik : Instance
    {
        public Rubik(int cubesPerRow, GameObject parent = null) : base(nameof(Rubik), parent)
        {
            var logger = IoC.Resolve<ILogger>();
            var offset = 0.5f - cubesPerRow * 0.5f;
            
            for (var z = 0; z < cubesPerRow; ++z)
            {
                for (var y = 0; y < cubesPerRow; ++y)
                {
                    for (var x = 0; x < cubesPerRow; ++x)
                    {                        
                        var stickerDirections = new Vector3(x, y, z)
                            .Map(0, cubesPerRow-1, -1, 1)
                            .Truncate();
                        
                        var piece = new Piece(stickerDirections, Self)
                        {
                            Position = new Vector3(x + offset, y + offset, z + offset),
                        };
                    }
                }
            }
        }
    }
}