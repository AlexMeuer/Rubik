using Core.State;
using Core.TinyMessenger;
using Game.Messages;
using UnityEngine;
using ILogger = Core.Logging.ILogger;

namespace Game.Camera.States
{
    public class DraggingState : CameraControllerStateBase
    {
        private TinyMessageSubscriptionToken dragProgressSubscriptionToken;
        private TinyMessageSubscriptionToken dragEndSubscriptionToken;
        
        public DraggingState(StateContext context, ITinyMessengerHub messengerHub, ILogger logger, UnityEngine.Camera camera)
            : base(context, messengerHub, logger, camera)
        {
        }

        protected override void OnEnter()
        {
            dragProgressSubscriptionToken = MessengerHub.Subscribe<DragProgressMessage>(OnDragProgress);
            dragEndSubscriptionToken = MessengerHub.Subscribe<DragEndMessage>(OnDragEnd);
        }

        protected override void OnExit()
        {
            dragProgressSubscriptionToken.Dispose();
            dragEndSubscriptionToken.Dispose();
        }

        private void OnDragProgress(DragProgressMessage message)
        {
            var transform = Camera.transform;
            
            var dragDirection = transform.TransformDirection(message.Direction);
            
            var axis = Vector3.Cross(transform.forward, dragDirection).normalized;
            
            Camera.transform.RotateAround(Vector3.zero, axis, message.Length);
        }

        private void OnDragEnd(DragEndMessage message)
        {
            Context.TransitionTo(new WaitingForDragState(Context, MessengerHub, Logger, Camera));
        }
    }
}