using System;
using Core.TinyMessenger;
using UnityEngine;

namespace Game
{
    public class GameObjectEnableDisableToggler<T> : IDisposable where T : class, ITinyMessage
    {
        public delegate bool Predicate(T arg);

        private readonly TinyMessageSubscriptionToken subscriptionToken;
        private readonly GameObject gameObject;
        private readonly Predicate<T> predicate;
        
        public GameObjectEnableDisableToggler(ITinyMessengerHub messengerHub, GameObject gameObject, Predicate<T> predicate)
        {
            this.gameObject = gameObject;
            this.predicate = predicate;
            
            subscriptionToken = messengerHub.Subscribe<T>(HandleMessage);
        }

        private void HandleMessage(T message)
        {
            gameObject.SetActive(predicate(message));
        }

        public void Dispose()
        {
            subscriptionToken?.Dispose();
        }
    }
}