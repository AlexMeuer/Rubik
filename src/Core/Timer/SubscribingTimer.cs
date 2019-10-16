using System;
using Core.Messages;
using Core.TinyMessenger;

namespace Core.Timer
{
    public class SubscribingTimer : ITimer
    {
        private readonly ITinyMessengerHub messengerHub;
        private event Action<TimeSpan> Listeners;
        private TinyMessageSubscriptionToken subscriptionToken;
        
        public TimeSpan Elapsed { get; private set; }

        public SubscribingTimer(ITinyMessengerHub messengerHub)
        {
            this.messengerHub = messengerHub;
        }
        
        public void AddListener(Action<TimeSpan> onUpdate, bool invokeNow = true)
        {
            Listeners += onUpdate;

            if (invokeNow)
                onUpdate(Elapsed);
        }

        public void RemoveListener(Action<TimeSpan> onUpdate)
        {
            Listeners -= onUpdate;
        }

        public void Start()
        {
            subscriptionToken?.Dispose();
            subscriptionToken = messengerHub.Subscribe<UpdateMessage>(OnUpdate);
        }

        public void Restart()
        {
            Stop();
            
            Elapsed = TimeSpan.Zero;
            
            Start();
        }

        public void Stop()
        {
            subscriptionToken?.Dispose();
            subscriptionToken = null;
        }

        private void OnUpdate(UpdateMessage message)
        {
            Elapsed += TimeSpan.FromSeconds(message.DeltaTime);

            Listeners?.Invoke(Elapsed);
        }
    }
}