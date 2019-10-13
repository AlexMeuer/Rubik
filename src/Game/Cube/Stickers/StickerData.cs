using Domain;
using UnityEngine;

namespace Game.Cube.Stickers
{
    public struct StickerData
    {
        public Vector3Int Directions;
        public FaceColor X;
        public FaceColor Y;
        public FaceColor Z;

        public StickerData(Vector3Int directions,
            FaceColor x = FaceColor.None,
            FaceColor y = FaceColor.None,
            FaceColor z = FaceColor.None)
        {
            Directions = directions;
            X = x;
            Y = y;
            Z = z;
        }
    }
}