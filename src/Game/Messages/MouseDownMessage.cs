using Core.TinyMessenger;
using UnityEngine;

namespace Game.Messages
{
    public class MouseDownMessage : TinyMessageBase
    {
        private Vector3 Position { get; }
        
        public MouseDownMessage(object sender, Vector3 position) : base(sender)
        {
            Position = position;
        }
    }
}