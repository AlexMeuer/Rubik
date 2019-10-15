using Core.TinyMessenger;

namespace Core.Messages
{
    public class SpinCamera360Message : TinyMessageBase
    {
        public float DurationSeconds { get; }
        public int NumberOfSpins { get; }

        public SpinCamera360Message(object sender, float durationSeconds, int numberOfSpins) : base(sender)
        {
            DurationSeconds = durationSeconds;
            NumberOfSpins = numberOfSpins;
        }
    }
}