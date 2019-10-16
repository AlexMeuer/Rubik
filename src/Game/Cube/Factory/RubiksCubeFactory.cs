using System.Collections.Generic;
using Core.TinyMessenger;
using UnityEngine;
using ILogger = Core.Logging.ILogger;

namespace Game.Cube.Factory
{
    public interface IRubiksCubeFactory
    {
        IRubiksCube Create(int widthHeight);
    }
    
    internal class RubiksCubeFactory : IRubiksCubeFactory
    {
        private readonly ILogger logger;
        private readonly ITinyMessengerHub messengerHub;
        private readonly IPieceFactory pieceFactory;

        public RubiksCubeFactory(ILogger logger, ITinyMessengerHub messengerHub, IPieceFactory pieceFactory)
        {
            this.logger = logger;
            this.messengerHub = messengerHub;
            this.pieceFactory = pieceFactory;
        }

        public IRubiksCube Create(int cubesPerRow)
        {
            var root = new GameObject(nameof(RubiksCube));
            
            pieceFactory.CubesPerRow = cubesPerRow;
            pieceFactory.Parent = root;
            
            var pieces = new List<IPiece>(cubesPerRow * cubesPerRow * cubesPerRow);
            
            for (var z = 0; z < cubesPerRow; ++z)
            {
                for (var y = 0; y < cubesPerRow; ++y)
                {
                    for (var x = 0; x < cubesPerRow; ++x)
                    {
                        pieces.Add(pieceFactory.Create(x, y, z));
                    }
                }
            }
            
            return new RubiksCube(root, pieces, cubesPerRow);
        }
    }
}