using System;
using Core.Messages;
using Core.TinyMessenger;

namespace Core.Timer
{
    public class SubscribingTimer : ITimer
    {
        private readonly ITinyMessengerHub messengerHub;
        private readonly TinyMessageSubscriptionToken pauseSubscriptionToken;
        private event Action<TimeSpan> Listeners;
        private TinyMessageSubscriptionToken updateSubscriptionToken;
        private bool gamePaused;
        
        public TimeSpan Elapsed { get; private set; }

        public SubscribingTimer(ITinyMessengerHub messengerHub)
        {
            this.messengerHub = messengerHub;
            pauseSubscriptionToken = messengerHub.Subscribe<SetPausedMessage>(OnGamePauseMessage);
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
            updateSubscriptionToken?.Dispose();
            updateSubscriptionToken = messengerHub.Subscribe<UpdateMessage>(OnUpdate);
        }

        public void Restart()
        {
            Stop();
            
            Elapsed = TimeSpan.Zero;
            
            Start();
        }

        public void Stop()
        {
            updateSubscriptionToken?.Dispose();
            updateSubscriptionToken = null;
        }

        private void OnUpdate(UpdateMessage message)
        {
            if (gamePaused)
                return;
            
            Elapsed += TimeSpan.FromSeconds(message.DeltaTime);

            Listeners?.Invoke(Elapsed);
        }

        private void OnGamePauseMessage(SetPausedMessage message)
        {
            gamePaused = message.Pause;
        }
    }
}