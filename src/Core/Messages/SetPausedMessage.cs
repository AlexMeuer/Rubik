using Core.TinyMessenger;

namespace Core.Messages
{
    public class SetPausedMessage : TinyMessageBase
    {
        public bool Pause { get; }
        
        public SetPausedMessage(object sender, bool pause) : base(sender)
        {
            Pause = pause;
        }
    }
}