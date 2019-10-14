using System;
using System.Collections.Generic;
using System.Linq;
using Core.IoC;
using Core.TinyMessenger;
using Game.Cube.Stickers;
using Game.Extensions;
using Game.Messages;
using UnityEngine;
using ILogger = Core.Logging.ILogger;

namespace Game.Cube
{
    public interface IRubiksCube : IInstance
    {
        Slice FindXAxisSlice(Vector3 localPosition);
        Slice FindYAxisSlice(Vector3 localPosition);
        Slice FindZAxisSlice(Vector3 localPosition);
    }
    
    public class RubiksCube : Instance, IRubiksCube
    {
        private readonly ILogger logger;
        private readonly ITinyMessengerHub messengerHub;
        private readonly List<IPiece> pieces;
        private readonly TinyMessageSubscriptionToken dragEndSubscriptionToken;

        // TODO: Refactor so that only 1 factory is required in the ctor.
        public RubiksCube(int cubesPerRow, ILogger logger, ITinyMessengerHub messengerHub, IStickerDataFactory stickerDataFactory, IStickerFactory stickerFactory,
            GameObject parent = null) : base(nameof(RubiksCube), parent)
        {
            this.logger = logger;
            this.messengerHub = messengerHub;
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

            dragEndSubscriptionToken = messengerHub.Subscribe<DragEndMessage>(OnDragEnd);
        }

        private void OnDragEnd(DragEndMessage drag)
        {
            //    D    R    A    F    T    \\
           
            // TODO: get world pos of start. check if in cube. rotate slice in direction
            // TODO LATER: refactor this out to a different class. The cube doesn't need to listen itself does it?
            var camera = IoC.Resolve<Camera>();
            var ray = camera.ScreenPointToRay(drag.StartPosition);

            if (!Physics.Raycast(ray, out var hit)) return;
            
            var piece = pieces.First(p => p.Is(hit.transform.gameObject));
            
            var face = InverseTransformPoint(hit.point);//.ToIntegerAxis();
            
            var direction = InverseTransformDirection(camera.transform.TransformDirection(drag.Direction)).ToIntegerAxis();
            
            var axis = Vector3.Cross(face, direction);//.ToIntegerAxis();
            
            logger.Info("Direction: {1}, Axis: {2}", direction, axis);

            var spinAxis = axis.ToIntegerAxis();
            if (spinAxis.y != 0)
            {
                var sign = Math.Sign(direction.x);
                if (sign == 0) 
                    sign = Math.Sign(direction.z);
                
                messengerHub.Publish(new DebugStartCoroutine(this,
                    FindYAxisSlice(piece.Position).Rotate90Degrees(sign > 0)));
                
            } else if (spinAxis.x != 0)
            {
                var sign = Math.Sign(direction.y);
                if (sign == 0) 
                    sign = Math.Sign(direction.z);
                
                messengerHub.Publish(new DebugStartCoroutine(this,
                    FindXAxisSlice(piece.Position).Rotate90Degrees(sign < 0)));
                
            } else if (spinAxis.z != 0)
            {
                var sign = Math.Sign(direction.x);
                if (sign == 0) 
                    sign = Math.Sign(direction.y);
                
                messengerHub.Publish(new DebugStartCoroutine(this,
                    FindZAxisSlice(piece.Position).Rotate90Degrees(sign < 0)));
                
            }
            
            UnityEngine.Debug.DrawRay(this.WorldPosition, spinAxis * 10, Color.green, 10f, false);
        }

        protected override void OnDispose()
        {
            foreach (var piece in pieces)
            {
                piece.Dispose();
            }
            
            base.OnDispose();
        }

        public Slice FindXAxisSlice(Vector3 localPosition)
        {
            return new Slice(
                Self.transform.right,
                MatchX(localPosition),
                ((piece, reverse) => piece.ApplyXRotationToData(reverse)));
        }

        public Slice FindYAxisSlice(Vector3 localPosition)
        {
            return new Slice(
                Self.transform.up,
                MatchY(localPosition),
                ((piece, reverse) => piece.ApplyYRotationToData(reverse)));
        }
        
        public Slice FindZAxisSlice(Vector3 localPosition)
        {
            return new Slice(
                Self.transform.forward,
                MatchZ(localPosition),
                ((piece, reverse) => piece.ApplyZRotationToData(reverse)));
            
        }

        private IEnumerable<IPiece> MatchX(Vector3 localPosition)
        {
            return pieces.Where(p => p.Position.x.NearlyEqual(localPosition.x, 0.51f));
            // TODO: 0.51f is a bad magic number. Unreliable.
        }

        private IEnumerable<IPiece> MatchY(Vector3 localPosition)
        {
            return pieces.Where(p => p.Position.y.NearlyEqual(localPosition.y, 0.51f));
            // TODO: 0.51f is a bad magic number. Unreliable.
        }

        private IEnumerable<IPiece> MatchZ(Vector3 localPosition)
        {
            return pieces.Where(p => p.Position.z.NearlyEqual(localPosition.z, 0.51f));
            // TODO: 0.51f is a bad magic number. Unreliable.
        }
    }
}