using System.Collections.Generic;
using UnityEngine;

namespace Game.Cube.Factory
{
    public interface IRubiksCubeFactory
    {
        IRubiksCube Create(int widthHeight);
        IRubiksCube Create(IEnumerable<StickerData> stickerData);
    }
    
    internal class RubiksCubeFactory : IRubiksCubeFactory
    {
        private readonly IPieceFactory pieceFactory;

        public RubiksCubeFactory(IPieceFactory pieceFactory)
        {
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

        public IRubiksCube Create(IEnumerable<StickerData> stickerData)
        {
            throw new System.NotImplementedException();
        }
    }
}