using UnityEngine;

namespace Game.Messages
{
    public class DragProgressMessage : DragMessageBase
    {
        public DragProgressMessage(object sender, Vector3 start, Vector3 end) : base(sender, start, end)
        {
        }
    }
}