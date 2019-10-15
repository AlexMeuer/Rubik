using Core.Extensions;
using UnityEngine;

namespace Game.Cube
{
    public interface IPiece : IInstance
    {
        StickerData StickerData { get; }
        void ApplyXRotationToData(bool reverse);
        void ApplyYRotationToData(bool reverse);
        void ApplyZRotationToData(bool reverse);
    }
    
    internal class Piece : Instance, IPiece
    {
        private StickerData stickerData;

        public StickerData StickerData => stickerData;

        private static string DebugName(StickerData sd) => "{0} {1}".Format(nameof(Piece), sd.Directions);

        public Piece(StickerData stickerData, GameObject parent = null)
            : base(DebugName(stickerData), GameObject.CreatePrimitive(PrimitiveType.Cube), parent)
        {
            this.stickerData = stickerData;

            Self.GetComponent<Renderer>().material.color = new Color(0.3f, 0.3f, 0.3f);
        }

        public void ApplyXRotationToData(bool reverse)
        {
            var oldData = stickerData;
            if (reverse)
            {
                stickerData.Directions.y = oldData.Directions.z;
                stickerData.Directions.z = -oldData.Directions.y;
            }
            else
            {
                // We're always rotating 90 degrees, therefore we can short-circuit the formula.
                stickerData.Directions.y = -oldData.Directions.z;
                stickerData.Directions.z = oldData.Directions.y;
            }

            stickerData.Y = oldData.Z;
            stickerData.Z = oldData.Y;

            Self.name = DebugName(stickerData);
        }
        
        public void ApplyYRotationToData(bool reverse)
        {
            var oldData = stickerData;
            if (reverse)
            {
                stickerData.Directions.x = -oldData.Directions.z;
                stickerData.Directions.z = oldData.Directions.x;
            }
            else
            {
                stickerData.Directions.x = oldData.Directions.z;
                stickerData.Directions.z = -oldData.Directions.x;
            }

            stickerData.X = oldData.Z;
            stickerData.Z = oldData.X;

            Self.name = DebugName(stickerData);
        }
        
        public void ApplyZRotationToData(bool reverse)
        {
            var oldData = stickerData;
            if (reverse)
            {
                stickerData.Directions.x = oldData.Directions.y;
                stickerData.Directions.y = -oldData.Directions.x;
            }
            else
            {
                stickerData.Directions.x = -oldData.Directions.y;
                stickerData.Directions.y = oldData.Directions.x;
            }

            stickerData.X = oldData.Y;
            stickerData.Y = oldData.X;

            Self.name = DebugName(stickerData);
        }
    }
}