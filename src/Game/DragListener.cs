using System;
using Core.Messages;
using Core.TinyMessenger;
using Game.Messages;
using UnityEngine;

namespace Game
{
    public interface IDragListener : IDisposable
    {
        void Poll();
    }
    
    public class MouseDragListener : IDragListener
    {
        private readonly ITinyMessengerHub messengerHub;
        private readonly TinyMessageSubscriptionToken updateSubscriptionToken;
        private Vector3 startPosition;
        private Vector3 lastUpdatePosition;
        private bool mouseIsDown;

        public MouseDragListener(ITinyMessengerHub messengerHub)
        {
            this.messengerHub = messengerHub;
            updateSubscriptionToken = messengerHub.Subscribe<UpdateMessage>(m => Poll());
        }

        public void Poll()
        {
            if (Input.GetMouseButtonDown(0))
            {
                startPosition = lastUpdatePosition = Input.mousePosition;

                mouseIsDown = true;
                
                messengerHub.Publish(new MouseDownMessage(this, startPosition));
            }
            else if (Input.GetMouseButtonUp(0))
            {
                mouseIsDown = false;
                
                messengerHub.Publish(new DragEndMessage(this, startPosition, Input.mousePosition));
            }
            else if (mouseIsDown)
            {
                messengerHub.Publish(new DragProgressMessage(this, lastUpdatePosition, Input.mousePosition));
                
                lastUpdatePosition = Input.mousePosition;
            }
        }

        public void Dispose()
        {
            updateSubscriptionToken.Dispose();
        }
    }
}