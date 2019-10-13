using Domain;
using UnityEngine;

namespace Game.Cube.Stickers
{
    public interface IStickerDataFactory
    {
        StickerData Create(Vector3Int directions);
    }
    
    internal class StickerDataFactory : IStickerDataFactory
    {
        private readonly IColorMap colorMap;

        public StickerDataFactory(IColorMap colorMap)
        {
            this.colorMap = colorMap;
        }

        public StickerData Create(Vector3Int directions)
        {
            return new StickerData(
                directions,
                colorMap.MapX(directions.x),
                colorMap.MapY(directions.y),
                colorMap.MapZ(directions.z));
        }
    }
}