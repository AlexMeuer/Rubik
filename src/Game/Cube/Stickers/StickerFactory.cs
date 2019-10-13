using System.Collections.Generic;
using Domain;
using Game.Extensions;
using UnityEngine;

namespace Game.Cube.Stickers
{
    public interface IStickerFactory
    {
        IEnumerable<GameObject> Create(StickerData data);
    }

    internal class StickerFactory : IStickerFactory
    {   
        public IEnumerable<GameObject> Create(StickerData data)
        {
            var stickers = new List<GameObject>();
            
            if (data.X != FaceColor.None)
                stickers.Add(Create(Vector3.right,   data.Directions.x, data.X));
            
            if (data.Y != FaceColor.None)
                stickers.Add(Create(Vector3.up,      data.Directions.y, data.Y));
            
            if (data.Z != FaceColor.None)
                stickers.Add(Create(Vector3.forward, data.Directions.z, data.Z));

            return stickers;
        }

        private static GameObject Create(Vector3 direction, int multiplier, FaceColor faceColor)
        {
            var sticker = GameObject.CreatePrimitive(PrimitiveType.Cube);
            
            sticker.transform.localScale = new Vector3(0.8f, 0.8f, 0.1f);
            
            sticker.transform.SetPositionAndRotation(
                direction * multiplier * 0.5f,
                Quaternion.LookRotation(direction));
            
            sticker.GetComponent<Renderer>().material.color = faceColor.ToColor();
            
            return sticker;
        }
    }
}