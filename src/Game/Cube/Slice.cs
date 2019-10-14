using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Cube
{
    public class Slice
    {
        public delegate void RotationStartListener(IPiece piece, bool reverse);

        private readonly IEnumerable<IPiece> pieces;
        private readonly Vector3 axis;

        private readonly RotationStartListener rotationStartListener;

        public Slice(Vector3 axis, IEnumerable<IPiece> pieces, RotationStartListener rotationStartListener)
        {
#if DEBUG
            // ReSharper disable JoinNullCheckWithUsage
            if (pieces == null) throw new ArgumentNullException(nameof(pieces));
            if (rotationStartListener == null) throw new ArgumentNullException(nameof(rotationStartListener));
            // ReSharper restore JoinNullCheckWithUsage
#endif
            this.axis = axis;
            this.pieces = pieces;
            this.rotationStartListener = rotationStartListener;
        }

        public IEnumerator Rotate90Degrees(bool reverse)
        {
            var piecesToRotate = pieces.ToArray();

            foreach (var piece in piecesToRotate)
            {
                rotationStartListener(piece, reverse);
            }
            
            var angle = reverse ? -3 : 3;

            for (var i = 0; i < 30; ++i)
            {
                foreach (var piece in piecesToRotate)
                {
                    piece.RotateAbout(axis, angle);
                }

                // TODO: Configurable duration
                yield return new WaitForFixedUpdate();
            }
        }
    }
}