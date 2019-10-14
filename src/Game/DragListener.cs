using Core.IoC;
using Core.TinyMessenger;
using Game.Messages;
using UnityEngine;
using ILogger = Core.Logging.ILogger;

namespace Game
{
    public interface IDragListener
    {
        void Poll();
    }
    
    public class MouseDragListener : IDragListener
    {
        private const float UpdateTolerance = 5f;
        private readonly ITinyMessengerHub messengerHub;
        private Vector3 startPosition;
        private Vector3 lastUpdatePosition;
        private bool mouseIsDown;

        public MouseDragListener(ITinyMessengerHub messengerHub)
        {
            this.messengerHub = messengerHub;
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
            else if (mouseIsDown && (Input.mousePosition - lastUpdatePosition).magnitude > UpdateTolerance)
            {
                lastUpdatePosition = Input.mousePosition;
                
                messengerHub.Publish(new DragProgressMessage(this, startPosition, Input.mousePosition));
            }
        }
    }
}