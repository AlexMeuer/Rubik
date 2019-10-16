using System.Linq;
using Domain;
using UnityEngine;

namespace Game.Cube
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

        public string Serialize() => $"{Directions.x},{Directions.y},{Directions.z},{(int)X},{(int)Y},{(int)Z}";

        public static StickerData Deserialize(string s)
        {
            var d = s.Split(',').Select(int.Parse).ToArray();
            
            return new StickerData(new Vector3Int(d[0], d[1], d[2]),
                                   (FaceColor) d[3],
                                   (FaceColor) d[4],
                                   (FaceColor) d[5]);
        }
    }
}