using Core.TinyMessenger;
using Game.Messages;
using UnityEngine;
using ILogger = Core.Logging.ILogger;

namespace Game.Camera.State
{
    public class DraggingState : CameraControllerStateBase
    {
        private TinyMessageSubscriptionToken dragProgressSubscriptionToken;
        private TinyMessageSubscriptionToken dragEndSubscriptionToken;
        
        public DraggingState(StateContext context, ITinyMessengerHub messengerHub, UnityEngine.Camera camera, ILogger logger)
            : base(context, messengerHub, camera, logger)
        {
        }

        public override void Enter()
        {
            dragProgressSubscriptionToken = MessengerHub.Subscribe<DragProgressMessage>(OnDragProgress);
            dragEndSubscriptionToken = MessengerHub.Subscribe<DragEndMessage>(OnDragEnd);
        }

        public override void Exit()
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
            Context.TransitionTo(new WaitingForDragState(Context, MessengerHub, Camera, Logger));
        }
    }
}