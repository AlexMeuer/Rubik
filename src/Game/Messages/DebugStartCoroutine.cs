using System.Collections;
using Core.TinyMessenger;

namespace Game.Messages
{
    public class DebugStartCoroutine : TinyMessageBase
    {
        public IEnumerator Coroutine { get; }
        
        public DebugStartCoroutine(object sender, IEnumerator coroutine) : base(sender)
        {
            Coroutine = coroutine;
        }
    }
}