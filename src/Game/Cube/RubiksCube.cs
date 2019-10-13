using System.Collections.Generic;
using System.Linq;
using Game.Cube.Stickers;
using Game.Extensions;
using UnityEngine;

namespace Game.Cube
{
    public class RubiksCube : Instance
    {
        private readonly List<IPiece> pieces;

        // TODO: Refactor so that only 1 factory is required in the ctor.
        public RubiksCube(int cubesPerRow, IStickerDataFactory stickerDataFactory, IStickerFactory stickerFactory,
            GameObject parent = null) : base(nameof(RubiksCube), parent)
        {
            pieces = new List<IPiece>(cubesPerRow * cubesPerRow * cubesPerRow);

            // We want to center all the pieces around (0,0,0).
            var offset = 0.5f - cubesPerRow * 0.5f;

            for (var z = 0; z < cubesPerRow; ++z)
            {
                for (var y = 0; y < cubesPerRow; ++y)
                {
                    for (var x = 0; x < cubesPerRow; ++x)
                    {
                        // Map the x,y,z vector from {0..cubesPerRow} into the range {-1..1}
                        // then truncate to integers. This gives us an int vector telling us
                        // which sides of the cube should have stickers (i.e. be colored).
                        var stickerDirections = new Vector3(x, y, z)
                            .Map(0, cubesPerRow - 1, -1, 1)
                            .Truncate();

                        var piece = new Piece(stickerDataFactory.Create(stickerDirections), stickerFactory, Self)
                        {
                            Position = new Vector3(x + offset, y + offset, z + offset),
                        };

                        pieces.Add(piece);
                    }
                }
            }
        }

        protected override void OnDispose()
        {
            foreach (var piece in pieces)
            {
                piece.Dispose();
            }
            
            base.OnDispose();
        }

        public Slice FindXAxisSlice(Vector3 worldPosition)
        {
            return new Slice(
                Vector3.right,
                MatchX(worldPosition),
                ((piece, reverse) => piece.ApplyXRotationToData(reverse)));
        }

        public Slice FindYAxisSlice(Vector3 worldPosition)
        {
            return new Slice(
                Vector3.up,
                MatchY(worldPosition),
                ((piece, reverse) => piece.ApplyYRotationToData(reverse)));
        }
        
        public Slice FindZAxisSlice(Vector3 worldPosition)
        {
            return new Slice(
                Vector3.forward,
                MatchZ(worldPosition),
                ((piece, reverse) => piece.ApplyZRotationToData(reverse)));
            
        }

        private IEnumerable<IPiece> MatchX(Vector3 worldPosition)
        {
            var localPosition = Self.transform.InverseTransformPoint(worldPosition);
            return pieces.Where(p => p.Position.x.NearlyEqual(localPosition.x, 0.51f));
            // TODO: 0.51f is a bad magic number. Unreliable.
        }

        private IEnumerable<IPiece> MatchY(Vector3 worldPosition)
        {
            var localPosition = Self.transform.InverseTransformPoint(worldPosition);
            return pieces.Where(p => p.Position.y.NearlyEqual(localPosition.y, 0.51f));
            // TODO: 0.51f is a bad magic number. Unreliable.
        }

        private IEnumerable<IPiece> MatchZ(Vector3 worldPosition)
        {
            var localPosition = Self.transform.InverseTransformPoint(worldPosition);
            return pieces.Where(p => p.Position.z.NearlyEqual(localPosition.z, 0.51f));
            // TODO: 0.51f is a bad magic number. Unreliable.
        }
    }
}