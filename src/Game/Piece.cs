using Core.Extensions;
using UnityEngine;

namespace Game
{
    public class Piece : Instance
    {
        public Vector3Int StickerDirections { get; }
        
        public Piece(Vector3Int stickerDirections, GameObject parent = null)
            : base("{0} {1}".Format(nameof(Piece), stickerDirections), parent)
        {
            StickerDirections = stickerDirections;

            SetAsChild(GameObject.CreatePrimitive(PrimitiveType.Cube));
        }
    }
}