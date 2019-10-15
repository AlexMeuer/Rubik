using Core.State;
using Core.TinyMessenger;
using Game.Messages;
using UnityEngine;
using ILogger = Core.Logging.ILogger;

namespace Game.Camera.States
{
    public class WaitingForDragState : CameraControllerStateBase
    {
        private TinyMessageSubscriptionToken subscriptionToken;
        
        public WaitingForDragState(StateContext context, ITinyMessengerHub messengerHub, ILogger logger, UnityEngine.Camera camera)
            : base(context, messengerHub, logger, camera)
        {
        }

        protected override void OnEnter()
        {
            subscriptionToken = MessengerHub.Subscribe<MouseDownMessage>(OnMouseDown);
        }

        protected override void OnExit()
        {
            subscriptionToken.Dispose();
        }

        private void OnMouseDown(MouseDownMessage message)
        {
            var ray = Camera.ScreenPointToRay(message.Position);

            // Only continue if nothing is under the ray.
            if (Physics.Raycast(ray, out var hit))
                return;
            
            Context.TransitionTo(new DraggingState(Context, MessengerHub, Logger, Camera));
        }
    }
}