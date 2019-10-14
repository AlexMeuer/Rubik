using Core.TinyMessenger;
using UnityEngine;

namespace Game.Messages
{
    public class DragEndMessage : DragMessageBase
    {
        public DragEndMessage(object sender, Vector3 start, Vector3 end) : base(sender, start, end)
        {
        }
    }
}