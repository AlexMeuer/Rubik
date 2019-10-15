using Core.TinyMessenger;
using Game.Messages;
using UnityEngine;
using ILogger = Core.Logging.ILogger;

namespace Game.Camera.State
{
    public class WaitingForDragState : CameraControllerStateBase
    {
        private TinyMessageSubscriptionToken subscriptionToken;
        
        public WaitingForDragState(StateContext context, ITinyMessengerHub messengerHub, UnityEngine.Camera camera, ILogger logger)
            : base(context, messengerHub, camera, logger)
        {
        }

        public override void Enter()
        {
            subscriptionToken = MessengerHub.Subscribe<MouseDownMessage>(OnMouseDown);
        }

        public override void Exit()
        {
            subscriptionToken.Dispose();
        }

        private void OnMouseDown(MouseDownMessage message)
        {
            var ray = Camera.ScreenPointToRay(message.Position);

            // Only continue if nothing is under the ray.
            if (Physics.Raycast(ray, out var hit))
                return;
            
            Context.TransitionTo(new DraggingState(Context, MessengerHub, Camera, Logger));
        }
    }
}