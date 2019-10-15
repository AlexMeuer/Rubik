using Game.Extensions;
using UnityEngine;

namespace Game.Cube.Factory
{
    public interface IPieceFactory
    {
        /// <summary>
        /// The number of cubes per row requested for the complete RubiksCube. Used to determine sticker directions.
        /// </summary>
        int CubesPerRow { set; }
        
        /// <summary>
        /// The parent of each piece created.
        /// </summary>
        GameObject Parent { set; }
        
        IPiece Create(int x, int y, int z);
    }
    
    internal class PieceFactory : IPieceFactory
    {
        private readonly IStickerFactory stickerFactory;
        private readonly IStickerDataFactory stickerDataFactory;
        private float offset;
        private int cubesPerRow;

        public int CubesPerRow
        {
            set
            {
                cubesPerRow = value;
                
                // We want to center all the pieces around (0,0,0).
                // and we know that each piece will be 1x1x1.
                offset = 0.5f - value * 0.5f;
            }
        }

        public GameObject Parent { private get; set; }

        public PieceFactory(IStickerFactory stickerFactory, IStickerDataFactory stickerDataFactory)
        {
            this.stickerFactory = stickerFactory;
            this.stickerDataFactory = stickerDataFactory;
        }

        public IPiece Create(int x, int y, int z)
        {
            // Map the x,y,z vector from {0..cubesPerRow} into the range {-1..1}
            // then truncate to integers. This gives us an int vector telling us
            // which sides of the cube should have stickers (i.e. be colored).
            var stickerDirections = new Vector3(x, y, z)
                .Map(0, cubesPerRow - 1, -1, 1)
                .Truncate();

            var stickerData = stickerDataFactory.Create(stickerDirections);

            var piece = new Piece(stickerDataFactory.Create(stickerDirections), Parent);
            
            stickerFactory.Create(stickerData, piece);

            piece.Position = new Vector3(x + offset, y + offset, z + offset);

            return piece;
        }
    }
}