using Core.TinyMessenger;

namespace Core.Messages
{
    public class UpdateMessage : TinyMessageBase
    {
        public float DeltaTime { get; }

        public UpdateMessage(object sender, float deltaTime) : base(sender)
        {
            DeltaTime = deltaTime;
        }
    }
}