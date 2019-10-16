using System;

namespace Core.Messages
{
    public class CameraOrbitRequestMessage : OnCompleteMessage
    {
        public int Times { get; }
        
        public CameraOrbitRequestMessage(object sender, int times, Action onComplete = null) : base(sender, onComplete)
        {
            Times = times;
        }
    }
}