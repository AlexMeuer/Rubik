using System.Collections.Generic;
using Domain;
using Game.Extensions;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.Cube.Factory
{
    public interface IStickerFactory
    {
        IEnumerable<GameObject> Create(StickerData data, GameObject parent);
    }

    internal class StickerFactory : IStickerFactory
    {   
        public IEnumerable<GameObject> Create(StickerData data, GameObject parent)
        {
            var parentTransform = parent.transform;
            
            var stickers = new List<GameObject>();
            
            if (data.X != FaceColor.None)
                stickers.Add(Create(Vector3.right,   data.Directions.x, data.X, parentTransform));
            
            if (data.Y != FaceColor.None)
                stickers.Add(Create(Vector3.up,      data.Directions.y, data.Y, parentTransform));
            
            if (data.Z != FaceColor.None)
                stickers.Add(Create(Vector3.forward, data.Directions.z, data.Z, parentTransform));

            return stickers;
        }

        private static GameObject Create(Vector3 direction, int multiplier, FaceColor faceColor, Transform parent)
        {
            var sticker = GameObject.CreatePrimitive(PrimitiveType.Cube);

            sticker.name = faceColor.ToString();
            
            sticker.transform.SetParent(parent);
            
            Object.Destroy(sticker.GetComponent<BoxCollider>());
            
            sticker.transform.localScale = new Vector3(0.8f, 0.8f, 0.1f);
            
            sticker.transform.SetPositionAndRotation(
                direction * multiplier * 0.5f,
                Quaternion.LookRotation(direction));
            
            sticker.GetComponent<Renderer>().material.color = faceColor.ToColor();
            
            return sticker;
        }
    }
}