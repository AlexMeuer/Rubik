using System;
using System.Collections.Generic;
using System.Linq;
using Core.Command.Messages;
using Core.Extensions;
using Core.IoC;
using Core.TinyMessenger;
using Game.Command;
using Game.Extensions;
using Game.Messages;
using UnityEngine;
using ILogger = Core.Logging.ILogger;

namespace Game.Cube
{
    public interface IRubiksCube : IInstance
    {
        int PiecesPerRow { get; }
        
        Slice FindXAxisSlice(Vector3 localPosition);
        Slice FindYAxisSlice(Vector3 localPosition);
        Slice FindZAxisSlice(Vector3 localPosition);

        void AcceptDragInput(DragEndMessage message);

        IEnumerable<StickerData> GetStickerData();
    }
    
    public class RubiksCube : Instance, IRubiksCube
    {
        private const float MinimumDragDistance = 30f;
        private const float PositionMatchingTolerance = 0.51f;
        
        private readonly ILogger logger;
        private readonly ITinyMessengerHub messengerHub;
        private readonly IEnumerable<IPiece> pieces;

        public int PiecesPerRow { get; }

        public RubiksCube(GameObject gameObject, IEnumerable<IPiece> pieces, int piecesPerRow, ILogger logger, ITinyMessengerHub messengerHub)
            : base(nameof(RubiksCube), gameObject, null)
        {
            this.pieces = pieces;
            this.logger = logger;
            this.messengerHub = messengerHub;
            PiecesPerRow = piecesPerRow;
        }

        public void AcceptDragInput(DragEndMessage drag)
        {
            if (drag.Length < MinimumDragDistance)
                return;
            
            var camera = IoC.Resolve<UnityEngine.Camera>(); // TODO: Remove use of IoC in methods
            var ray = camera.ScreenPointToRay(drag.StartPosition);

            if (!Physics.Raycast(ray, out var hit)) return;
            
            var piece = pieces.First(p => p.Is(hit.transform.gameObject));
            
            var face = InverseTransformPoint(hit.point);
            
            var direction = InverseTransformDirection(camera.transform.TransformDirection(drag.Direction)).ToIntegerAxis();
            
            var axis = Vector3.Cross(face, direction).ToIntegerAxis();
            
            // logger.Info("Direction: {0}, Axis: {1}", direction, axis);

            RotateSliceCommand cmd = null;

            if (axis.y != 0)
            {
                var sign = Math.Sign(direction.x);
                if (sign == 0) 
                    sign = Math.Sign(direction.z);
                
                // We flip the comparison on this one because of the positive direction on Z in unity.
                cmd = new RotateSliceCommand(FindYAxisSlice(piece.Position), sign > 0);
                
            } else if (axis.x != 0)
            {
                var sign = Math.Sign(direction.y);
                if (sign == 0) 
                    sign = Math.Sign(direction.z);
                
                cmd =  new RotateSliceCommand(FindXAxisSlice(piece.Position), sign < 0);
                
            } else if (axis.z != 0)
            {
                var sign = Math.Sign(direction.x);
                if (sign == 0) 
                    sign = Math.Sign(direction.y);
                
                cmd = new RotateSliceCommand(FindZAxisSlice(piece.Position), sign < 0);
                
            }
            
            if (cmd != null)
                messengerHub.Publish(new EnqueueCommandMessage(this, cmd));
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

        public IEnumerable<StickerData> GetStickerData()
        {
            return pieces.Select(p => p.StickerData);
        }

        protected override void OnDispose()
        {
            foreach (var piece in pieces)
            {
                piece.Dispose();
            }
            
            base.OnDispose();
        }

        private IEnumerable<IPiece> MatchX(Vector3 localPosition)
        {
            return pieces.Where(p => p.Position.x.NearlyEqual(localPosition.x, PositionMatchingTolerance));
        }

        private IEnumerable<IPiece> MatchY(Vector3 localPosition)
        {
            return pieces.Where(p => p.Position.y.NearlyEqual(localPosition.y, PositionMatchingTolerance));
        }

        private IEnumerable<IPiece> MatchZ(Vector3 localPosition)
        {
            return pieces.Where(p => p.Position.z.NearlyEqual(localPosition.z, PositionMatchingTolerance));
        }
    }
}