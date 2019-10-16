using System;
using Core.TinyMessenger;

namespace Core.Messages
{
    public abstract class OnCompleteMessage : TinyMessageBase
    {
        public Action OnComplete { get; }
        
        protected OnCompleteMessage(object sender, Action onComplete) : base(sender)
        {
            OnComplete = onComplete;
        }
    }
}