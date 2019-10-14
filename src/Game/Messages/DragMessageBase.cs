using Core.TinyMessenger;
using UnityEngine;

namespace Game.Messages
{
    public abstract class DragMessageBase : TinyMessageBase
    {
        public Vector3 StartPosition { get; }
        public Vector3 EndPosition { get; }
        public Vector3 Displacement => EndPosition - StartPosition;
        public Vector3 Direction => Displacement.normalized;
        public float Length => Displacement.magnitude;
        
        protected DragMessageBase(object sender, Vector3 start, Vector3 end) : base(sender)
        {
            StartPosition = start;
            EndPosition = end;
        }
    }
}