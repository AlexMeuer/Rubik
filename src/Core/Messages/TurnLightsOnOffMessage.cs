using Core.TinyMessenger;

namespace Core.Messages
{
    public class TurnLightsOnOffMessage : TinyMessageBase
    {
        public bool TurnOn { get; }
        
        public TurnLightsOnOffMessage(object sender, bool turnOn) : base(sender)
        {
            TurnOn = turnOn;
        }
    }
}