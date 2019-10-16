using System;
using Core.TinyMessenger;

namespace Domain.Messages
{
    public class CubeSolvedMessage : TinyMessageBase
    {
        public TimeSpan TimeTaken { get; }
        
        public CubeSolvedMessage(object sender, TimeSpan timeTaken) : base(sender)
        {
            TimeTaken = timeTaken;
        }
    }
}