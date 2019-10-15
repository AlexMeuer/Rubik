using System;
using Core.Logging;
using Core.State;
using Core.TinyMessenger;
using Game.Camera.State;
using Game.Messages;

namespace Game.Camera
{
    public class CameraController : IDisposable
    {
        private readonly ITinyMessengerHub messengerHub;
        private readonly ILogger logger;
        private readonly StateContext context;
        private TinyMessageSubscriptionToken subscriptionToken;
        
        public CameraController(UnityEngine.Camera camera, ITinyMessengerHub messengerHub, ILogger logger)
        {
            this.messengerHub = messengerHub;
            this.logger = PrefixedLogger.ForType<CameraController>(logger);
            
            context = new StateContext(this.logger);
            
            context.TransitionTo(
                new WaitingForDragState(context,
                                        messengerHub,
                                        this.logger,
                                        camera));

            subscriptionToken = messengerHub.Subscribe<DisableCameraControlMessage>(DisableControl);
        }

        public void Dispose()
        {
            context.Dispose();
        }

        private void DisableControl(DisableCameraControlMessage message)
        {
            context.Disable();
            
            messengerHub.Unsubscribe<DisableCameraControlMessage>(subscriptionToken);

            subscriptionToken = messengerHub.Subscribe<EnableCameraControlMessage>(EnableControl);
            
            logger.Info("Disabled camera control");
        }

        private void EnableControl(EnableCameraControlMessage message)
        {
            context.Enable();
            
            messengerHub.Unsubscribe<DisableCameraControlMessage>(subscriptionToken);

            subscriptionToken = messengerHub.Subscribe<DisableCameraControlMessage>(DisableControl);
            
            logger.Info("Enabled camera control");
        }
    }
}